using financial_control_domain.Interfaces.Repositories;
using financial_control_domain.Interfaces.Services;
using financial_control_domain.Models;
using Microsoft.Extensions.Logging;

namespace financial_control_application.Services;

public class PersonService(
    IPersonRepository repository,
    ILogger<PersonService> logger
) : IPersonService
{
    public async Task<PersonModel> CreateAsync(
        PersonModel person,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await repository.InsertAsync(person, cancellationToken);
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Error: {Message}", ex.Message);
        }
        return person;
    }

    public async Task<IEnumerable<PersonModel>> GetAllAsync(
        int page = 1,
        int itemsPeerPage = 10,
        CancellationToken cancellationToken = default
    )
    {
        var response = await repository.GetAllAsync(page, itemsPeerPage, cancellationToken);
        return response.Select(x => new PersonModel(x.Id, x.Name));
    }
} 
