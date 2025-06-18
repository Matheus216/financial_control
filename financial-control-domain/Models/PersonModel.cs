namespace financial_control_domain.Models;

public class PersonModel
{
    public PersonModel(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public PersonModel(string name)
    {
        Id = 0;
        Name = name;
    }

    public PersonModel()
    {
        this.Id = 0;
        this.Name = string.Empty;
    }

    public long Id { get; set; }
    public string Name { get; set; }
}