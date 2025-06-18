
namespace financial_control_Domain.Entities;

public record Person(long Id, string Name) 
{
    public Person(string name)
        : this(0, name)
    {
        Name = name;
    }
    public static void CalculeAge(Person request)
    {
        var result = request switch
        {
            Person { Id: long identifier } when identifier > 21 => 12,
            _ =>  throw new ArgumentException("")
        };
    }    
}
