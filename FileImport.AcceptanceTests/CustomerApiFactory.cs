using FileImport.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace FileImport.AcceptanceTests;

//implement factory as it's own class so implement from the controller tests
public class CustomerApiFactory : WebApplicationFactory<Startup>
{
    //protected override void ConfigureWebHost(IWebHostBuilder builder)
    // {
    //     builder.ConfigureLogging(logging =>
    //     {
    //         logging.ClearProviders();
    //     });
    // }
}