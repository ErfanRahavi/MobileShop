using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileShop.Auth;

namespace MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpPost("Increase/{id}")]
        public async Task<IActionResult> IncreaseStock(int id, [FromBody] int amount)
        {
            var mobile = await _context.Mobiles.FindAsync(id);
            if (mobile == null)
            {
                return NotFound(); 
            }

            mobile.Stock += amount;
            await _context.SaveChangesAsync();

            return Ok(mobile); 
        }

        
        [HttpPost("Decrease/{id}")]
        public async Task<IActionResult> DecreaseStock(int id, [FromBody] int amount)
        {
            var mobile = await _context.Mobiles.FindAsync(id);
            if (mobile == null)
            {
                return NotFound(); 
            }

            if (mobile.Stock < amount)
            {
                return BadRequest("Not enough stock available"); 
            }

            mobile.Stock -= amount;
            await _context.SaveChangesAsync();

            return Ok(mobile); 
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetStock(int id)
        {
            var mobile = await _context.Mobiles.FindAsync(id);
            if (mobile == null)
            {
                return NotFound(); 
            }

            return Ok(mobile.Stock); 
        }
    }
}
