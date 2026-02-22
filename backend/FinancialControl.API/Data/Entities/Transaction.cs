using System.Text.Json.Serialization;
using FinancialControl.API.Domain.Enums;

namespace FinancialControl.API.Data.Entities;

public abstract class TransactionBase
{
    public Guid PersonId { get; set; }
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
    [JsonIgnore]
    public Guid Id { get; set; }
    public Person Person { get; set; } = new(); 
    public DateTime CreatedDate { get; set; } = new();


    public static implicit operator Transaction(TransactionRequest request)
    {
        return new()
        {
            Date = request.Date,
            Description = request.Description,
            Value = request.Value,
            CreatedDate = DateTime.UtcNow,
            TransactionType = request.TransactionType,
            PersonId = request.PersonId,
            IsRecurring = request.IsRecurring,
            ExpirationDate = request.ExpirationDate
        };
    }
}
