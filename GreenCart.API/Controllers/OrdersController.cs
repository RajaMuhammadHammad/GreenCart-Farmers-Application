using GreenCart.API.Models;
using GreenCart.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GreenCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly GreenCartDbContext _context;

        public OrdersController(GreenCartDbContext context)
        {
            _context = context;
        }

        // 🔒 Buyers: Place Order
        [HttpPost]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                Items = new List<OrderItem>()
            };

            foreach (var item in dto.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null || product.StockQuantity < item.Quantity)
                    return BadRequest("Invalid product or insufficient stock");

                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                });

                product.StockQuantity -= item.Quantity; // Update stock
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Order placed successfully", OrderId = order.OrderId });
        }

        // 👤 Buyers: View Own Orders
        [HttpGet("my")]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .Select(o => new OrderResponseDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    Items = o.Items.Select(i => new OrderItemDetailDto
                    {
                        ProductName = i.Product.ProductName,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList()
                })
                .ToListAsync();

            return Ok(orders);
        }

        // 👨‍🌾 Farmers: View Orders for their Products
        [HttpGet("farmer")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> GetOrdersForFarmer()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            var products = await _context.Products
                .Where(p => p.CreatedBy == userName)
                .Select(p => p.ProductId)
                .ToListAsync();

            var orders = await _context.OrderItems
                .Where(oi => products.Contains(oi.ProductId))
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Select(oi => new
                {
                    OrderId = oi.OrderId,
                    ProductName = oi.Product.ProductName,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    BuyerId = oi.Order.UserId,
                    OrderDate = oi.Order.OrderDate,
                    Status = oi.Order.Status
                })
                .ToListAsync();

            return Ok(orders);
        }
    }
}
