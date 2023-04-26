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
    public class UsersController : ControllerBase
    {
        private readonly FitCenterContext _context = new FitCenterContext();

        // GET: api/Users/GetList
        [Route("GetList")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetList()
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/GetId/5
        [Route("GetId")]
        [HttpGet]
        public async Task<ActionResult<User>> GetId(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Users/GetLogin
        [Route("GetLogin")]
        [HttpGet]
        public async Task<ActionResult<User>?> GetLogin(string email, string password)
        {
            // Buscar el usuario en la base de datos por correo electrónico y contraseña
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
                return null;
            else
                return user;
        }

        // PUT: api/Users/Edit/5
        [Route("Edit")] 
        [HttpPut]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users/Insert
        [Route("Insert")]
        [HttpPost]
        public async Task<ActionResult<User>> Insert(User user)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'FitCenterContext.User'  is null.");
            }
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetId", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/Delete/5
        [Route("Delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}

