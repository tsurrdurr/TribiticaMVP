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

        [HttpGet(APIRoutes.Goals.Day.GetAll)]
        public async Task<ActionResult<IEnumerable<GoalViewModel>>> GetAllDay()
        {
            var userGuid = GetUserId();
            if (userGuid == null)
                return Forbid();

            var goals = await _goalDayService.GetAll(userGuid.Value);
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

        [HttpGet(APIRoutes.Goals.Day.GetById)]
        public async Task<ActionResult<GoalViewModel>> GetByIdDay(Guid id)
        {
            var goalWeek = await _goalDayService.Get(id);
            if (goalWeek == null)
                return NotFound();
            return goalWeek.ToViewModel();
        }

        // PUT: api/Goals/5
        [HttpPut(APIRoutes.Goals.Year.Put)]
        public async Task<ActionResult<GoalViewModel>> PutYear(Guid id, [FromBody] GoalViewModel goalViewModel)
        {
            if (id != goalViewModel.Id)
                return BadRequest();

            var goalYearPrevious = await _goalYearService.Update(goalViewModel.ToYearDb());
            return goalYearPrevious.ToViewModel();
        }

        [HttpPut(APIRoutes.Goals.Week.Put)]
        public async Task<ActionResult<GoalViewModel>> PutWeek(Guid id, [FromBody] GoalViewModel goalViewModel)
        {
            if (id != goalViewModel.Id)
                return BadRequest();

            var goalYearPrevious = await _goalWeekService.Update(goalViewModel.ToWeekDb());
            return goalYearPrevious.ToViewModel();
        }

        [HttpPut(APIRoutes.Goals.Day.Put)]
        public async Task<ActionResult<GoalViewModel>> PutDay(Guid id, [FromBody] GoalViewModel goalViewModel)
        {
            if (id != goalViewModel.Id)
                return BadRequest();

            var goalYearPrevious = await _goalDayService.Update(goalViewModel.ToDayDb());
            return goalYearPrevious.ToViewModel();
        }

        [HttpPost(APIRoutes.Goals.Year.Post)]
        public async Task<ActionResult<GoalViewModel>> PostYear([FromBody] GoalViewModel goalViewModel)
        {
            var userGuid = GetUserId();
            if (!userGuid.HasValue)
                return Forbid();

            goalViewModel.OwnerId = userGuid.Value;

            var entry = await _goalYearService.Add(goalViewModel.ToYearDb());
            return CreatedAtAction(nameof(GetByIdYear), new { id = entry.Id }, entry);
        }

        [HttpPost(APIRoutes.Goals.Week.Post)]
        public async Task<ActionResult<GoalViewModel>> PostWeek([FromBody] GoalViewModel goalViewModel)
        {
            var userGuid = GetUserId();
            if (!userGuid.HasValue)
                return Forbid();

            goalViewModel.OwnerId = userGuid.Value;

            var entry = await _goalWeekService.Add(goalViewModel.ToWeekDb());
            return CreatedAtAction(nameof(GetByIdYear), new { id = entry.Id }, entry);
        }

        [HttpPost(APIRoutes.Goals.Day.Post)]
        public async Task<ActionResult<GoalViewModel>> PostDay([FromBody] GoalViewModel goalViewModel)
        {
            var userGuid = GetUserId();
            if (!userGuid.HasValue)
                return Forbid();

            goalViewModel.OwnerId = userGuid.Value;

            var entry = await _goalDayService.Add(goalViewModel.ToDayDb());
            return CreatedAtAction(nameof(GetByIdYear), new { id = entry.Id }, entry);
        }

        [HttpDelete(APIRoutes.Goals.Year.Delete)]
        public async Task<ActionResult> DeleteYear(Guid id)
        {
            var success = await _goalYearService.Delete(id);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpDelete(APIRoutes.Goals.Week.Delete)]
        public async Task<ActionResult> DeleteWeek(Guid id)
        {
            var success = await _goalWeekService.Delete(id);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpDelete(APIRoutes.Goals.Day.Delete)]
        public async Task<ActionResult> DeleteDay(Guid id)
        {
            var success = await _goalDayService.Delete(id);
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
