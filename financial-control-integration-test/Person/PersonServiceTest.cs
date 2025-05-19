using System.Net.Http.Json;
using financial_control_infrastructure.Message;
using financial_control_integration_test.Configuration;

namespace financial_control_integration_test.Person;

public class PersonServiceTest(ApiFactory factory)
        : IClassFixture<ApiFactory>
{
    private readonly ConsumerService _consumerService = factory.ConsumerService;
    private readonly ApiFactory _factory = factory;

    [Fact]
    public async Task ShouldBeSuccessWhenPassCorrectlyData()
    {
        //Arrange

        var client = _factory.CreateClient();
        var person = new { Name = "francisco" };

        await _consumerService.QueueBind(_factory.Exchange, _factory.Queue);

        //Act
        var httpResponse = await client.PostAsJsonAsync("/api/person", person);

        var response = await _consumerService.Consumer(TimeSpan.FromSeconds(10));

        //Arrange
        Assert.Equal(System.Net.HttpStatusCode.Created, httpResponse.StatusCode);
        Assert.True(response);
    }
}
