using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Dtos;
using CarRentalAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<PagedResult<CarDto>> GetAll(QueryModel query)
        {
            var baseQuery = _dbContext.Cars
                .Include(x => x.Price)
                .Where(x => query.SearchPhrase == null
                || x.Brand.ToLower().Contains(query.SearchPhrase.ToLower())
                || x.Model.ToLower().Contains(query.SearchPhrase.ToLower()));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelector = new Dictionary<string, Expression<Func<Car, object>>>()
                {
                    {nameof(Car.Brand), x => x.Brand },
                    {nameof(Car.Model), x => x.Model },
                    {nameof(Car.Price.PriceForAnHour), x => x.Price.PriceForAnHour.GetValueOrDefault() },
                    {nameof(Car.Price.PriceForADay), x => x.Price.PriceForADay.GetValueOrDefault() },
                    {nameof(Car.Price.PriceForAWeek), x => x.Price.PriceForAWeek.GetValueOrDefault() }
                };

                var selectedColumn = columnSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.Ascending
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var cars = await baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync();

            var carsDtos = _mapper.Map<List<CarDto>>(cars);

            var pagedResult = new PagedResult<CarDto>(carsDtos, baseQuery.Count(), query.PageSize, query.PageNumber);

            return pagedResult; 
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

            _dbContext.Cars.Update(car);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var car = await GetCar(id);
            _dbContext.Cars.Remove(car);
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
