using CarRentalAPI.Entities;

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
                    Price = 3000
                }
            };

            return cars;
        }

    }
}
