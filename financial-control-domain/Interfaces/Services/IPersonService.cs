using financial_control_domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace financial_control_domain.Interfaces.Services;
public interface IPersonService
{
    Task<PersonModel> Create(PersonModel person);
}