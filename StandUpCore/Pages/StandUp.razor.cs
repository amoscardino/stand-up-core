using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using StandUpCore.Common;
using StandUpCore.Models;
using StandUpCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StandUpCore.Pages
{
    public partial class StandUp
    {
        [Inject]
        public CredentialService CredentialService { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private StandUpModel Model { get; set; }
        private bool HasCredentials { get; set; }
        private string GenerateLabel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Model = new StandUpModel();
            HasCredentials = (await CredentialService.GetCredentialsAsync()).Any();
            GenerateLabel = "Generate!";
        }

        private async Task GenerateAsync()
        {
            // window.copyToClipboard is a custom function declared at the bottom of wwwroot/index.html
            await JSRuntime.InvokeAsync<string>("copyToClipboard", Model.Generate());

            GenerateLabel = "Copied to Clipboard!";
            this.StateHasChanged();

            await Task.Delay(2500).ContinueWith(t =>
            {
                GenerateLabel = "Generate!";
                this.StateHasChanged();
            });
        }
    }
}
