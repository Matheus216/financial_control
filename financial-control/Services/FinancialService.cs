using financial_control.Domain.Entities;

namespace financial_control.Services;
public class FinancialService
{
    public void Test() 
    {
        var financial = new Financial(1, 100, DateTime.Now, 1, 1, new Person(1, "Person 1"));
    }
}