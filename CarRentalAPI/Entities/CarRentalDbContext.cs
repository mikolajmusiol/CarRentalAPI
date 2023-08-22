using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CarRentalAPI.Entities
{
    public class CarRentalDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .Property(c => c.Brand)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Car>()
                .Property(c => c.Model)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(o => o.ClientId)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .Property(c => c.City)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Address>()
                .Property(s => s.Street)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Client>()
                .Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Client>()
                .Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Client>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
