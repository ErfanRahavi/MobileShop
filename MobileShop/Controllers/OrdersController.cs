using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileShop.Auth;
using MobileShop.Models;
using System.Security.Claims;

namespace MobileShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<OrdersController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(int mobileId, int quantity)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", userId);
                return Unauthorized("User not found in the database.");
            }

            var mobile = await _context.Mobiles.FindAsync(mobileId);
            if (mobile == null)
            {
                _logger.LogWarning("Mobile with ID {MobileId} not found", mobileId);
                return NotFound("Mobile not found.");
            }

            if (mobile.Stock < quantity)
            {
                _logger.LogWarning("Not enough stock for Mobile ID {MobileId}. Requested: {Quantity}, Available: {Stock}", mobileId, quantity, mobile.Stock);
                return BadRequest("Not enough stock available.");
            }

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

            _logger.LogInformation("Order created for User ID {UserId}, Mobile ID {MobileId}, Quantity {Quantity}", userId, mobileId, quantity);
            return Ok("Order created successfully.");
        }

        [HttpGet("MyOrders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Mobile)
                .ToListAsync();

            _logger.LogInformation("Fetched orders for User ID {UserId}", userId);
            return Ok(orders);
        }

        [HttpGet("AllOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Mobile)
                .ToListAsync();

            _logger.LogInformation("Fetched all orders");
            return Ok(orders);
        }
    }
}
