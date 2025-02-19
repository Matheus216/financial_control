namespace financial_control.Domain.Entities;

public class Person(long id, string name)
{
    public Person(string name)
        : this(0, name)
    {
        Name = name;   
    }

    public long Id { get; private set; } = id;
    public string Name { get; private set; } = name;
}
