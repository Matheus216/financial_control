using financial_control_domain.Models;

namespace financial_control_domain.Interfaces.Services;

public interface IPersonService
{
    Task<PersonModel> Create(PersonModel person);
}