using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using financial_control_domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using financial_control_domain.Models;

namespace financial_control_infrastructure.Message;

public class ConsumerService(
    ILogger<ConsumerService> logger, 
    ConfigurationConnection configurationConnection,
    IPersonService personService
) : BackgroundService
{

    private readonly IConnectionFactory _connectionFactory =
        new ConnectionFactory { Uri = new Uri(configurationConnection.ConnectionString) };
    private readonly ConfigurationConnection _configurationConnection = configurationConnection;
    private readonly ILogger<ConsumerService> _logger = logger;

    public async Task QueueBind(IChannel channel)
    {
        await channel.ExchangeDeclareAsync(
            exchange: _configurationConnection.exchangeName,
            type: ExchangeType.Fanout,
            durable: true
        );

        await channel.QueueDeclareAsync(
            queue: _configurationConnection.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );
        await channel.QueueBindAsync(
            queue: _configurationConnection.QueueName,
            exchange: _configurationConnection.exchangeName,
            routingKey: string.Empty
        );
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"ConnectionString RabbitMQ: ");
        try
        {
            var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken: stoppingToken);

            ArgumentNullException.ThrowIfNull(connection);
            var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await QueueBind(channel);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (connection?.IsOpen == false || !channel.IsOpen)
                    {
                        connection = await _connectionFactory.CreateConnectionAsync(cancellationToken: stoppingToken);
                        channel = await connection?.CreateChannelAsync(cancellationToken: stoppingToken)!;

                        await QueueBind(channel);
                        await Consumer(TimeSpan.FromSeconds(5));
                    }

                    var consumerCount = await channel.ConsumerCountAsync(
                        _configurationConnection.QueueName,
                        stoppingToken
                    );

                    if (consumerCount == 0)
                    {
                        await Consumer(TimeSpan.FromSeconds(5));
                    }
                    await Task.Delay(5000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while consuming message from RabbitMQ");
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Error: {E}", e.Message);
        }
    }
    
    public async Task<bool> Consumer(TimeSpan timeout)
    {

        var messageReceived = new TaskCompletionSource<bool>();

        _logger.LogInformation(
            "Start consumer message exchange: {E}, Queue: {Q}, Host: {H}",
            _configurationConnection.exchangeName,
            _configurationConnection.QueueName,
            _connectionFactory.Uri + _connectionFactory.VirtualHost
        );
        var connection = await _connectionFactory.CreateConnectionAsync();
        ArgumentNullException.ThrowIfNull(connection);
        var channel = await connection.CreateChannelAsync();

        _logger.LogInformation("Bind done");
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation($"Message received: {message}");
                return personService.CreateAsync(JsonSerializer.Deserialize<PersonModel>(message) ?? throw new JsonException(""));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.CompletedTask;
            }
            finally
            {
                messageReceived.SetResult(true);
            }
        };
        var response = await channel.BasicConsumeAsync
        (
            _configurationConnection.QueueName,
            autoAck: true,
            consumer: consumer
        );
        _logger.LogInformation("Created basic consumer");


        return await messageReceived.Task;
    }
}