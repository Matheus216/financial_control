namespace financial_control.Domain.Entities;

public class Person
{
    public Person(long Id, string Name)
    {
        this.Id = Id;
        this.Name = Name;
    }
    public Person(string name) 
    {
        Name = name;   
    }

    public long Id { get; private set; }
    public string Name { get; private set; }
}
