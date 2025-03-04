using financial_control_domain.Interfaces.Services;
using financial_control_infrastructure.Message;
using financial_control_domain.Models;
using financial_control.Services;
using Microsoft.AspNetCore.Mvc;

namespace financial_control.Controller;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly PersonService _personService;
    private readonly IPublisherService _publisherService;

    public PersonController(PersonService personService, IPublisherService publisherService)
    {
        _personService = personService;
        _publisherService = publisherService;
    }

    [HttpGet]
    public async Task<IEnumerable<PersonModel>> GetAll()
    {
        return await _personService.GetAll();
    }

    [HttpPost]
    public async Task<ActionResult<PersonModel>> Create([FromBody] PersonModel person)
    {
        await _publisherService.PublishMessage(person);
        return Created(nameof(Create), person); 
    }
}