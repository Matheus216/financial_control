using System;

namespace FinancialControl.API.ViewModel;

public class ResponseAPI<T>
{
    public List<T> Results { get; set; } = [];
}
