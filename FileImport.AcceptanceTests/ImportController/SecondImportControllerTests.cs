using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FileImport.Api;
using FileImport.Api.Contracts.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FileImport.AcceptanceTests.ImportController;

public class SecondImportControllerTests : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        IgnoreNullValues = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public SecondImportControllerTests(WebApplicationFactory<IApiMarker> appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }
    
    [Fact]
    public async Task ImportTheFileShouldPass()
    {
        var toConvert = new ImportRequest()
        {
            InputLocation = "\\..\\..\\..\\Files\\Data2.json",
            OutputLocation = "\\..\\..\\..\\Files\\ImportTextAcpTest2.txt"
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
        var location = "\\..\\..\\..\\Files\\ImportTextAcpTest2.txt";
        var response = await _httpClient.GetAsync($"import/confirm?location={location}");
        response.EnsureSuccessStatusCode(); // Status Code 200-299
    }
    
}