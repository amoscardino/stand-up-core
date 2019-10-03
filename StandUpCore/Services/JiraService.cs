using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using StandUpCore.Common;
using StandUpCore.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace StandUpCore.Services
{
    public class JiraService
    {
        private readonly CredentialService _credentialService;
        private readonly HttpClient _httpClient;
        private readonly ConfigurationService _configuration;

        public JiraService(CredentialService credentialService, ConfigurationService configuration, HttpClient httpClient)
        {
            _credentialService = credentialService;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<JiraTask> GetTaskAsync(string key)
        {
            var credentials = await _credentialService.GetCredentialsAsync();

            foreach (var credential in credentials)
            {
                var task = await GetTaskAsync(key, credential);

                if (task != null)
                    return task;
            }

            return null;
        }

        private async Task<JiraTask> GetTaskAsync(string key, JiraCredential credential)
        {
            var url = await _configuration.GetValueAsync("JiraServiceUrl");
            var queryBuilder = new QueryBuilder
            {
                { "key", key },
                { "site", credential.SiteUrl },
                { "email", credential.Email },
                { "token", credential.ApiToken }
            };

            try
            {
                return await _httpClient.GetJsonAsync<JiraTask>(url + queryBuilder.ToQueryString());
            }
            catch
            {
                return null;
            }
        }
    }
}
