using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TribiticaMVP.Contracts;
using TribiticaMVP.Models;
using TribiticaMVP.Models.Abstractions;
using TribiticaMVP.Models.ViewModels;
using TribiticaMVP.Services;

namespace TribiticaMVP.Controllers
{
    public class GoalsController : ControllerBase
    {
        private readonly IGoalService<GoalYear> _goalYearService;
        private readonly IGoalService<GoalWeek> _goalWeekService;
        private readonly IGoalService<GoalDay> _goalDayService;

        public GoalsController(IGoalService<GoalYear> goalYearService, IGoalService<GoalWeek> goalWeekService, IGoalService<GoalDay> goalDayService)
        {
            _goalYearService = goalYearService;
            _goalWeekService = goalWeekService;
            _goalDayService = goalDayService;
        }

        [HttpGet(APIRoutes.Goals.Year.GetAll)]
        public async Task<ActionResult<IEnumerable<GoalViewModel>>> GetAllYear()
        {
            var userGuid = GetUserId();
            if (!userGuid.HasValue)
                return Forbid();

            var goals = await _goalYearService.GetAll(userGuid.Value);
            return goals.Select(x => x.ToViewModel()).ToList();
        }

        [HttpGet(APIRoutes.Goals.Week.GetAll)]
        public async Task<ActionResult<IEnumerable<GoalViewModel>>> GetAllWeek()
        {
            var userGuid = GetUserId();
            if (userGuid == null)
                return Forbid();

            var goals = await _goalWeekService.GetAll(userGuid.Value);
            return goals.Select(x => x.ToViewModel()).ToList();
        }

        [HttpGet(APIRoutes.Goals.Year.GetById)]
        public async Task<ActionResult<GoalViewModel>> GetByIdYear(Guid id)
        {
            var goalYear = await _goalYearService.Get(id);
            if (goalYear == null)
                return NotFound();
            return goalYear.ToViewModel();
        }

        [HttpGet(APIRoutes.Goals.Week.GetById)]
        public async Task<ActionResult<GoalViewModel>> GetByIdWeek(Guid id)
        {
            var goalWeek = await _goalWeekService.Get(id);
            if (goalWeek == null)
                return NotFound();
            return goalWeek.ToViewModel();
        }

        // PUT: api/Goals/5
        [HttpPut(APIRoutes.Goals.Year.Put)]
        public async Task<ActionResult<GoalViewModel>> PutYear(Guid id, GoalViewModel goalYear)
        {
            if (id != goalYear.Id)
                return BadRequest();

            var goalYearPrevious = await _goalYearService.Update(goalYear.ToYearDb());
            return goalYearPrevious.ToViewModel();
        }

        [HttpPost(APIRoutes.Goals.Year.Post)]
        public async Task<ActionResult<GoalViewModel>> PostYear(GoalViewModel goalYear)
        {
            var userGuid = GetUserId();
            if (!userGuid.HasValue)
                return Forbid();

            goalYear.OwnerId = userGuid.Value;

            var entry = await _goalYearService.Add(goalYear.ToYearDb());
            return CreatedAtAction(nameof(GetByIdYear), new { id = entry.Id }, entry);
        }

        [HttpPost(APIRoutes.Goals.Week.Post)]
        public async Task<ActionResult<GoalViewModel>> PostWeek(GoalViewModel goalWeek)
        {
            var userGuid = GetUserId();
            if (!userGuid.HasValue)
                return Forbid();

            goalWeek.OwnerId = userGuid.Value;

            var entry = await _goalWeekService.Add(goalWeek.ToWeekDb());
            return CreatedAtAction(nameof(GetByIdYear), new { id = entry.Id }, entry);
        }

        [HttpDelete(APIRoutes.Goals.Year.Delete)]
        public async Task<ActionResult> DeleteYear(Guid id)
        {
            var success = await _goalYearService.Delete(id);
            if (!success) return NotFound();
            return Ok();
        }

        private Guid? GetUserId()
        {
            HttpContext.Session.TryGetValue("UserID", out var userGuidBytes);
            if (userGuidBytes == null)
                return null;
            return new Guid(userGuidBytes);
        }
    }
}
