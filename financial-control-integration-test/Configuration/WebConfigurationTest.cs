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
using Moq;

namespace financial_control_integration_test.Configuration;

public abstract class ApiFactory<TProgram> : WebApplicationFactory<IInitialProject> 
{
    private readonly RabbitMqContainer _rabbitMqContainer =
        new RabbitMqBuilder()
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
            .Build();

    private const string Queue = "queue-test";
    public ConsumerService ConsumerService { get; set; }
    private Mock<ILogger<ConsumerService>> loggerConsumer { get; set; } = new  Mock<ILogger<ConsumerService>>();
    public async Task InitializeAsync()
    {
        await _rabbitMqContainer.StartAsync();
        ConsumerService = new ConsumerService(loggerConsumer.Object, new ConfigurationConnection(
            _rabbitMqContainer.GetConnectionString(),
            Queue
            ), new Mock<IServiceProvider>().Object);
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices(services => {
            services.RemoveAll(typeof(DbContextOptions<DbFinancialContext>));

            services.AddDbContext<DbFinancialContext>(options => {
                options.UseInMemoryDatabase("FinancialDb", root);
            });

            services.RemoveAll(typeof(ConfigurationConnection));
            services.AddSingleton<ConfigurationConnection>(new ConfigurationConnection(
                _rabbitMqContainer.GetConnectionString(),
                "queue-test"
                ));
            
            services.RemoveAll(typeof(PublisherService));
            services.AddSingleton(
                new PublisherService(
                    new ConfigurationConnection(_rabbitMqContainer.GetConnectionString(), "queue-test"),
                    new Mock<ILogger<PublisherService>>().Object)
                );
        });

        builder.UseEnvironment("Testing");
    }
}
