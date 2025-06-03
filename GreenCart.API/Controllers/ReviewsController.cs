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
    public class ReviewsController : ControllerBase
    {
        private readonly GreenCartDbContext _context;

        public ReviewsController(GreenCartDbContext context)
        {
            _context = context;
        }

        // ✅ Buyer: Add review
        [HttpPost]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> AddReview(ReviewDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null) return Unauthorized();

            var review = new Review
            {
                ProductId = dto.ProductId,
                UserId = user.UserId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok(review);
        }

        // ✅ Farmer: View reviews for a product
        [HttpGet("{productId}")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> GetReviews(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null) return NotFound();

            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            return Ok(reviews);
        }
    }
}
