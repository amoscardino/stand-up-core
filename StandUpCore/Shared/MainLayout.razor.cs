using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using StandUpCore.Services;

namespace StandUpCore.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public IConfiguration Configuration { get; set; }

        private string AppVersion { get; set; }
        private string BlazorVersion { get; set; }

        protected override void OnInitialized()
        {
            AppVersion = Configuration.GetValue<string>("AppVersion");
            BlazorVersion = Configuration.GetValue<string>("BlazorVersion");

            this.StateHasChanged();
        }
    }
}
