using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileShop.Auth;
using MobileShop.Models;

namespace MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MobileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MobileController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mobile>>> GetMobiles()
        {
            return await _context.Mobiles.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Mobile>> GetMobile(int id)
        {
            var mobile = await _context.Mobiles.FindAsync(id);

            if (mobile == null)
            {
                return NotFound();
            }

            return mobile;
        }

        
        [HttpPost]
        public async Task<ActionResult<Mobile>> PostMobile(Mobile mobile)
        {
            _context.Mobiles.Add(mobile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMobile", new { id = mobile.Id }, mobile);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMobile(int id, Mobile mobile)
        {
            if (id != mobile.Id)
            {
                return BadRequest();
            }

            _context.Entry(mobile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MobileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            
            return Ok(await _context.Mobiles.ToListAsync());
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMobile(int id)
        {
            var mobile = await _context.Mobiles.FindAsync(id);
            if (mobile == null)
            {
                return NotFound();
            }

            _context.Mobiles.Remove(mobile);
            await _context.SaveChangesAsync();

            
            return Ok(await _context.Mobiles.ToListAsync());
        }

        private bool MobileExists(int id)
        {
            return _context.Mobiles.Any(e => e.Id == id);
        }

    }
}
