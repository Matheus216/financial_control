using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace financial_control_infrastructure.Connections;
public class RabbitMQConnection : IDisposable
{
    private readonly IConnection _connection;

    public RabbitMQConnection(IConfiguration configuration)
    {
        var factory = new ConnectionFactory() 
        { 
            HostName = configuration["RabbitMQ:Hostname"] ?? "localhost",
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672")
        };

        _connection = factory.CreateConnectionAsync().Result;
    }

    public void Dispose()
    {
        _connection.Dispose();    
    }
}