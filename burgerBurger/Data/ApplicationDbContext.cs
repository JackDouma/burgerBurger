using burgerBurger.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace burgerBurger.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Location> Location { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<StaticItem> StaticItem { get; set; }
        public DbSet<ItemInventory> ItemInventory { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}