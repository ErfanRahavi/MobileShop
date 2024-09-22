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
    [Authorize]
    public class MobileSearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MobileSearchController> _logger;  

        public MobileSearchController(ApplicationDbContext context, ILogger<MobileSearchController> logger)  
        {
            _context = context;
            _logger = logger;  
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mobile>>> GetMobiles(
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int? year,
            [FromQuery] string? brand)
        {
            _logger.LogInformation("Search request received with parameters - MinPrice: {MinPrice}, MaxPrice: {MaxPrice}, Year: {Year}, Brand: {Brand}",
                minPrice, maxPrice, year, brand);  

            var query = _context.Mobiles.AsQueryable();

            if (minPrice.HasValue)
            {
                _logger.LogInformation("Filtering mobiles with minimum price of {MinPrice}", minPrice);  
                query = query.Where(m => m.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                _logger.LogInformation("Filtering mobiles with maximum price of {MaxPrice}", maxPrice);  
                query = query.Where(m => m.Price <= maxPrice.Value);
            }

            if (year.HasValue)
            {
                _logger.LogInformation("Filtering mobiles with year {Year}", year);  
                query = query.Where(m => m.Year == year.Value);
            }

            if (!string.IsNullOrEmpty(brand))
            {
                _logger.LogInformation("Filtering mobiles by brand {Brand}", brand);  
                query = query.Where(m => m.Brand.ToLower().Contains(brand.ToLower()));
            }

            var mobiles = await query.ToListAsync();

            if (!mobiles.Any())
            {
                _logger.LogWarning("No mobiles found with the given search criteria");  
            }

            _logger.LogInformation("{Count} mobiles found matching the search criteria", mobiles.Count);  

            return Ok(mobiles);
        }
    }
}
