using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using StandUpCore.Common;

namespace StandUpCore.Functions
{
    public static class JiraFunction
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        [FunctionName("JiraFunction")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var queryValues = req.GetQueryParameterDictionary();
            var key = queryValues["key"];
            var site = queryValues["site"];
            var email = queryValues["email"];
            var token = queryValues["token"];

            var headerValue = $"{email}:{token}";
            var headerValueEncoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(headerValue));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", headerValueEncoded);

            var response = await _httpClient.GetAsync($"{site}/rest/api/2/issue/{key}");

            if (response == null || !response.IsSuccessStatusCode)
                return null;

            var issue = await response.Content.ReadAsAsync<dynamic>();

            // Reset the auth header so we don't accidentally store the credentials. This is needed
            // as the HttpClient instance is static and reused between requests.
            _httpClient.DefaultRequestHeaders.Authorization = null;

            try
            {
                var task = new JiraTask
                {
                    Key = issue.key,
                    Project = issue.fields.project.name,
                    Summary = issue.fields.summary,
                    OriginalEstimate = (decimal)issue.fields.timetracking.originalEstimateSeconds / 60 / 60,
                    HoursRemaining = (decimal)issue.fields.timetracking.remainingEstimateSeconds / 60 / 60,
                    HoursComplete = (decimal)issue.fields.timetracking.timeSpentSeconds / 60 / 60
                };

                return new OkObjectResult(task);
            }
            catch { return null; }
        }
    }
}
