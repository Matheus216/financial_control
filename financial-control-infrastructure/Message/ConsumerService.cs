using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace financial_control_infrastructure.Message;

public class ConsumerService(
    ILogger<ConsumerService> logger, 
    ConfigurationConnection configurationConnection) : BackgroundService
{

    private readonly IConnectionFactory _connectionFactory =
        new ConnectionFactory { Uri = new Uri(configurationConnection.ConnectionString) };
    private readonly ConfigurationConnection _configurationConnection = configurationConnection;
    private readonly ILogger<ConsumerService> _logger = logger;

    public async Task QueueBind(string exchange, string queueName, string routingKey = "")
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Fanout, durable: true);
        var queueResult = await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );
        await channel.QueueBindAsync(
            queue: queueName,
            exchange: exchange,
            routingKey: routingKey
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

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (connection?.IsOpen == false || !channel.IsOpen)
                    {
                        connection = await _connectionFactory.CreateConnectionAsync(cancellationToken: stoppingToken);
                        channel = await connection?.CreateChannelAsync(cancellationToken: stoppingToken)!;

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

        _logger.LogInformation("Start consumer message");
        var connection = await _connectionFactory.CreateConnectionAsync();
        ArgumentNullException.ThrowIfNull(connection);
        var channel = await connection.CreateChannelAsync();

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            messageReceived.SetResult(true);
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // using var scope = serviceProvider.CreateAsyncScope();
                // var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();
                _logger.LogInformation($"Message received: {message}");
                // return personService.Create(JsonSerializer.Deserialize<PersonModel>(message) ?? throw new ArgumentException());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return Task.CompletedTask;
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