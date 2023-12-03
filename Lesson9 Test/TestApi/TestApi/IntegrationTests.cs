using System.Net.Http;
using System.Threading.Tasks;
using Api.Controllers.Dto.Response;
using Newtonsoft.Json;
using Xunit;

namespace TestApi;

public class IntegrationTests : IClassFixture<BaseWebApplicationFactory>
{
    private readonly BaseWebApplicationFactory _factory;

    public IntegrationTests()
    {
        
    }
    
    [Fact]
    public async Task IntegrationTest()
    {
        var client = new HttpClient();

        var response = await client.GetAsync("http://localhost:5231/api/v1/test");
        
        
        Assert.True(response.IsSuccessStatusCode);
        
        var data = JsonConvert.DeserializeObject<TestGetResponse>(response.Content.ReadAsStringAsync().Result);
        Assert.True(data.IsSuccessed);
        Assert.Equal(1, data.RandomNumber);
    }
}