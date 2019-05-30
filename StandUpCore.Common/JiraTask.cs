using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandUpCore.Common
{
    public class JiraTask
    {
        public string Key { get; set; }
        public string Project { get; set; }
        public string Summary { get; set; }
        public decimal OriginalEstimate { get; set; }
        public decimal HoursRemaining { get; set; }
        public decimal HoursComplete { get; set; }

        public string GetLine()
            => $"Project: {Project}, Task: {Key}, OE: {OriginalEstimate}, HR: {HoursRemaining}, HC: {HoursComplete} -- {Summary}";
    }
}
