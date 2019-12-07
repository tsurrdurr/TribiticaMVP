using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TribiticaMVP.Models;

namespace TribiticaMVP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalsController : ControllerBase
    {
        private readonly TribiticaDbContext _context;

        public GoalsController()
        {
            _context = new TribiticaDbContext();
        }

        // GET: api/Goals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GoalYear>>> GetGoalsYear()
        {
            return await _context.GoalsYear.ToListAsync();
        }

        // GET: api/Goals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GoalYear>> GetGoalYear(Guid id)
        {
            var goalYear = await _context.GoalsYear.FindAsync(id);

            if (goalYear == null)
            {
                return NotFound();
            }

            return goalYear;
        }

        // PUT: api/Goals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGoalYear(Guid id, GoalYear goalYear)
        {
            if (id != goalYear.Id)
            {
                return BadRequest();
            }

            _context.Entry(goalYear).State = EntityState.Modified;

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

        // POST: api/Goals
        [HttpPost]
        public async Task<ActionResult<GoalYear>> PostGoalYear(GoalYear goalYear)
        {
            _context.GoalsYear.Add(goalYear);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGoalYear", new { id = goalYear.Id }, goalYear);
        }

        // DELETE: api/Goals/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GoalYear>> DeleteGoalYear(Guid id)
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
    }
}
