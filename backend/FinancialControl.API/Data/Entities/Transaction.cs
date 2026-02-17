using System.Text.Json.Serialization;
using FinancialControl.API.Domain.Enums;

namespace FinancialControl.API.Data.Entities;

public abstract class TransactionBase
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsRecurring { get; set; }
    public TransactionType TransactionType { get; set; }
}


public class TransactionRequest : TransactionBase
{
}

public class Transaction : TransactionBase
{
    public Person Person { get; set; } = new(); 
    public Guid PersonId { get; set; }
}
