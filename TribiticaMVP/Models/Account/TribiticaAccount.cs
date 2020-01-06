using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace TribiticaMVP.Models
{
    public class TribiticaAccount
    {
        public Guid ID { get; set; }

        [Required]
        [Display(Name = "Account name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Your e-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Short description of yourself (optional)")]
        public string SelfSummary { get; set; }

        [NotMapped]
        public ICollection<GoalYear> GoalsYear { get; set; }
    }
}
