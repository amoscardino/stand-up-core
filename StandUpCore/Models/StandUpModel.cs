using StandUpCore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandUpCore.Models
{
    public class StandUpModel
    {
        public List<JiraTask> CurrentTasks { get; set; }
        public List<JiraTask> DoneTasks { get; set; }
        public List<JiraTask> BlockedTasks { get; set; }

        public string PreviousDayLabel { get; set; }
        public string PreviousDaySummary { get; set; }
        public string TodaySummary { get; set; }
        public string BlockedSummary { get; set; }

        public StandUpModel()
        {
            CurrentTasks = new List<JiraTask>();
            DoneTasks = new List<JiraTask>();
            BlockedTasks = new List<JiraTask>();

            PreviousDayLabel = "Yesterday";
            PreviousDaySummary = string.Empty;
            TodaySummary = string.Empty;
            BlockedSummary = string.Empty;
        }

        public string Generate()
        {
            var sb = new StringBuilder();

            sb.AppendLine("*Current*:");

            foreach (var task in CurrentTasks)
                sb.AppendLine(task.GetLine());

            if (!CurrentTasks.Any())
                sb.AppendLine("none");

            sb.AppendLine();
            sb.AppendLine("*Done*:");

            foreach (var task in DoneTasks)
                sb.AppendLine(task.GetLine());

            if (!DoneTasks.Any())
                sb.AppendLine("none");

            sb.AppendLine();
            sb.AppendLine("*Blocked*:");

            foreach (var task in BlockedTasks)
                sb.AppendLine(task.GetLine());

            if (!BlockedTasks.Any())
                sb.AppendLine("none");

            sb.AppendLine();
            sb.Append($"*{PreviousDayLabel}*: ");
            sb.AppendLine(string.IsNullOrWhiteSpace(PreviousDaySummary) ? "none" : PreviousDaySummary);
            sb.AppendLine();
            sb.Append("*Today*: ");
            sb.AppendLine(string.IsNullOrWhiteSpace(TodaySummary) ? "none" : TodaySummary);
            sb.AppendLine();
            sb.Append("*Impediments*: ");
            sb.Append(string.IsNullOrWhiteSpace(BlockedSummary) ? "none" : BlockedSummary);

            return sb.ToString();
        }
    }
}
