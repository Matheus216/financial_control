using financial_control_domain.Interfaces.Repositories;
using financial_control_Infrastructure.Repositories;
using financial_control.Infrastructure.Context;
using financial_control_domain.Models;
using Microsoft.EntityFrameworkCore;

namespace financial_control_infrastructure.Repositories;

public class PersonRepository(IDbContextFactory<DbFinancialContext> factory) 
    : RepositoryBase<PersonModel>(factory), IPersonRepository;
