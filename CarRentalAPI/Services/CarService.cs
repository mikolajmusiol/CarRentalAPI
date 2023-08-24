﻿using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Models;
using CarRentalAPI.Services.Interfaces;

namespace CarRentalAPI.Services
{
    public class CarService : ICarService
    {
        private readonly CarRentalDbContext _dbContext;
        private readonly IMapper _mapper;

        public CarService(CarRentalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<CarDto> GetAll()
        {
            var cars = _dbContext.Cars.ToList();
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
            car.Price = carDto.Price ?? car.Price;

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
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == id);

            if (car is null)
            {
                throw new NotFoundException("Car not found");
            }
            else
            {
                return car;
            }
        }
    }
}
