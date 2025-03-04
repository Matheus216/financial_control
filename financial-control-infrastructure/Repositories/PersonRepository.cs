using financial_control.Infrastructure.Context;
using financial_control_domain.Models;

namespace financial_control_Infrastructure.Repositories;

public class PersonRepository : RepositoryBase<PersonModel>
{
    public PersonRepository(DbFinancialContext context) : base(context)
    {
    }
}