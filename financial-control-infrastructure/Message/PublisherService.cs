using financial_control_domain.Interfaces.Services;
using financial_control_infrastructure.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using RabbitMQ.Client;
using System.Text;

namespace financial_control_infrastructure.Message;
public class PublisherService(
    ConfigurationConnection configuration, 
    ILogger<PublisherService> logger
) : IPublisherService 
{
    public async Task PublishMessage(object request)
    {
        try
        {
            logger.LogInformation("Publishing message to RabbitMQ");

            var connection = await new ConnectionFactory
            {
                Uri = new Uri(configuration.ConnectionString)
            }.CreateConnectionAsync();

            ArgumentNullException.ThrowIfNull(request);

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync
            (
                queue: configuration.QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));

            await channel.BasicPublishAsync
            (
                exchange: string.Empty,
                routingKey: configuration.QueueName!,
                body: body
            );

            logger.LogInformation("Message published to RabbitMQ");
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Error publishing message to RabbitMQ");
        }
    }
}