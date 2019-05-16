using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using StandUpCore.Common;
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

        protected bool HasCredentials { get; set; }

        protected List<JiraTask> CurrentTasks { get; set; }
        protected List<JiraTask> DoneTasks { get; set; }
        protected List<JiraTask> BlockedTasks { get; set; }

        public string PreviousDayLabel { get; set; }
        public string PreviousDaySummary { get; set; }
        public string TodaySummary { get; set; }
        public string BlockedSummary { get; set; }

        protected string GenerateLabel { get; set; }

        protected override async Task OnInitAsync()
        {
            HasCredentials = (await CredentialService.GetCredentialsAsync()).Any();

            CurrentTasks = new List<JiraTask>();
            DoneTasks = new List<JiraTask>();
            BlockedTasks = new List<JiraTask>();

            PreviousDayLabel = "Yesterday";
            PreviousDaySummary = string.Empty;
            TodaySummary = string.Empty;
            BlockedSummary = string.Empty;

            GenerateLabel = "Generate!";
        }

        protected async Task GenerateAsync()
        {
            var standup = new StringBuilder();

            standup.AppendLine("Current:");

            foreach (var task in CurrentTasks)
                standup.AppendLine(GetLine(task));

            if (!CurrentTasks.Any())
                standup.AppendLine("none");

            standup.AppendLine();
            standup.AppendLine("Done:");

            foreach (var task in DoneTasks)
                standup.AppendLine(GetLine(task));

            if (!DoneTasks.Any())
                standup.AppendLine("none");

            standup.AppendLine();
            standup.AppendLine("Blocked:");

            foreach (var task in BlockedTasks)
                standup.AppendLine(GetLine(task));

            if (!BlockedTasks.Any())
                standup.AppendLine("none");

            standup.AppendLine();
            standup.Append($"{PreviousDayLabel}: ");
            standup.AppendLine(string.IsNullOrWhiteSpace(PreviousDaySummary) ? "none" : PreviousDaySummary);
            standup.AppendLine();
            standup.Append("Today: ");
            standup.AppendLine(string.IsNullOrWhiteSpace(TodaySummary) ? "none" : TodaySummary);
            standup.AppendLine();
            standup.Append("Impediments: ");
            standup.Append(string.IsNullOrWhiteSpace(BlockedSummary) ? "none" : BlockedSummary);

            // window.copyToClipboard is a custom function declared at the bottom of wwwroot/index.html
            await JSRuntime.InvokeAsync<string>("copyToClipboard", standup.ToString());

            GenerateLabel = "Copied to Clipboard!";
            StateHasChanged();

            await Task.Delay(2500).ContinueWith(t =>
            {
                GenerateLabel = "Generate!";
                StateHasChanged();
            });
        }

        private string GetLine(JiraTask task)
            => $"Project: {task.Project}, Task: {task.Key}, OE: {task.OriginalEstimate}, HR: {task.HoursRemaining}, HC: {task.HoursComplete} -- {task.Summary}";
    }
}
