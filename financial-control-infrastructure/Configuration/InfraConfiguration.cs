using financial_control.Infrastructure.Context;
using financial_control_infrastructure.Message;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace financial_control_infrastructure.Configuration;

public static class InfraConfiguration
{
    public static IServiceCollection ConfigureDB(
        this IServiceCollection services
    )
    {
        services.AddDbContextFactory<DbFinancialContext>();
        return services;
    }
    public static IServiceCollection ConfigureMessaging(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionStringRabbit = configuration["RABBITMQ:ConnectionString"] ?? throw new ArgumentException("RABBITMQ:ConnectionString");
        var queue = configuration["RABBITMQ:QUEUE"] ?? throw new ArgumentException("RABBITMQ:QUEUE");
        var exchange = configuration["RABBITMQ:EXCHANGE"] ?? throw new ArgumentException("RABBITMQ:EXCHANGE");

        services.AddSingleton(new ConfigurationConnection(
            connectionStringRabbit,
            queue,
            exchange
        ));

        ConnectionFactory connectionFactory = new()
        {
            Uri = new Uri(connectionStringRabbit)
        };

        services.AddSingleton<IConnectionFactory>(connectionFactory);
        
        return services;
    }
}
