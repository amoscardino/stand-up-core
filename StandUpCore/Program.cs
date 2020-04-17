using Blazor.Extensions.Storage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StandUpCore.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StandUpCore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            // Register services
            builder.Services.AddStorage();
            builder.Services.AddTransient<CredentialService>();
            builder.Services.AddTransient<JiraService>();

            // Register root component(s)
            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }
    }
}
