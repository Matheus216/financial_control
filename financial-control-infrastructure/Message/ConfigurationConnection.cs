namespace financial_control_infrastructure.Message;

public class ConfigurationConnection(string connectionString, string queueName, string exchangeName)
{
    public string ConnectionString { get; private set; } = connectionString;
    public string QueueName { get; private set; } = queueName;
    public string exchangeName { get; set; } = exchangeName;
}