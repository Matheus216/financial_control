using financial_control_domain.Interfaces.Services;
using financial_control_infrastructure.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;

namespace financial_control_infrastructure.Message;
public class PublisherService : IPublisherService 
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PublisherService> _logger;

    public PublisherService(IConfiguration configuration, 
        ILogger<PublisherService> logger
    )
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task PublishMessage(object request)
    {
        try
        {
            _logger.LogInformation("Publishing message to RabbitMQ");

            using var connection =  RabbitMQConnection.GetConnection(_configuration);

            ArgumentNullException.ThrowIfNull(request);

            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync
            (
                exchange: _configuration["RABBITMQ:EXCHANGE"] ?? throw new ArgumentException("Invalid"),
                type: ExchangeType.Fanout
            );

            await channel.QueueDeclareAsync
            (
                queue: _configuration["RABBITMQ:QUEUE"] ?? throw new ArgumentException("Invalid"),
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));

            await channel.BasicPublishAsync
            (
                exchange: _configuration["RABBITMQ:EXCHANGE"] ?? throw new ArgumentException("Invalid"),
                routingKey: string.Empty,
                body: body
            );

            _logger.LogInformation("Message published to RabbitMQ");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to RabbitMQ");
        }
    }
}