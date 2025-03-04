using financial_control.Infrastructure.Repository;
using financial_control.Infrastructure.Models;

namespace financial_control.Services;

public class PersonService
{
    private readonly PersonRepository _personRepository;

    public PersonService(PersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    
    public async Task<IEnumerable<PersonModel>> GetAll()
    {
        return await _personRepository.GetAll();
    }  

    public async Task<PersonModel> Create(PersonModel person)
    {
        await _personRepository.Insert(person);
        return person;
    }
}