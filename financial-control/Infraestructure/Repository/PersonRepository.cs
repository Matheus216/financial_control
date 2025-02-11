using financial_control.Infraestructure.Context;
using financial_control.Infraestructure.Models;

namespace financial_control.Infraestructure.Repository;

public class PersonRepository : RepositoryBase<PersonModel>
{
    public PersonRepository(DbFinancialContext context) : base(context)
    {
    }
}