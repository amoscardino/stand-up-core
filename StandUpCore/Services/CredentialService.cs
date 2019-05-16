using Blazor.Extensions.Storage;
using StandUpCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandUpCore.Services
{
    public class CredentialService
    {
        private const string STORAGE_KEY = "StandUpCoreCredentials";

        private LocalStorage _localStorage;

        public CredentialService(LocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<List<JiraCredential>> GetCredentialsAsync()
        {
            var credentials = await _localStorage.GetItem<List<JiraCredential>>(STORAGE_KEY);

            return credentials ?? new List<JiraCredential>();
        }

        public async Task AddCredentialAsync(JiraCredential credential)
        {
            var credentials = await GetCredentialsAsync();

            credential.Id = Guid.NewGuid();

            credentials.Add(credential);

            await _localStorage.SetItem(STORAGE_KEY, credentials);
        }

        public async Task RemoveCredentialAsync(Guid id)
        {
            var credentials = await GetCredentialsAsync();

            credentials = credentials.Where(x => x.Id != id).ToList();

            await _localStorage.SetItem(STORAGE_KEY, credentials);
        }
    }
}
