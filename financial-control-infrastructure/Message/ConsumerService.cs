using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace financial_control_infrastructure.Message;

public class ConsumerService(
    ILogger<ConsumerService> logger, 
    ConfigurationConnection configurationConnection,
    IConnectionFactory connectionFactory) : BackgroundService
{
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation($"ConnectionString RabbitMQ: ");
        var connection = await connectionFactory.CreateConnectionAsync(cancellationToken: stoppingToken);
        ArgumentNullException.ThrowIfNull(connection);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (connection?.IsOpen == false || !channel.IsOpen)
                {
                    connection =  await connectionFactory.CreateConnectionAsync(cancellationToken: stoppingToken);
                    channel = await connection?.CreateChannelAsync(cancellationToken: stoppingToken)!;
            
                    await Consumer(configurationConnection.QueueName);
                }

                var consumerCount = await channel.ConsumerCountAsync(
                    configurationConnection.QueueName,
                    stoppingToken
                );
                
                if (consumerCount == 0)
                {
                    await Consumer();
                }
                await Task.Delay(5000, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while consuming message from RabbitMQ");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
    private async Task Consumer()
    {
        logger.LogInformation("Start consumer message");
        var connection = await connectionFactory.CreateConnectionAsync();
        ArgumentNullException.ThrowIfNull(connection);
        var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync
        (
            queue: configurationConnection.QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // using var scope = serviceProvider.CreateAsyncScope();
                // var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();
                logger.LogInformation($"Message received: {message}");
                // return personService.Create(JsonSerializer.Deserialize<PersonModel>(message) ?? throw new ArgumentException());
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return Task.CompletedTask;
            }
        };

        await channel.BasicConsumeAsync
        (
            configurationConnection.QueueName,
            autoAck: true,
            consumer: consumer
        );
        logger.LogInformation("Created basic consumer");
    }
}