using Microsoft.Extensions.DependencyInjection.Extensions;
using financial_control.Infrastructure.Context;
using financial_control_infrastructure.Message;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Mvc.Testing;
using DotNet.Testcontainers.Builders;
using financial_control.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Testcontainers.RabbitMq;
using RabbitMQ.Client;
using Moq;
using Microsoft.AspNetCore.TestHost;

namespace financial_control_integration_test.Configuration;

public class ApiFactory : WebApplicationFactory<IInitialProject>, IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitMqContainer =
        new RabbitMqBuilder()
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
            .Build();

    public readonly string Queue = "queue-test";
    public readonly string Exchange = "exchange-test";
    public required ConsumerService ConsumerService { get; set; }
    public required ConnectionFactory connectionFactory { get; set; }
    private Mock<ILogger<ConsumerService>> LoggerConsumer { get; set; } = new Mock<ILogger<ConsumerService>>();
    public async Task InitializeAsync()
    {
        
        await _rabbitMqContainer.StartAsync();
        ConsumerService = new ConsumerService(
            LoggerConsumer.Object,
            new ConfigurationConnection(
                _rabbitMqContainer.GetConnectionString(),
                Queue,
                Exchange
            )
        );
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureTestServices(services =>
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMqContainer.GetConnectionString())
            };
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
                    connectionFactory
                    )
                );

            services.Remove<IConnectionFactory>();
            services.AddSingleton<IConnectionFactory>(connectionFactory);
        });
        builder.UseEnvironment("Testing");
        base.ConfigureWebHost(builder);
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return _rabbitMqContainer.DisposeAsync().AsTask();
    }
}
