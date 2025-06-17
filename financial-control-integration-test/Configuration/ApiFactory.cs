using financial_control_domain.Interfaces.Repositories;
using financial_control.Infrastructure.Context;
using financial_control_infrastructure.Message;
using Microsoft.Extensions.DependencyInjection;
using financial_control_application.Services;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Mvc.Testing;
using DotNet.Testcontainers.Builders;
using financial_control.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Testcontainers.RabbitMq;
using RabbitMQ.Client;
using Moq;

namespace financial_control_integration_test.Configuration;

public class ApiFactory : WebApplicationFactory<IApiConfiguration>, IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitMqContainer =
        new RabbitMqBuilder()
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
            .Build();

    public readonly string Queue = "queue-test";
    public readonly string Exchange = "exchange-test";
    public required ConsumerService ConsumerService { get ; set; }

    private readonly Mock<IPersonRepository> PersonRepositoryMock = new();

    private ConnectionFactory? _connectionFactory;
    public ConnectionFactory ConnectionFactory {
        get {
            _connectionFactory ??= GetConnection();   
            return _connectionFactory;
        }
        set {
            _connectionFactory = value;
        }
    }
    
    private Mock<ILogger<ConsumerService>> LoggerConsumer { get; set; } = new Mock<ILogger<ConsumerService>>();

    private ConnectionFactory GetConnection() => new ConnectionFactory
    {
        Uri = new Uri(_rabbitMqContainer.GetConnectionString())
    };

    public async Task<IChannel> GetChannelAsync()
    {
        var connection = await ConnectionFactory.CreateConnectionAsync();
        return await connection.CreateChannelAsync();
    }

    public async Task InitializeAsync()
    {
        await _rabbitMqContainer.StartAsync();
        // ConsumerService = new ConsumerService(
        //     LoggerConsumer.Object,
        //     new ConfigurationConnection(
        //         _rabbitMqContainer.GetConnectionString(),
        //         Queue,
        //         Exchange
        //     ),
        //     new PersonService(PersonRepositoryMock.Object)
        // );
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureTestServices(services =>
        {
            var configurationConnection = new ConfigurationConnection
            (
                _rabbitMqContainer.GetConnectionString(),
                Queue,
                Exchange
            );

            services.Remove<DbContextOptions<DbFinancialContext>>();
            services.AddDbContext<DbFinancialContext>(options =>
            {
                options.UseInMemoryDatabase("FinancialDb", root);
            });

            services.Remove<ConfigurationConnection>();
            services.AddSingleton(configurationConnection);

            services.Remove<PublisherService>();
            services.AddSingleton(x =>
                new PublisherService(
                    new ConfigurationConnection(_rabbitMqContainer.GetConnectionString(), Queue, Exchange),
                    new Mock<ILogger<PublisherService>>().Object,
                    ConnectionFactory
                    )
                );

            services.Remove<IConnectionFactory>();
            services.AddSingleton<IConnectionFactory>(ConnectionFactory);

            ConsumerService = new ConsumerService(
                LoggerConsumer.Object,
                new ConfigurationConnection(
                    _rabbitMqContainer.GetConnectionString(),
                    Queue,
                    Exchange
                ),
                services.BuildServiceProvider()
            );
        });
        builder.UseEnvironment("Testing");
        base.ConfigureWebHost(builder);
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return _rabbitMqContainer.DisposeAsync().AsTask();
    }
}
