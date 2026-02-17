
using System.Net;
using System.Net.Http.Json;
using FinancialControl.API.Data.Entities;
using FinancialControl.Tests.ApiConfiguration;

namespace FinancialControl.Tests.Fixtures;

public abstract class AssetFixture : TestBase
{
    [Test]
    public async Task WhenWeCallCreateAsset_ShouldCreateNewRecordInTheTable(CancellationToken cancellationToken)
    {
        //Arrange
        var client = Factory.CreateClient();
        var asset = new Asset
        {
            Id = new Guid(),
            Description = "new"  
        };

        //Act
        var response = await client.PostAsJsonAsync("/api/assets", asset, cancellationToken);

        //Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Created);
    }
}
