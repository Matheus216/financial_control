using financial_control.Infraestructure.Models;
using financial_control.Services;
using Microsoft.AspNetCore.Mvc;

namespace financial_control.Controller;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly PersonService _personService;

    public PersonController(PersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public async Task<IEnumerable<PersonModel>> GetAll()
    {
        return await _personService.GetAll();
    }

    [HttpPost]
    public async Task<PersonModel> Create([FromBody] PersonModel person)
    {
        return await _personService.Create(person);
    }
}