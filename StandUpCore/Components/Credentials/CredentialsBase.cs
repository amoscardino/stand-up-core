using Blazor.Extensions.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using StandUpCore.Models;
using StandUpCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandUpCore
{
    public class CredentialsBase : ComponentBase
    {
        [Inject]
        protected CredentialService CredentialService { get; set; }

        protected List<JiraCredential> Credentials { get; set; } = new List<JiraCredential>();

        protected JiraCredential NewCredential { get; set; } = new JiraCredential();

        protected override async Task OnInitAsync()
        {
            Credentials = await CredentialService.GetCredentialsAsync();
        }

        protected async Task SaveNewCredential()
        {
            await CredentialService.AddCredentialAsync(NewCredential);
            Credentials = await CredentialService.GetCredentialsAsync();
            NewCredential = new JiraCredential();
        }

        protected async Task DeleteCredential(Guid id)
        {
            await CredentialService.RemoveCredentialAsync(id);

            Credentials = await CredentialService.GetCredentialsAsync();
        }
    }
}
