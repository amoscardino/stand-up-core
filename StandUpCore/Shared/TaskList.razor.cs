using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using StandUpCore.Common;
using StandUpCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandUpCore.Shared
{
    public partial class TaskList
    {
        [Inject]
        public JiraService JiraService { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public List<JiraTask> Tasks { get; set; }

        [Parameter]
        public EventCallback<List<JiraTask>> TasksChanged { get; set; }

        private string Keys { get; set; }

        protected override void OnInitialized()
        {
            Keys = string.Empty;
            Tasks = new List<JiraTask>();
        }

        private async Task GetTasksAsync()
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
