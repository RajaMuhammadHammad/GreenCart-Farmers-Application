using GreenCart.API;
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
    public class ProductController : ControllerBase
    {
        private readonly GreenCartDbContext _context;

        public ProductController(GreenCartDbContext context)
        {
            _context = context;
        }

        // ✅ Buyers and Farmers: View all products
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // ✅ Farmers: Add product
        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> AddProduct(ProductDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);

            var product = new Product
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = username
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        // ✅ Farmers: Update own product
        [HttpPut("{id}")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id && p.CreatedBy == username);

            if (product == null)
                return NotFound("Product not found or access denied.");

            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(product);
        }

        // ✅ Farmers: Delete own product
        [HttpDelete("{id}")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id && p.CreatedBy == username);

            if (product == null)
                return NotFound("Product not found or access denied.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok("Product deleted.");
        }

        // ✅ Buyers and Farmers: Get single product
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found.");
            return Ok(product);
        }
    }
}
