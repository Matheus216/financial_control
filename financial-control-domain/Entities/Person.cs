using System.Data.Common;

namespace financial_control_Domain.Entities;

public class Person(long id, string name)
{
    public Person(string name)
        : this(0, name)
    {
        Name = name;
    }

    public long Id { get; private set; } = id;
    public string Name { get; private set; } = name;

    public static void CalculeAge(Person request)
    {
        var result = request switch
        {
            Person { Id: long identifier } when identifier > 21 => 12,
            _ =>  throw new ArgumentException("")
        };

    }    
}
