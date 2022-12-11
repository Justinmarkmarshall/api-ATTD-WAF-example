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
//IClassFixture means only one api news up for all tests in the class
public class ImportControllerTests : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        IgnoreNullValues = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public ImportControllerTests(WebApplicationFactory<IApiMarker> appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }

    //Integration test
    [Fact]
    public async Task HitTheController()
    {
        var response = await _httpClient.GetAsync("import/");
        response.EnsureSuccessStatusCode(); 
    }
    //so this can infact be turned into an acceptance test, the question is,
    //if I run multiple feature files will there be multiple different instances of the API instantiated
    //i think so, but different files can be loaded in easily enough from the ACP project
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
        var response = await _httpClient.GetAsync($"import/confirm?location={location}");
        
        response.EnsureSuccessStatusCode(); 
    }
    
}