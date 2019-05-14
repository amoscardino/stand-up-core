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
            SetAuthHeader(credential);

            var issue = await _httpClient.GetJsonAsync<dynamic>($"{credential.SiteUrl}/rest/api/2/{key}");

            if (issue == null)
                return null;

            try
            {
                return new JiraTask
                {
                    Key = issue.key,
                    Project = issue.fields.project.name,
                    Summary = issue.fields.summary,
                    OriginalEstimate = (decimal)issue.fields.timetracking.originalEstimateSeconds / 60 / 60,
                    HoursRemaining = (decimal)issue.fields.timetracking.remainingEstimateSeconds / 60 / 60,
                    HoursComplete = (decimal)issue.fields.timetracking.timeSpentSeconds / 60 / 60
                };
            }
            catch { return null; }
        }

        private void SetAuthHeader(JiraCredential credential)
        {
            var headerValue = $"{credential.Email}:{credential.ApiToken}";
            var headerValueEncoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(headerValue));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", headerValueEncoded);
        }
    }
}
