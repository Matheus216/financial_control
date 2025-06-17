using financial_control_integration_test.Configuration;
using financial_control_infrastructure.Message;
using System.Net.Http.Json;

namespace financial_control_integration_test.Person;

public class PersonServiceTest(ApiFactory factory)
        : IClassFixture<ApiFactory>
{

    [Fact]
    public async Task ShouldBeSuccessWhenPassCorrectlyData()
    {
        //Arrange

        var client = factory.CreateClient();
        var person = new { Name = "francisco" };

        var channel = await factory.GetChannelAsync();

        await factory.ConsumerService.QueueBind(channel);

        //Act
        var httpResponse = await client.PostAsJsonAsync("/api/person", person);

        var response = await factory.ConsumerService.Consumer(TimeSpan.FromSeconds(10));

        //Arrange
        Assert.Equal(System.Net.HttpStatusCode.Created, httpResponse.StatusCode);
        Assert.True(response);
    }
}
