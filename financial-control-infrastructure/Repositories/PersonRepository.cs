using financial_control_domain.Interfaces.Repositories;
using financial_control.Infrastructure.Context;
using financial_control_Domain.Entities;

namespace financial_control_Infrastructure.Repositories;

public class PersonRepository : RepositoryBase<Person>, IPersonRepository
{
    public PersonRepository(DbFinancialContext context)
        : base(context)
    {
    }
}