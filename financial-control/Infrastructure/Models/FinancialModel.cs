namespace financial_control.Infrastructure.Models;

public class FinancialModel
{
    public long Id { get; set; }
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
    public short Type { get; set; }
    public long PersonId { get; set; }
    public string Description { get; set; } = string.Empty;
    public PersonModel Person { get; set; } = new PersonModel();
}