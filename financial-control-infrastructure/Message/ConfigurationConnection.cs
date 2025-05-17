namespace financial_control_infrastructure.Message;

public class ConfigurationConnection(string connectionString, string queueName)
{
    public string ConnectionString { get; private set; } = connectionString;
    public string QueueName { get; private set; } = queueName;
}