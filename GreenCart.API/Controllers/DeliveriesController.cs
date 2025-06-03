using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenCart.API.Models;
using GreenCart.API.Models.DTOs;
using System.Threading.Tasks;
using System.Security.Claims;

namespace GreenCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveriesController : ControllerBase
    {
        private readonly GreenCartDbContext _context;

        public DeliveriesController(GreenCartDbContext context)
        {
            _context = context;
        }

        // GET: api/deliveries/{orderId}
        [HttpGet("{orderId}")]
        public async Task<ActionResult<DeliveryDTO>> GetDeliveryStatus(int orderId)
        {
            var delivery = await _context.Deliveries
                .Where(d => d.OrderId == orderId)
                .FirstOrDefaultAsync();

            if (delivery == null)
                return NotFound();

            var deliveryDTO = new DeliveryDTO
            {
                DeliveryId = delivery.DeliveryId,
                OrderId = delivery.OrderId,
                DeliveryDate = delivery.DeliveryDate,  // Nullable DateTime
                DeliveryStatus = delivery.DeliveryStatus
            };

            return Ok(deliveryDTO);
        }

        // POST: api/deliveries
        [HttpPost]
        public async Task<ActionResult<DeliveryDTO>> CreateDelivery(CreateUpdateDeliveryDTO model)
        {
            // Ensure the OrderId exists in the Orders table
            var orderExists = await _context.Orders.AnyAsync(o => o.OrderId == model.OrderId);
            if (!orderExists)
                return BadRequest("Order not found.");

            var delivery = new Delivery
            {
                OrderId = model.OrderId,
                DeliveryDate = model.DeliveryDate,  // Nullable DateTime
                DeliveryStatus = model.DeliveryStatus
            };

            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();

            var deliveryDTO = new DeliveryDTO
            {
                DeliveryId = delivery.DeliveryId,
                OrderId = delivery.OrderId,
                DeliveryDate = delivery.DeliveryDate,  // Nullable DateTime
                DeliveryStatus = delivery.DeliveryStatus
            };

            return CreatedAtAction(nameof(GetDeliveryStatus), new { orderId = delivery.OrderId }, deliveryDTO);
        }

        // PUT: api/deliveries/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDelivery(int id, CreateUpdateDeliveryDTO model)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
                return NotFound();

            // Ensure the OrderId exists in the Orders table
            var orderExists = await _context.Orders.AnyAsync(o => o.OrderId == model.OrderId);
            if (!orderExists)
                return BadRequest("Order not found.");

            delivery.DeliveryDate = model.DeliveryDate;  // Nullable DateTime
            delivery.DeliveryStatus = model.DeliveryStatus;

            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();

            return NoContent();  // Successful update, no content returned
        }
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<DeliveryDTO>>> GetMyDeliveries()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int farmerId = int.Parse(userIdClaim.Value);

            // Get order IDs where at least one OrderItem belongs to a Product owned by this farmer
            var myOrderIds = await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.Product.FarmerId == farmerId)
                .Select(oi => oi.OrderId)
                .Distinct()
                .ToListAsync();

            var deliveries = await _context.Deliveries
                .Where(d => myOrderIds.Contains(d.OrderId))
                .Select(d => new DeliveryDTO
                {
                    DeliveryId = d.DeliveryId,
                    OrderId = d.OrderId,
                    DeliveryDate = d.DeliveryDate,
                    DeliveryStatus = d.DeliveryStatus
                })
                .ToListAsync();

            return Ok(deliveries);
        }


    }
}
