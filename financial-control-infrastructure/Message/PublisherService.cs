using financial_control_domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;

namespace financial_control_infrastructure.Message;
public class PublisherService(
    ConfigurationConnection configuration, 
    ILogger<PublisherService> logger,
    IConnectionFactory connection
) : IPublisherService 
{
    private readonly ILogger<PublisherService> _logger = logger;
    private readonly ConfigurationConnection _configurationConnection = configuration;
    private readonly IConnectionFactory _connectionFactory = connection; 

    public async Task PublishMessage(object request)
    {
        try
        {
            _logger.LogInformation("Publishing message to RabbitMQ");

            var connection = await _connectionFactory.CreateConnectionAsync();

            ArgumentNullException.ThrowIfNull(request);

            using var channel = await connection.CreateChannelAsync();

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));

            await channel.BasicPublishAsync
            (
                exchange: _configurationConnection.exchangeName,
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