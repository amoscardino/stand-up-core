using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using StandUpCore.Common;
using StandUpCore.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace StandUpCore.Services
{
    public class JiraService
    {
        private readonly CredentialService _credentialService;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public JiraService(CredentialService credentialService, HttpClient httpClient, IConfiguration configuration)
        {
            _credentialService = credentialService;
            _httpClient = httpClient;
            _baseUrl = configuration.GetValue<string>("JiraServiceUrl");
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
            var queryBuilder = new QueryBuilder
            {
                { "key", key },
                { "site", credential.SiteUrl },
                { "email", credential.Email },
                { "token", credential.ApiToken }
            };

            try
            {
                return await _httpClient.GetFromJsonAsync<JiraTask>(_baseUrl + queryBuilder.ToQueryString());
            }
            catch
            {
                return null;
            }
        }
    }
}
