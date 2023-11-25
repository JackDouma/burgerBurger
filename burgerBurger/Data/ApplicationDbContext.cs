using burgerBurger.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace burgerBurger.Data
{
    public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
    {
        public TimeOnlyConverter()
            : base(timeOnly =>
                    timeOnly.ToTimeSpan(),
                timeSpan => TimeOnly.FromTimeSpan(timeSpan))
        { }
    }

    public class TimeOnlyComparer : ValueComparer<TimeOnly>
    {
        public TimeOnlyComparer() : base(
            (x, y) => x.Ticks == y.Ticks,
            timeOnly => timeOnly.GetHashCode())
        { }
    }

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

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.postalCode)
                .HasMaxLength(7)
                .IsRequired(false);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<TimeOnly>()
                .HaveConversion<TimeOnlyConverter, TimeOnlyComparer>();

            base.ConfigureConventions(configurationBuilder);
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
        public DbSet<GiftCard> GiftCards { get; set; }
    }
}