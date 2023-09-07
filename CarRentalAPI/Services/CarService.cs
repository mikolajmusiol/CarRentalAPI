using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Models;
using CarRentalAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Services
{
    public class CarService : ICarService
    {
        private readonly CarRentalDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CarService> _logger;

        public CarService(CarRentalDbContext dbContext, IMapper mapper, ILogger<CarService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CarDto>> GetAll()
        {
            var cars = await _dbContext.Cars
                .Include(x => x.Price)
                .ToListAsync();

            return _mapper.Map<List<CarDto>>(cars); ;
        }

        public async Task<CarDto> GetById(int id)
        {
            var car = await GetCar(id);

            return _mapper.Map<CarDto>(car);
        }

        public async Task<int> Add(AddCarDto addCarDto)
        {
            var car = _mapper.Map<Car>(addCarDto);
            await _dbContext.Cars.AddAsync(car);
            await _dbContext.SaveChangesAsync();
            return car.Id;
        }

        public async Task UpdateById(int id, UpdateCarDto carDto)
        {
            var car = await GetCar(id);

            car.Mileage = carDto.Mileage ?? car.Mileage;
            car.Color = carDto.Color ?? car.Color;
            car.HorsePower = carDto.HorsePower ?? car.HorsePower;
            car.Description = carDto.Description ?? car.Description;

            if (carDto.Price != null)
            {
                car.Price.PriceForAnHour = carDto.Price.PriceForAnHour ?? car.Price.PriceForAnHour;
                car.Price.PriceForADay = carDto.Price.PriceForADay ?? car.Price.PriceForADay;
                car.Price.PriceForAWeek = carDto.Price.PriceForAWeek ?? car.Price.PriceForAWeek;
            }

            await Task.Run(() => _dbContext.Cars.Update(car));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var car = await GetCar(id);
            await Task.Run(() => _dbContext.Cars.Remove(car));
            await _dbContext.SaveChangesAsync();
        }

        private async Task<Car> GetCar(int id)
        {
            var car = await _dbContext.Cars
                .Include(x => x.Price)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car is null)
            {
                _logger.LogError("Car not found");
                throw new NotFoundException("Car not found");
            }
            else
            {
                return car;
            }
        }
    }
}
