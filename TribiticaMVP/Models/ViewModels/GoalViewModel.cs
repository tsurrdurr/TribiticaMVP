using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TribiticaMVP.Models.ViewModels
{
    public class GoalViewModel : IGoal
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public string Header { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreatedTimeStamp { get; set; }

        public DateTimeOffset? ProjectedDate { get; set; }

        public Guid? Parent { get; set; }
    }
}
