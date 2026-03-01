using FinancialControl.API.Contracts.User.Requests;
using FinancialControl.API.Contracts.User.Responses;
using FinancialControl.API.Data.Entities;

namespace FinancialControl.API.Application.Interfaces;

public interface IKeycloakService
{
    Task<Guid> CreateUserAsync(PersonCreateRequest request);
    Task<UserResponse?> GetUserByIdAsync(string id);
    Task<IEnumerable<UserResponse>> ListUsersAsync();
    Task UpdateUserAsync(Guid id, PersonUpdateRequest request);
    Task DeleteUserAsync(string id);
}
