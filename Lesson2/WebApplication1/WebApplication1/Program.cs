using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Extensions.Http;
using WebApplication1;
using JsonSerializer = System.Text.Json.JsonSerializer;

var stopWatch = new System.Diagnostics.Stopwatch();
stopWatch.Start();

var mainTask = MainAsync();
var task2 = mainTask.ContinueWith(t => DelayAsync());


Task MainAsync()
{
    return Task.Delay(100);
}

int GetInt()
{
    return 0;
}

Task DelayAsync()
{
    return Task.Delay(500);
}






//var ipAddressList = Dns.GetHostAddresses("google.com");



var services = new ServiceCollection();

services.AddHttpClient();

//services.AddHttpClient<MyHttpClientService>();

var httpClient1Name = "httpClient1";
services.AddTransient<HttpDelegateHandler>();
services.AddHttpClient(httpClient1Name, httpClient =>
    {
        httpClient.BaseAddress = new Uri("http://localhost:5031/");

        httpClient.DefaultRequestHeaders.Add(
            HeaderNames.Accept, MediaTypeNames.Application.Json);
        httpClient.DefaultRequestHeaders.Add(
            HeaderNames.UserAgent, "HttpRequestsSample");
    })
    .AddHttpMessageHandler<HttpDelegateHandler>()
    .AddPolicyHandler(GetRetryPolicy());

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .OrResult(msg =>
        {
            return msg.StatusCode == HttpStatusCode.OK;
        })
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(3)); // TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

var serviceProvider = services.BuildServiceProvider();

var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

var httpClient = httpClientFactory.CreateClient();
//await GetValuesAsync(httpClient);

/*var result = await httpClient.GetAsync("http://localhost:5031/operation/users");
var jsonArray = await GetArrayDataAsync(result);
Console.WriteLine(jsonArray);*/

//var myHttpClient = serviceProvider.GetRequiredService<MyHttpClientService>();

var httpClient1 = httpClientFactory.CreateClient(httpClient1Name);

await GetValuesAsync(httpClient1);

async Task GetValuesAsync(HttpClient httpClient)
{
    using var httpResponseMessage = await httpClient.GetAsync("operation/users");

    var array = await GetArrayDataAsync(httpResponseMessage);

    Console.WriteLine(array);
}

async Task<JArray> GetArrayDataAsync(HttpResponseMessage httpResponseMessage)
{
    var json = await httpResponseMessage.Content.ReadAsStringAsync();
    var array = JArray.Parse(json);

    return array;
}

var todoItem = new List<string> { "1231", "21312" };

var todoItemJson = new StringContent(
    JsonSerializer.Serialize(todoItem),
    Encoding.UTF8,
    MediaTypeNames.Application.Json);
 
using var httpResponseMessage2 =
    await httpClient.PostAsync("/api/TodoItems", todoItemJson);


Console.Read();
    