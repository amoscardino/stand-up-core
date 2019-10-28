using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using StandUpCore.Services;

namespace StandUpCore.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public ConfigurationService ConfigService { get; set; }

        private string AppVersion { get; set; }
        private string BlazorVersion { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AppVersion = await ConfigService.GetValueAsync("AppVersion");
            BlazorVersion = await ConfigService.GetValueAsync("BlazorVersion");

            this.StateHasChanged();
        }
    }
}
