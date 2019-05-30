using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using StandUpCore.Common;
using StandUpCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandUpCore
{
    public class TaskListBase : ComponentBase
    {
        [Inject]
        protected JiraService JiraService { get; set; }

        [Parameter]
        protected string Label { get; set; }

        [Parameter]
        protected List<JiraTask> Tasks { get; set; }

        [Parameter]
        protected EventCallback<List<JiraTask>> TasksChanged { get; set; }

        protected string Keys { get; set; }

        protected override void OnInit()
        {
            Keys = string.Empty;
            Tasks = new List<JiraTask>();
        }

        protected async Task GetTasksAsync()
        {
            Tasks = new List<JiraTask>();

            var keys = Keys.Split(' ').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            foreach (var key in keys)
            {
                var task = await JiraService.GetTaskAsync(key);

                if (task != null)
                    Tasks.Add(task);

                await TasksChanged.InvokeAsync(Tasks);
            }
        }
    }
}
