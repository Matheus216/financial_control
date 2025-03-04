using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace financial_control_domain.Interfaces.Services;

public interface IPublisherService
{
    Task PublishMessage(object request);
}