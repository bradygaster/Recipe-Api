using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace RecipeApi
{
    class Program
    {
        static Task Main(string[] args) => 
            CreateHostBuilder(args)
                .Build()
#if DEBUG
                .ConfigureTestTunnel(builder =>
                {
                    builder
                        .UseNGrok()
                        .UseSwashbuckleOpenApiEndpoint()
                        .UseAzureApiMangement(new AzureApiManagementCreateApiOptions
                        {
                            ApiManagementServiceName = "recipe-apis-dev",
                            ResourceGroupName = "RecipeApp"
                        });
                })
#endif
                .RunAsync();

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
