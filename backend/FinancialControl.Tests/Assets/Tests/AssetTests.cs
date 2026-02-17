using FinancialControl.Tests.ApiConfiguration;
using FinancialControl.API.Data.Entities;
using System.Net.Http.Json;
using System.Net;

namespace FinancialControl.Tests.Assets;

public class AssetTests : TestBase
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
