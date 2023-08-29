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

        public IEnumerable<CarDto> GetAll()
        {
            var cars = _dbContext.Cars
                .Include(x => x.Price)
                .ToList();

            return _mapper.Map<List<CarDto>>(cars); ;
        }

        public CarDto GetById(int id)
        {
            var car = GetCar(id);

            return _mapper.Map<CarDto>(car);
        }

        public int Add(AddCarDto addCarDto)
        {
            var car = _mapper.Map<Car>(addCarDto);
            _dbContext.Cars.Add(car);
            _dbContext.SaveChanges();
            return car.Id;
        }

        public void UpdateById(int id, UpdateCarDto carDto)
        {
            var car = GetCar(id);

            car.Mileage = carDto.Mileage ?? car.Mileage;
            car.Color = carDto.Color ?? car.Color;
            car.HorsePower = carDto.HorsePower ?? car.HorsePower;
            car.Description = carDto.Description ?? car.Description;

            car.Price.PriceForAnHour = carDto.Price.PriceForAnHour ?? car.Price.PriceForAnHour;
            car.Price.PriceForADay = carDto.Price.PriceForADay ?? car.Price.PriceForADay;
            car.Price.PriceForAWeek = carDto.Price.PriceForAWeek ?? car.Price.PriceForAWeek;

            _dbContext.Cars.Update(car);
            _dbContext.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var car = GetCar(id);
            _dbContext.Cars.Remove(car);
            _dbContext.SaveChanges();
        }

        private Car GetCar(int id)
        {
            var car = _dbContext.Cars
                .Include(x => x.Price)
                .FirstOrDefault(c => c.Id == id);

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
