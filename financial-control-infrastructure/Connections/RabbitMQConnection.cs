using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace financial_control_infrastructure.Connections;
public class RabbitMQConnection : IDisposable
{
    private readonly IConnection _connection;
    public IChannel? _channel { get; private set; }

    public RabbitMQConnection(IConfiguration configuration)
    {
        var factory = new ConnectionFactory() 
        { 
            HostName = configuration["RABBITMQ:HOSTNAME"] ?? throw new ArgumentException("Invalid"),
            UserName = configuration["RABBITMQ:USERNAME"] ?? throw new ArgumentException("Invalid"),
            Password = configuration["RABBITMQ:PASSWORD"] ?? throw new ArgumentException("Invalid"),
            Port = int.Parse(configuration["RABBITMQ:PORT"] ?? throw new ArgumentException("Invalid"))
        };

        _connection = factory.CreateConnectionAsync().Result;
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public async Task<IChannel> GetChannelAsync()
    {
        _channel ??= await _connection.CreateChannelAsync();
        return _channel;
    }
}