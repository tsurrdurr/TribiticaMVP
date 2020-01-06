using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TribiticaMVP.Models
{
    public interface IGoal
    {
        [Key]
        Guid Id { get; set; }

        Guid OwnerId { get; set; }

        string Header { get; set; }

        string Description { get; set; }

        DateTimeOffset CreatedTimeStamp { get; set; }

        DateTimeOffset? ProjectedDate { get; set; }

        Guid? Parent { get; set; }
    }
}
