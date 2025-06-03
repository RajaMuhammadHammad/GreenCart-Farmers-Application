using GreenCart.API.Models;
using Microsoft.EntityFrameworkCore;

public class GreenCartDbContext : DbContext
{
    public GreenCartDbContext(DbContextOptions<GreenCartDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items) // Order has many OrderItems
            .HasForeignKey(oi => oi.OrderId);
    }
}
