using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly FitCenterContext _context = new FitCenterContext();

        // GET: api/Assignments/GetList
        [Route("GetList")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetList()
        {
          if (_context.Assignment == null)
          {
              return NotFound();
          }
            return await _context.Assignment.ToListAsync();
        }

        // GET: api/Assignments/GetList
        [Route("GetListByUser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetListByUser(int id)
        {
            var assignments = await _context.Assignment
                .Where(a => a.UserId == id)
                .ToListAsync();

            if (assignments == null)
            {
                return NotFound();
            }

            return assignments;
        }

        // GET: api/Assignments/GetId/5
        [Route("GetId")]
        [HttpGet]
        public async Task<ActionResult<Assignment>> GetId(int id)
        {
          if (_context.Assignment == null)
          {
              return NotFound();
          }
            var assignment = await _context.Assignment.FindAsync(id);

            if (assignment == null)
            {
                return NotFound();
            }

            return assignment;
        }

        // PUT: api/Assignments/Edit/5
        [Route("Edit")]
        [HttpPut]
        public async Task<IActionResult> Edit(int id, Assignment assignment)
        {
            if (id != assignment.AssignmentId)
            {
                return BadRequest();
            }

            _context.Entry(assignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(id))
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

        // POST: api/Assignments/Insert
        [Route("Insert")]
        [HttpPost]
        public async Task<ActionResult<Assignment>> Insert(AssignmentPost assignment)
        {
          if (_context.Assignment == null)
          {
              return Problem("Entity set 'FitCenterContext.Assignment'  is null.");
          }
            Assignment newAssignment = new Assignment
            {
                ClassId = assignment.ClassId,
                UserId = assignment.UserId,
                AssignmentDate = assignment.AssignmentDate,
                Status = assignment.Status 
            };
            _context.Assignment.Add(newAssignment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetId", new { id = assignment.AssignmentId }, assignment);
        }

        // DELETE: api/Assignments/Delete/5
        [Route("Delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Assignment == null)
            {
                return NotFound();
            }
            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }

            _context.Assignment.Remove(assignment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssignmentExists(int id)
        {
            return (_context.Assignment?.Any(e => e.AssignmentId == id)).GetValueOrDefault();
        }
    }
}
