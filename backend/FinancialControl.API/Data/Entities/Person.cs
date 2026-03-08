using System.Text.Json.Serialization;

namespace FinancialControl.API.Data.Entities;

public abstract class PersonBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; 
    public string Username { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public DateTime BornDate { get; set; }
    public string CPF { get; set; } = string.Empty; 
}

public class Person : PersonBase
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public IEnumerable<Wallet> Wallets { get; set; } = [];
    public ICollection<Transaction> Transactions { get; set;} = [];

    public void SetId(Guid id)
    {
        Id = id;
    }

    public static explicit operator Person(PersonCreateRequest request)
    {
        return new()
        {
            Id = new Guid(),
            CPF = request.CPF,
            CreatedDate = DateTime.Now,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BornDate = request.BornDate,
            Email = request.Email
        };
    }

    public static explicit operator Person(PersonUpdateRequest request)
    {
        return new()
        {
            CPF = request.CPF,
            UpdatedDate = DateTime.Now,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BornDate = request.BornDate,
            Email = request.Email
        };
    }
}


public class PersonCreateRequest : PersonBase
{
    public string Password { get; set; } = string.Empty; 
}

public class PersonUpdateRequest : PersonBase
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty; 
}