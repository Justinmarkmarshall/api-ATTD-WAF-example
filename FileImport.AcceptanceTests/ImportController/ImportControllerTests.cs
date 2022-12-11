using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FileImport.Api;
using FileImport.Api.Contracts.Requests;
using FileImport.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FileImport.AcceptanceTests.ImportController;

//I have to create a Marker, which is an empty interface to mark the assembly
//it is an interface in the Api I am testing called IApiMarker
//IClassFixture means only one api sun up for all tests in the class
//this method below is taken from Nicks course
//public class ImportControllerTests : IClassFixture<WebApplicationFactory<IApiMarker>>
//this method below uses docs HERE https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
// public class ImportControllerTests : WebApplicationFactory<Program>
//try with just the normal instantiation without ClassFixture (2 below)
//get constructor parameters did not have matching fixture data error
//public class ImportControllerTests : WebApplicationFactory<IApiMarker>
public class ImportControllerTests : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    //old way would be to new up a HttpClint, but we are taking from the WAF
   
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        IgnoreNullValues = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    // private readonly WebApplicationFactory<Program> _factory;
    public ImportControllerTests(WebApplicationFactory<IApiMarker> appFactory)
    {
        //Nix way
        //_httpClient = appFactory.CreateClient();
        _httpClient = appFactory.CreateClient();
    }

    //Integration test
    //this succeeds in hitting the breakpoint in the controller
    [Fact]
    public async Task HitTheController()
    {
        var response = await _httpClient.GetAsync("import/");
        
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }
    //so this can infact be turned into an acceptance test, the question is, if I run multiple feature files will there be multiple different instances of the API instantiated
    [Fact]
    public async Task ImportTheFileShouldFail()
    {
        var toConvert = new ImportRequest()
        {
            InputLocation = "test",
            OutputLocation = "test"
        };
        var json = JsonSerializer.Serialize(toConvert);
        HttpRequestMessage request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("import/", UriKind.Relative),
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        
        var response = await _httpClient.SendAsync(request);

        var responseString = await response.Content.ReadAsStringAsync();

        var importControllerResponse = JsonSerializer.Deserialize<Result>(responseString, SerializerOptions);
        
        
        importControllerResponse?.Success.Should().BeFalse();
    }

    //does this create in the actual project file directory?
    [Fact]
    public async Task ImportTheFileShouldPass()
    {
        var toConvert = new ImportRequest()
        {
            InputLocation = "\\..\\..\\..\\Files\\Data.csv",
            OutputLocation = "\\..\\..\\..\\Files\\ImportTextAcpTest.txt"
        };

        var json = JsonSerializer.Serialize(toConvert);
        HttpRequestMessage request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("import/", UriKind.Relative),
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        
        var response = await _httpClient.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }
    
    [Fact]
    public async Task ShouldReturnTheCreatedFile()
    {
        var location = "\\..\\..\\..\\Files\\ImportTextAcpTest.txt";
        // string readFile = $"location={location}";
        //var response = await _httpClient.GetAsync($"import/confirm/{location.ToString()}");
        var response = await _httpClient.GetAsync($"import/confirm?location={location}");
        
        response.EnsureSuccessStatusCode(); // Status Code 200-299

        var text = await response.Content.ReadAsStringAsync();
        // text.Should().Contain("Unique Id");
    }
    
}