namespace financial_control.Domain.Entities;

public class Person(long Id, string Name)
{
    public Person(string name) : this(0, name)
    {
        Name = name;   
    }

    public long Id { get; private set; }
    public string Name { get; private set; }
}
