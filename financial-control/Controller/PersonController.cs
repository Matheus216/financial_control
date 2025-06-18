using financial_control_domain.Interfaces.Services;
using financial_control_domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace financial_control.Controller;

[ApiController]
[Route("api/[controller]")]
public class PersonController(
    IPublisherService publisherService,
    IPersonService service
) : ControllerBase
{
    private readonly IPublisherService _publisherService = publisherService;

    [HttpPost]
    public async Task<ActionResult<PersonModel>> Create([FromBody] PersonModel person)
    {
        await _publisherService.PublishMessage(person);
        return Created(nameof(Create), person);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonModel>>> GetAllAsync( 
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default
    )
    {
        var response = await service.GetAllAsync(page, pageSize, cancellationToken);
        return Ok(response);        
    }
}