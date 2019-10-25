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

namespace StandUpCore.Pages
{
    public partial class Credentials
    {
        [Inject]
        public CredentialService CredentialService { get; set; }

        private List<JiraCredential> SavedCredentials { get; set; }
        private JiraCredential NewCredential { get; set; }

        protected override async Task OnInitializedAsync()
        {
            SavedCredentials = await CredentialService.GetCredentialsAsync();
            NewCredential = new JiraCredential();
        }

        private async Task SaveNewCredential()
        {
            await CredentialService.AddCredentialAsync(NewCredential);

            SavedCredentials = await CredentialService.GetCredentialsAsync();
            NewCredential = new JiraCredential();
        }

        private async Task DeleteCredential(Guid id)
        {
            await CredentialService.RemoveCredentialAsync(id);

            SavedCredentials = await CredentialService.GetCredentialsAsync();
        }
    }
}
