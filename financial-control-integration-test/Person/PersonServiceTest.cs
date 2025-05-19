using System.Net;
using System.Net.Http.Json;
using financial_control_infrastructure.Message;
using financial_control_integration_test.Configuration;
using financial_control.Configuration;

namespace financial_control_integration_test.Person;

public class PersonServiceTest(ApiFactory<IInitialProject> factory)
    : IClassFixture<ApiFactory<IInitialProject>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly ConsumerService _consumerService = factory.ConsumerService;

    [Fact]
    public async Task ShouldBeSuccessWhenPassCorrectlyData() {
        //Arrange
        var person = new { Name = "francisco" };

        await _consumerService.QueueBind(factory.Exchange, factory.Queue);

        //Act
        var response = await _consumerService.Consumer();

        //Arrange
        Assert.True(response);
    }
}
