using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TribiticaMVP.Models.Abstractions;
using TribiticaMVP.Models.ViewModels;

namespace TribiticaMVP.Models
{
    public static class Mapping
    {
        public static GoalViewModel ToViewModel(this IGoal goal)
        {
            var vm = new GoalViewModel();
            CopyFields(goal, vm);
            return vm;
        }

        public static GoalYear ToYearDb(this IGoal goal)
        {
            var year = new GoalYear();
            CopyFields(goal, year);
            return year;
        }

        public static GoalWeek ToWeekDb(this IGoal goal)
        {
            var week = new GoalWeek();
            CopyFields(goal, week);
            return week;
        }

        public static GoalDay ToDayDb(this IGoal goal)
        {
            var day = new GoalDay();
            CopyFields(goal, day);
            return day;
        }

        public static void CopyFields(IGoal source, IGoal recepient)
        {
            recepient.Id = source.Id;
            recepient.OwnerId = source.OwnerId;
            recepient.Header = source.Header;
            recepient.Parent = source.Parent;
            recepient.Description = source.Description;
            recepient.CreatedTimeStamp = source.CreatedTimeStamp;
            recepient.ProjectedDate = source.ProjectedDate;
        }

        public static void ProjectNonNullGoalProperties(IGoal newGoal, IGoal previousGoal)
        {
            previousGoal.Header = newGoal.Header ?? previousGoal.Header;
            previousGoal.Description = newGoal.Description ?? previousGoal.Description;
            previousGoal.Parent = newGoal.Parent ?? previousGoal.Parent;
            previousGoal.CreatedTimeStamp = newGoal.CreatedTimeStamp != new DateTimeOffset() ? newGoal.CreatedTimeStamp : previousGoal.CreatedTimeStamp;
            previousGoal.ProjectedDate = newGoal.ProjectedDate ?? previousGoal.ProjectedDate;
        }
    }
}
