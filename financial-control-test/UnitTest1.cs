using financial_control_Domain.Entities;

namespace financial_control_test;

public class UnitTest1
{
    [Fact]
    public void InstanceWhenHaveIdShouldBeTrue()
    {
        var financial = new Financial(1, 100, DateTime.Now, 1, 1, new Person(1, "Person 1"));

        Assert.Equal(1, financial.Id);
    }
}