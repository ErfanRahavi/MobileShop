using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileShop.Auth;
using Serilog;  

namespace MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StockController> _logger;  

        public StockController(ApplicationDbContext context, ILogger<StockController> logger)  
        {
            _context = context;
            _logger = logger;  
        }

        [HttpPost("Increase/{id}")]
        public async Task<IActionResult> IncreaseStock(int id, [FromBody] int amount)
        {
            var mobile = await _context.Mobiles.FindAsync(id);
            if (mobile == null)
            {
                _logger.LogWarning("Mobile with ID {Id} not found for stock increase", id);  
                return NotFound();
            }

            _logger.LogInformation("Increasing stock for Mobile ID {Id} by {Amount}", id, amount);  
            mobile.Stock += amount;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Stock successfully increased. New stock for Mobile ID {Id} is {Stock}", id, mobile.Stock);  
            return Ok(mobile);
        }

        [HttpPost("Decrease/{id}")]
        public async Task<IActionResult> DecreaseStock(int id, [FromBody] int amount)
        {
            var mobile = await _context.Mobiles.FindAsync(id);
            if (mobile == null)
            {
                _logger.LogWarning("Mobile with ID {Id} not found for stock decrease", id);  
                return NotFound();
            }

            if (mobile.Stock < amount)
            {
                _logger.LogWarning("Not enough stock to decrease for Mobile ID {Id}. Requested: {Amount}, Available: {Stock}", id, amount, mobile.Stock);  
                return BadRequest("Not enough stock available");
            }

            _logger.LogInformation("Decreasing stock for Mobile ID {Id} by {Amount}", id, amount);  
            mobile.Stock -= amount;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Stock successfully decreased. New stock for Mobile ID {Id} is {Stock}", id, mobile.Stock);  
            return Ok(mobile);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetStock(int id)
        {
            var mobile = await _context.Mobiles.FindAsync(id);
            if (mobile == null)
            {
                _logger.LogWarning("Mobile with ID {Id} not found when fetching stock", id);  
                return NotFound();
            }

            _logger.LogInformation("Fetching stock for Mobile ID {Id}. Current stock is {Stock}", id, mobile.Stock);  
            return Ok(mobile.Stock);
        }
    }
}
