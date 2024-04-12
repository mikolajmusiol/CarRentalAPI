using CarRentalAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.IntegrationTests.Helpers
{
    public class Seeder
    {
        private readonly CarRentalDbContext _dbContext;

        public Seeder(CarRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<int> SeedCar()
        {
            if (!await _dbContext.Database.CanConnectAsync()) { throw new Exception("Can't connect to database"); }

            var car = GetCar();
            await _dbContext.Cars.AddAsync(car);
            await _dbContext.SaveChangesAsync();

            return car.Id;
        }

        public async Task<int> SeedOrder()
        {
            if (!await _dbContext.Database.CanConnectAsync()) { throw new Exception("Can't connect to database"); }

            var order = GetOrder();
            
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            

            return order.Id;
        }

        public async Task<int> SeedUser()
        {
            if (!await _dbContext.Database.CanConnectAsync()) { throw new Exception("Can't connect to database"); }

            var user = GetUser();

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user.Id;
        }

        private Car GetCar()
        {
            var car = new Car()
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
            };

            return car;
        }

        private User GetUser()
        {
            var user = new User()
            {
                Email = "test@test.pl",
                FirstName = "Test",
                LastName = "Test",
                PasswordHash = "hash"
            };

            return user;
        }

        private Order GetOrder()
        {
            var order = new Order()
            {
                Value = 1,
                RentalFrom = DateTime.Now,
                RentalTo = DateTime.Now.AddDays(1),
                CreatedById = 1,
                CarId = 1,
                Car = GetCar(),
                CreatedBy = GetUser()
            };

            return order;
        }

    }
}