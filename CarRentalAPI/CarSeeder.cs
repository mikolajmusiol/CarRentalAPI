using CarRentalAPI.Entities;
using Humanizer;

namespace CarRentalAPI
{
    public class CarSeeder
    {
        private readonly CarRentalDbContext _dbContext;

        public CarSeeder(CarRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Cars.Any())
                {
                    var cars = GetCars();
                    _dbContext.Cars.AddRange(cars);
                    _dbContext.SaveChanges();
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
