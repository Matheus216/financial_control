using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using financial_control_infrastructure.Connections;
using Microsoft.Extensions.Configuration;
using financial_control_domain.Interfaces.Services;
using System.Text.Json;
using financial_control_domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace financial_control_infrastructure.Message;

public class ConsumerService : BackgroundService
{
    private ILogger<ConsumerService> _logger;
    private IConnection _connection;
    private IChannel _channel;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public ConsumerService(ILogger<ConsumerService> logger,
        IConfiguration configuration,
        IServiceProvider serviceProvider
    )
    {
        _logger = logger;
        _connection = RabbitMQConnection.GetConnection(configuration);   
        _channel = _connection.CreateChannelAsync().Result;
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        _logger.LogInformation("Consuming message from RabbitMQ");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_connection == null || !_connection.IsOpen || _channel == null || !_channel.IsOpen)
                {
                    _connection = RabbitMQConnection.GetConnection(_configuration);
                    _channel = await _connection.CreateChannelAsync();

                    await Consumer();
                }

                var consumerCount = await _channel.ConsumerCountAsync(_configuration["RABBITMQ:QUEUE"] ?? throw new ArgumentException("invalid"));
                if (consumerCount == 0)
                {
                    await Consumer();
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

    public async Task Consumer()
    {
        _logger.LogInformation("Start consumer message");
       
        await _channel.QueueDeclareAsync
        (
            queue: _configuration["RABBITMQ:QUEUE"] ?? throw new ArgumentException("invalid"),
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                using var scope = _serviceProvider.CreateAsyncScope();
                var personService = scope.ServiceProvider.GetRequiredService<IPersonService>();

                _logger.LogInformation($"Message received: {message}");
                return personService.Create(JsonSerializer.Deserialize<PersonModel>(message) ?? throw new ArgumentException());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                 // TODO
                return Task.CompletedTask;

            }
        };

        await _channel.BasicConsumeAsync
        (
            _configuration["RABBITMQ:QUEUE"] ?? throw new ArgumentException(),
            autoAck: true,
            consumer: consumer
        );
        _logger.LogInformation("Created basic consumer");
    }
}