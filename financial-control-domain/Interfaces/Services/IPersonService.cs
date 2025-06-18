using financial_control_domain.Models;

namespace financial_control_domain.Interfaces.Services;

public interface IPersonService
{
    Task<PersonModel> CreateAsync(PersonModel person, CancellationToken cancellationToken = default);
    Task<IEnumerable<PersonModel>> GetAllAsync(
        int page = 1,
        int itemsPeerPage = 10,
        CancellationToken cancellationToken = default
    );
}