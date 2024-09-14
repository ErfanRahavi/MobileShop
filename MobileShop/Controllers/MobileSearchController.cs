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
    [Authorize]
    public class MobileSearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MobileSearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mobile>>> GetMobiles(
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int? year,
            [FromQuery] string? brand)
        {
            var query = _context.Mobiles.AsQueryable();

            if (minPrice.HasValue)
            {
                query = query.Where(m => m.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(m => m.Price <= maxPrice.Value);
            }

            if (year.HasValue)
            {
                query = query.Where(m => m.Year == year.Value);
            }

            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(m => m.Brand.ToLower().Contains(brand.ToLower()));
            }

            var mobiles = await query.ToListAsync();

            return Ok(mobiles);
        }
    }
}
