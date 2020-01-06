using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TribiticaMVP.Models.Abstractions
{
    public class GoalWeek : IGoal
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        [NotMapped]
        public TribiticaAccount Owner { get; set; }

        public string Header { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreatedTimeStamp { get; set; }

        public DateTimeOffset? ProjectedDate { get; set; }

        public Guid? Parent { get; set; }
    }
}
