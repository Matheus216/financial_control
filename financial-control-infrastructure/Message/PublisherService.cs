using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;

namespace financial_control_infrastructure.Message;
public class PublisherService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PublisherService> _logger;

    public PublisherService(IConfiguration configuration, ILogger<PublisherService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task PublishMessage(object request)
    {
        _logger.LogInformation("Publishing message to RabbitMQ");

        ArgumentNullException.ThrowIfNull(request);

        var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:Connection"] ?? "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync
        (
            exchange: _configuration["RabbitMQ:ExchangeName"] ?? "financial-control-exchange", 
            type: ExchangeType.Fanout
        );

        await channel.QueueDeclareAsync
        (
            queue: _configuration["RabbitMQ:QueueName"] ?? "financial-control-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));

        await channel.BasicPublishAsync
        (
            _configuration["RabbitMQ:ExchangeName"] ?? "financial-control-exchange",
            _configuration["RabbitMQ:QueueName"] ?? "financial-control-queue",
            body
        );
        

        _logger.LogInformation("Message published to RabbitMQ");
    }
}