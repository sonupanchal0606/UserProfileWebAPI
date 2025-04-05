using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfileWebAPI.Data;
using UserProfileWebAPI.Model;

namespace UserProfileWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        // GET: api/Users
        [HttpGet]
        // asynchronous approach to fetch data ----> recommended for all DB operations
        /*        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
                {
                    return await _context.Users.ToListAsync();
                }*/

        // another way to write above async query

        public async Task<IActionResult> GetUsers()
        {
             var result = await _context.Users.ToListAsync();
             //var result = await (from user in _context.Users select user).ToListAsync();
             return Ok(result);
        }


        // synchronus approach to fetch data
        /*  public IActionResult GetUsers()
        {
            // var result = _context.Users.ToList();
            // or
            var result = (from user in _context.Users select user).ToList(); // alterantive to above approach. Both are same

            // Return the result wrapped in an Ok response
            return Ok(result); // This returns a successful HTTP response (status code 200) with the result (the list of users) serialized as JSON in the response body.

            // return result; --> this will not work with IActionResult
        }*/


        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.Include(u => u.Profile)
                .Where(u => u.UserId == id)
                .Include(u => u.Posts)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
