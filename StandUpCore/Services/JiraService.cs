using Microsoft.AspNetCore.Components;
using StandUpCore.Common;
using StandUpCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StandUpCore.Services
{
    public class JiraService
    {
        private readonly CredentialService _credentialService;
        private readonly HttpClient _httpClient;

        public JiraService(CredentialService credentialService, HttpClient httpClient)
        {
            _credentialService = credentialService;
            _httpClient = httpClient;
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
            var url = "https://standupcore-functions.azurewebsites.net/api/jirafunction";
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString.Add("key", key);
            queryString.Add("site", credential.SiteUrl);
            queryString.Add("email", credential.Email);
            queryString.Add("token", credential.ApiToken);

            return await _httpClient.GetJsonAsync<JiraTask>($"{url}?{queryString}");
        }
    }
}
