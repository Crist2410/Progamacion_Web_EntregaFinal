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
    public class ClassesController : ControllerBase
    {

        private readonly FitCenterContext _context = new FitCenterContext();

        // GET: api/Classes/GetId
        [Route("GetList")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetList()
        {
          if (_context.Class == null)
          {
              return NotFound();
          }
            return await _context.Class.ToListAsync();
        }

        // GET: api/Classes/GetId/5
        [Route("GetId")]
        [HttpGet]
        public async Task<ActionResult<Class>> GetId(int id)
        {
          if (_context.Class == null)
          {
              return NotFound();
          }
            var @class = await _context.Class.FindAsync(id);

            if (@class == null)
            {
                return NotFound();
            }

            return @class;
        }

        // PUT: api/Classes/Edit/5
        [Route("Edit")]
        [HttpPut]
        public async Task<IActionResult> Edit(int id, Class @class)
        {
            if (id != @class.ClassId)
            {
                return BadRequest();
            }

            _context.Entry(@class).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
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

        // POST: api/Classes
        [Route("Insert")]
        [HttpPost]
        public async Task<ActionResult<Class>> Insert(Class @class)
        {
          if (_context.Class == null)
          {
              return Problem("Entity set 'FitCenterContext.Class'  is null.");
          }
            _context.Class.Add(@class);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetId", new { id = @class.ClassId }, @class);
        }

        // DELETE: api/Classes/Delete/5
        [Route("Delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Class == null)
            {
                return NotFound();
            }
            var @class = await _context.Class.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            _context.Class.Remove(@class);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassExists(int id)
        {
            return (_context.Class?.Any(e => e.ClassId == id)).GetValueOrDefault();
        }
    }
}
