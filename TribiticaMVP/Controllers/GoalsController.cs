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

namespace TribiticaMVP.Controllers
{
    public class GoalsController : ControllerBase
    {
        private readonly TribiticaDbContext _context;

        public GoalsController()
        {
            _context = new TribiticaDbContext();
        }

        [HttpGet(APIRoutes.Goals.Year.GetAll)]
        public async Task<ActionResult<IEnumerable<GoalViewModel>>> GetAllYear()
        {
            var userGuid = GetUserId();
            if (userGuid == null)
                return Forbid();

            var goals = await _context
                .GoalsYear
                .Where(x => x.OwnerId == userGuid)
                .ToListAsync();
            return goals.Select(x => x.ToViewModel()).ToList();
        }

        [HttpGet(APIRoutes.Goals.Week.GetAll)]
        public async Task<ActionResult<IEnumerable<GoalViewModel>>> GetAllWeek()
        {
            var userGuid = GetUserId();
            if (userGuid == null)
                return Forbid();

            var goals = await _context
                .GoalsWeek
                .Where(x => x.OwnerId == userGuid)
                .ToListAsync();
            return goals.Select(x => x.ToViewModel()).ToList();
        }

        [HttpGet(APIRoutes.Goals.Year.GetById)]
        public async Task<ActionResult<GoalViewModel>> GetByIdYear(Guid id)
        {
            var goalYear = await _context.GoalsYear.FindAsync(id);

            if (goalYear == null)
            {
                return NotFound();
            }

            return goalYear.ToViewModel();
        }

        [HttpGet(APIRoutes.Goals.Week.GetById)]
        public async Task<ActionResult<GoalViewModel>> GetByIdWeek(Guid id)
        {
            var goalWeek = await _context.GoalsWeek.FindAsync(id);

            if (goalWeek == null)
            {
                return NotFound();
            }

            return goalWeek.ToViewModel();
        }

        // PUT: api/Goals/5
        [HttpPut(APIRoutes.Goals.Year.Put)]
        public async Task<IActionResult> PutYear(Guid id, GoalViewModel goalYear)
        {
            if (id != goalYear.Id)
            {
                return BadRequest();
            }

            var goalYearPrevious = _context.GoalsYear.Find(id);
            Mapping.ProjectNonNullGoalProperties(goalYear, goalYearPrevious);
            _context.GoalsYear.Update(goalYearPrevious);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoalYearExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost(APIRoutes.Goals.Year.Post)]
        public async Task<ActionResult<GoalViewModel>> PostYear(GoalViewModel goalYear)
        {
            goalYear.CreatedTimeStamp = DateTimeOffset.Now;
            var entry = _context.GoalsYear.Add(goalYear.ToYearDb());
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByIdYear), new { id = entry.Entity.Id }, entry.Entity);
        }

        [HttpPost(APIRoutes.Goals.Week.Post)]
        public async Task<ActionResult<GoalViewModel>> PostWeek(GoalViewModel goalWeek)
        {
            goalWeek.CreatedTimeStamp = DateTimeOffset.Now;
            var entry = _context.GoalsWeek.Add(goalWeek.ToWeekDb());
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByIdWeek), new { id = entry.Entity.Id }, entry.Entity);
        }

        [HttpDelete(APIRoutes.Goals.Year.Delete)]
        public async Task<ActionResult> DeleteYear(Guid id)
        {
            var goalYear = await _context.GoalsYear.FindAsync(id);
            if (goalYear == null)
            {
                return NotFound();
            }

            _context.GoalsYear.Remove(goalYear);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private Guid? GetUserId()
        {
            HttpContext.Session.TryGetValue("UserID", out var userGuidBytes);
            if (userGuidBytes == null)
                return null;
            return new Guid(userGuidBytes);
        }

        private bool GoalYearExists(Guid id)
        {
            return _context.GoalsYear.Any(e => e.Id == id);
        }
    }
}
