using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;
public class Person
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty; 
    public string CNPJ { get; set; } = string.Empty;
    public DateTime BornDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public IEnumerable<Wallet> Wallets { get; set; } = [];
}
