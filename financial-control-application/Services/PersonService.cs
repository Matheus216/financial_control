
using financial_control_domain.Interfaces.Repositories;
using financial_control_domain.Models;
using financial_control_Domain.Entities;

namespace financial_control_application.Services;

public class PersonService(IPersonRepository repository)
{
    public async Task<PersonModel> CreateAsync(
        PersonModel person,
        CancellationToken cancellationToken = default
    )
    {
        await repository.InsertAsync(new Person(person.Name));
        return person;
    }
} 
