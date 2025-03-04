using financial_control_infrastructure.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;

namespace financial_control_infrastructure.Message;
public class PublisherService : IPublisherService 
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PublisherService> _logger;
    private readonly RabbitMQConnection _connection;

    public PublisherService(IConfiguration configuration, 
        ILogger<PublisherService> logger,
        RabbitMQConnection connection 
    )
    {
        _configuration = configuration;
        _logger = logger;
        _connection = connection;
    }

    public async Task PublishMessage(object request)
    {
        _logger.LogInformation("Publishing message to RabbitMQ");

        ArgumentNullException.ThrowIfNull(request);

        using var channel = await _connection.GetChannelAsync();

        await channel.ExchangeDeclareAsync
        (
            exchange: _configuration["RABBITMQ:EXCHANGE"]  ?? throw new ArgumentException("Invalid"), 
            type: ExchangeType.Fanout
        );

        await channel.QueueDeclareAsync
        (
            queue: _configuration["RABBITMQ:QUEUE"]?? throw new ArgumentException("Invalid"),
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));

        await channel.BasicPublishAsync
        (
            _configuration["RABBITMQ:EXCHANGE"] ?? throw new ArgumentException("Invalid"),
            _configuration["RabbitMQ:QUEUE"]?? throw new ArgumentException("Invalid"),
            body
        );
        
        _logger.LogInformation("Message published to RabbitMQ");
    }

    public async Task ConsumerMessage()
    {
        _logger.LogInformation("Consuming message from RabbitMQ");

        using var channel = await _connection.GetChannelAsync();

        await channel.ExchangeDeclareAsync
        (
            exchange: _configuration["RABBITMQ:EXCHANGE"] ?? throw new ArgumentException("invalid"), 
            type: ExchangeType.Fanout
        );

        await channel.QueueDeclareAsync
        (
            queue: _configuration["RABBITMQ:QUEUE"] ?? throw new ArgumentException("invalid"),
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation($"Message received: {message}");

            return Task.CompletedTask;
        };   

        await channel.BasicConsumeAsync
        (
            queue: _configuration["RABBITMQ:QUEUE"] ?? throw new ArgumentException(),
            autoAck: true,
            consumer: consumer
        );
    }
}

public interface IPublisherService
{
    Task PublishMessage(object request);
}