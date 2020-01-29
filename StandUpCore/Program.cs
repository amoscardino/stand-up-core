using Blazor.Extensions.Logging;
using Blazor.Extensions.Storage;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StandUpCore.Services;
using System.Threading.Tasks;

namespace StandUpCore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Register services
            builder.Services.AddStorage();
            builder.Services.AddLogging(builder => builder.AddBrowserConsole().SetMinimumLevel(LogLevel.Trace));
            builder.Services.AddTransient<ConfigurationService>();
            builder.Services.AddTransient<CredentialService>();
            builder.Services.AddTransient<JiraService>();

            // Register root component(s)
            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }
    }
}
