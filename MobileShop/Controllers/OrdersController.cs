using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileShop.Auth;
using MobileShop.Models;

namespace MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateOrder(int mobileId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            

            
            if (user == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            
            var mobile = await _context.Mobiles.FindAsync(mobileId);
            if (mobile == null)
                return NotFound("Mobile not found");

            if (mobile.Stock < quantity)
                return BadRequest("Not enough stock available");

            
            var order = new Order
            {
                UserId = user.Id,
                MobileId = mobileId,
                Quantity = quantity,
                OrderDate = DateTime.UtcNow
            };

            _context.Orders.Add(order);

            
            mobile.Stock -= quantity;

            await _context.SaveChangesAsync();

            return Ok("Order created successfully");
        }

        
        [HttpGet("MyOrders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var user = await _userManager.GetUserAsync(User);

            var orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .Include(o => o.Mobile)
                .ToListAsync();

            return Ok(orders);
        }


        [HttpGet("AllOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Mobile)
                .ToListAsync();

            return Ok(orders);
        }
    }
}
