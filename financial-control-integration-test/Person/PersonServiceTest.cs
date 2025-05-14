
using System.Net;
using System.Net.Http.Json;
using financial_control_integration_test.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace financial_control_integration_test.Person;

public class PersonServiceTest 
    : IClassFixture<WebConfigurationTest<Program>>
{
    private readonly WebConfigurationTest<Program> _webConfiguration; 

    public PersonServiceTest(WebConfigurationTest<Program> webConfiguration)
    {
        _webConfiguration = webConfiguration;    
    }

    [Fact]
    public async Task ShouldBeSuccessWhenPassCorrectlyData() {
        //Arrange
        var url = "/api/person";
        var person = new { Name = "francisco" };

        //Act
        var client = _webConfiguration.CreateClient();
        var response = await client.PostAsJsonAsync(url, person);

        //Arrange
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
