using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileShop.Auth;
using MobileShop.Models;
using Serilog;  

namespace MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MobileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MobileController> _logger;  

        public MobileController(ApplicationDbContext context, ILogger<MobileController> logger)  
        {
            _context = context;
            _logger = logger;  
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mobile>>> GetMobiles()
        {
            _logger.LogInformation("Fetching all mobiles");  
            return await _context.Mobiles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mobile>> GetMobile(int id)
        {
            _logger.LogInformation("Fetching mobile with ID {MobileId}", id);  
            var mobile = await _context.Mobiles.FindAsync(id);

            if (mobile == null)
            {
                _logger.LogWarning("Mobile with ID {MobileId} not found", id);  
                return NotFound();
            }

            return mobile;
        }

        [HttpPost]
        public async Task<ActionResult<Mobile>> PostMobile(Mobile mobile)
        {
            _logger.LogInformation("Creating a new mobile: {MobileName}", mobile.Name);  
            _context.Mobiles.Add(mobile);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Mobile {MobileName} created successfully", mobile.Name);  
            return CreatedAtAction("GetMobile", new { id = mobile.Id }, mobile);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMobile(int id, Mobile mobile)
        {
            if (id != mobile.Id)
            {
                _logger.LogWarning("Mobile ID mismatch for ID {MobileId}", id);  
                return BadRequest();
            }

            _context.Entry(mobile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Mobile with ID {MobileId} updated successfully", mobile.Id);  
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MobileExists(id))
                {
                    _logger.LogWarning("Mobile with ID {MobileId} not found during update", id);  
                    return NotFound();
                }
                else
                {
                    _logger.LogError("Concurrency error occurred while updating mobile with ID {MobileId}", id);  
                    throw;
                }
            }

            return Ok(await _context.Mobiles.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMobile(int id)
        {
            _logger.LogInformation("Attempting to delete mobile with ID {MobileId}", id);  
            var mobile = await _context.Mobiles.FindAsync(id);
            if (mobile == null)
            {
                _logger.LogWarning("Mobile with ID {MobileId} not found for deletion", id);  
                return NotFound();
            }

            _context.Mobiles.Remove(mobile);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Mobile with ID {MobileId} deleted successfully", id);  
            return Ok(await _context.Mobiles.ToListAsync());
        }

        private bool MobileExists(int id)
        {
            return _context.Mobiles.Any(e => e.Id == id);
        }
    }
}
