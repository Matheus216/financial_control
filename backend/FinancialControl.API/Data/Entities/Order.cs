using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;
public enum OrderType
{
    Sell, 
    Buy
}

public class Order
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public Guid PersonWalletId { get; set; }
    public PersonWallet PersonWallet { get; set; } = new();
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public OrderType Type { get; set; }
}
