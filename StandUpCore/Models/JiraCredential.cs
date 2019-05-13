using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StandUpCore.Models
{
    public class JiraCredential
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Site URL is required.")]
        public string SiteUrl { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "API Token is required.")]
        public string ApiToken { get; set; }
    }
}
