using Blazor.Extensions.Logging;
using Blazor.Extensions.Storage;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StandUpCore.Services;

namespace StandUpCore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStorage();

            services.AddLogging(builder => builder.AddBrowserConsole().SetMinimumLevel(LogLevel.Trace));

            services.AddTransient<ConfigurationService>();
            services.AddTransient<CredentialService>();
            services.AddTransient<JiraService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
