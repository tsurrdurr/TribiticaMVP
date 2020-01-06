using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TribiticaMVP.Contracts;
using TribiticaMVP.Models;

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
        public async Task<ActionResult<IEnumerable<GoalYear>>> GetAllYear()
        {
            return await _context.GoalsYear.ToListAsync();
        }

        [HttpGet(APIRoutes.Goals.Year.GetById)]
        public async Task<ActionResult<GoalYear>> GetByIdYear(Guid id)
        {
            var goalYear = await _context.GoalsYear.FindAsync(id);

            if (goalYear == null)
            {
                return NotFound();
            }

            return goalYear;
        }

        // PUT: api/Goals/5
        [HttpPut(APIRoutes.Goals.Year.Put)]
        public async Task<IActionResult> PutYear(Guid id, GoalYear goalYear)
        {
            if (id != goalYear.Id)
            {
                return BadRequest();
            }

            var goalYearPrevious = _context.GoalsYear.Find(id);
            ProjectNonNullGoalProperties(goalYear, goalYearPrevious);
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
        public async Task<ActionResult<GoalYear>> PostYear(GoalYear goalYear)
        {
            _context.GoalsYear.Add(goalYear);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGoalYear", new { id = goalYear.Id }, goalYear);
        }

        [HttpDelete(APIRoutes.Goals.Year.Delete)]
        public async Task<ActionResult<GoalYear>> DeleteYear(Guid id)
        {
            var goalYear = await _context.GoalsYear.FindAsync(id);
            if (goalYear == null)
            {
                return NotFound();
            }

            _context.GoalsYear.Remove(goalYear);
            await _context.SaveChangesAsync();

            return goalYear;
        }

        private bool GoalYearExists(Guid id)
        {
            return _context.GoalsYear.Any(e => e.Id == id);
        }

        private static void ProjectNonNullGoalProperties(IGoal newGoal, IGoal previousGoal)
        {
            previousGoal.Header = newGoal.Header ?? previousGoal.Header;
            previousGoal.Description = newGoal.Description ?? previousGoal.Description;
        }

    }
}
