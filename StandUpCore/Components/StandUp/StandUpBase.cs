using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using StandUpCore.Common;
using StandUpCore.Models;
using StandUpCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandUpCore
{
    public class StandUpBase : ComponentBase
    {
        [Inject]
        protected CredentialService CredentialService { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected StandUp StandUp { get; set; }

        protected bool HasCredentials { get; set; }

        protected string GenerateLabel { get; set; }

        protected override async Task OnInitAsync()
        {
            StandUp = new StandUp();
            HasCredentials = (await CredentialService.GetCredentialsAsync()).Any();
            GenerateLabel = "Generate!";
        }

        protected async Task GenerateAsync()
        {
            // window.copyToClipboard is a custom function declared at the bottom of wwwroot/index.html
            await JSRuntime.InvokeAsync<string>("copyToClipboard", StandUp.Generate());

            GenerateLabel = "Copied to Clipboard!";
            StateHasChanged();

            await Task.Delay(2500).ContinueWith(t =>
            {
                GenerateLabel = "Generate!";
                StateHasChanged();
            });
        }
    }
}
