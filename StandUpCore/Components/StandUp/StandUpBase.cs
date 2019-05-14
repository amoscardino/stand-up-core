using Microsoft.AspNetCore.Components;
using StandUpCore.Common;
using StandUpCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandUpCore
{
    public class StandUpBase : ComponentBase
    {
        [Inject]
        protected JiraService JiraService { get; set; }

        protected JiraTask JiraTask { get; set; }

        protected override async Task OnInitAsync()
        {
            JiraTask = await JiraService.GetTaskAsync("P2-87");
        }
    }
}
