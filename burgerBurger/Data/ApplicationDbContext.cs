using burgerBurger.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace burgerBurger.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.balance)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0.00m)
                .IsRequired(false);


            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.locationIdentifier)
                .IsRequired(false); 

            
            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.address)
                .HasMaxLength(250)
                .IsRequired(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.city)
                .HasMaxLength(100) 
                .IsRequired(false); 

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.province)
                .HasMaxLength(100) 
                .IsRequired(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.country)
                .HasMaxLength(100) 
                .IsRequired(false); 
        }


        public DbSet<Location> Location { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<StaticItem> StaticItem { get; set; }
        public DbSet<ItemInventory> ItemInventory { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<InventoryOutline> InventoryOutline { get; set; }
        public DbSet<CustomItem>? CustomItem { get; set; }
    }
}