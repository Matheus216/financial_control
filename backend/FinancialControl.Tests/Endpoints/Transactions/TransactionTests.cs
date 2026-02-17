using System.Net;
using System.Net.Http.Json;
using FinancialControl.API.Data;
using FinancialControl.API.Data.Entities;
using FinancialControl.API.Domain.Enums;
using FinancialControl.Tests.ApiConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialControl.Tests.Endpoints.Transactions;

public class TransactionTests : TestBase
{
    [Test]
    public async Task WhenSendCorrectTransaction_ShouldBeCreatedCorretly(CancellationToken cancellationToken)
    {
        //Arrange
        var client = Factory.CreateClient(); 
        var transaction = new Transaction
        {
            Description = "First transaction",
            Date = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(12),
            Value = 123.33M,
            IsRecurring = true,
            TransactionType = TransactionType.Out
        };

        //ACT
        var response = await client.PostAsJsonAsync("/api/transactions", transaction, cancellationToken);

        //Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Created);
    }
}
