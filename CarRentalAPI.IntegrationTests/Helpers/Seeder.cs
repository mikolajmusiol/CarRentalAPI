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


        public async Task SeedCar(Car car)
        {
            if (!await _dbContext.Database.CanConnectAsync()) { throw new Exception("Can't connect to database"); }

            await _dbContext.Cars.AddAsync(car);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedOrder(Order order)
        {
            if (!await _dbContext.Database.CanConnectAsync()) { throw new Exception("Can't connect to database"); }

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedUser(User user)
        {
            if (!await _dbContext.Database.CanConnectAsync()) { throw new Exception("Can't connect to database"); }

            if (!await _dbContext.Users.AnyAsync())
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public Car GetCar()
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

        public Order GetOrder()
        {
            var order = new Order()
            {
                Value = 1,
                RentalFrom = DateTime.Now,
                RentalTo = DateTime.Now.AddDays(1),
                Car = GetCar(),
                CreatedBy = GetUser()
            };

            return order;
        }

        public User GetUser()
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
    }
}