using System.Net;
using System.Net.Http.Json;
using financial_control_infrastructure.Message;
using financial_control_integration_test.Configuration;
using financial_control.Configuration;

namespace financial_control_integration_test.Person;

public class PersonServiceTest(ApiFactory<IInitialProject> webConfiguration)
    : IClassFixture<ApiFactory<IInitialProject>>
{
    private readonly HttpClient _client = webConfiguration.CreateClient();
    private readonly ConsumerService _consumerService = webConfiguration.ConsumerService;

    [Fact]
    public async Task ShouldBeSuccessWhenPassCorrectlyData() {
        //Arrange
        var person = new { Name = "francisco" };
        
        //Act
        var response = await _client.PostAsJsonAsync("/api/person", person);

        //Arrange
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
