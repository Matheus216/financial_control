using financial_control_domain.Interfaces.Repositories;
using financial_control_Infrastructure.Repositories;
using financial_control.Infrastructure.Context;
using financial_control_Domain.Entities;

namespace financial_control_infrastructure.Repositories;

public class PersonRepository(DbFinancialContext context) 
    : RepositoryBase<Person>(context), IPersonRepository;
