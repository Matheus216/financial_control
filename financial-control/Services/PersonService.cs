using financial_control_Infrastructure.Repositories;
using financial_control_domain.Interfaces.Services;
using financial_control_domain.Models;

namespace financial_control.Services;

public class PersonService(PersonRepository personRepository) : IPersonService
{
    public async Task<IEnumerable<PersonModel>> GetAll()
    {
        return await personRepository.GetAll();
    }  

    public async Task<PersonModel> Create(PersonModel person)
    {
        await personRepository.Insert(person);
        return person;
    }
}