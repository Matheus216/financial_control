using financial_control.Infrastructure.Context;
using financial_control.Infrastructure.Models;

namespace financial_control.Infrastructure.Repository;

public class PersonRepository : RepositoryBase<PersonModel>
{
    public PersonRepository(DbFinancialContext context) : base(context)
    {
    }
}