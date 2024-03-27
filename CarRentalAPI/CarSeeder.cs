using CarRentalAPI.Entities;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI
{
    public class CarSeeder
    {
        private readonly CarRentalDbContext _dbContext;

        public CarSeeder(CarRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();

                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!await _dbContext.Cars.AnyAsync())
                {
                    var cars = GetCars();
                    await _dbContext.Cars.AddRangeAsync(cars);
                    await _dbContext.SaveChangesAsync();
                }

                if (!await _dbContext.Roles.AnyAsync())
                {
                    var roles = GetRoles();
                    await _dbContext.Roles.AddRangeAsync(roles);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<Car> GetCars()
        {
            var cars = new List<Car>()
            {
                new Car()
                {
                    Brand = "Skoda",
                    Model = "Fabia I",
                    YearOfProduction = 2003,
                    Color = "Red",
                    HorsePower = 60,
                    Description = null,
                    Price = new Price()
                    {
                        PriceForAnHour = 60,
                        PriceForADay = 500,
                        PriceForAWeek = 3000
                    }
                }
            };

            return cars;
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role() { Name = "Tenant" },
                new Role() { Name = "Employee"},
                new Role() { Name = "Admin"}
            };

            return roles;
        }

    }
}
