using System.ComponentModel.DataAnnotations;

namespace financial_control.Domain.Entities; 
public class Financial(long id, decimal value, DateTime date, short Type, long PersonId, Person Person)
{
    public Financial(decimal value, DateTime date, short type, long personId, Person person) 
        : this(0, value, date, type, personId, person)
    {
        Id = 0;
        Value = value;
        Date = date;
        Type = type;
        PersonId = personId;
        Person = person;
    }

    public long Id { get; private set; }
    public decimal Value { get; private set; }
    public DateTime Date { get; private set; }
    public short Type { get; private set; }
    public long PersonId { get; private set; }
    public Person Person { get; private set; }
}
