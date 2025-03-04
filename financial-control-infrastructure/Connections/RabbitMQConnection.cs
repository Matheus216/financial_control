using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace financial_control_infrastructure.Connections;
public static class RabbitMQConnection 
{
    public static IConnection GetConnection(IConfiguration _configuration)
    {
        var factory = new ConnectionFactory() 
        { 
            HostName = _configuration["RABBITMQ:HOSTNAME"] ?? throw new ArgumentException("Invalid"),
            UserName = _configuration["RABBITMQ:USERNAME"] ?? throw new ArgumentException("Invalid"),
            Password = _configuration["RABBITMQ:PASSWORD"] ?? throw new ArgumentException("Invalid"),
            Port = int.Parse(_configuration["RABBITMQ:PORT"] ?? throw new ArgumentException("Invalid"))
        };

        return factory.CreateConnectionAsync().Result;
    }
}