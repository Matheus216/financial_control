using financial_control_domain.Interfaces.Services;
using financial_control_infrastructure.Message;
using financial_control_domain.Models;
using financial_control.Services;
using Microsoft.AspNetCore.Mvc;

namespace financial_control.Controller;

[ApiController]
[Route("api/[controller]")]
public class PersonController(IPublisherService publisherService) : ControllerBase
{
    private readonly IPublisherService _publisherService = publisherService;

    [HttpPost]
    public async Task<ActionResult<PersonModel>> Create([FromBody] PersonModel person)
    {
        await _publisherService.PublishMessage(person);
        return Created(nameof(Create), person); 
    }
}