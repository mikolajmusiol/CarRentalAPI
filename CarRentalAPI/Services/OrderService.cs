﻿using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Models;
using CarRentalAPI.Services.Interfaces;
using CarRentalAPI.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly CarRentalDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderValueCalculator _orderValueCalculator;
        private readonly IUserContextService _userContextService;

        public OrderService(CarRentalDbContext dbContext, IMapper mapper, ILogger<OrderService> logger, IOrderValueCalculator orderValueCalculator, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _orderValueCalculator = orderValueCalculator;
            _userContextService = userContextService;
        }

        public List<OrderDto> GetAllOrders()
        {
            var orders = _dbContext.Orders
                .Include(x => x.CreatedBy)
                .Include(x => x.Car)
                .Include(x => x.Car.Price)
                .Where(x => x.CreatedById == _userContextService.GetUserId)
                .ToList();

            if (orders.Count == 0)
                throw new NotFoundException("No orders found");

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public OrderDto GetOrderById(int orderId)
        {
            var order = GetOrder(orderId);

            return _mapper.Map<OrderDto>(order);
        }

        public int CreateOrder(CreateOrderDto createOrderDto)
        {
            var user = _dbContext.Users.FirstOrDefault(c => c.Id == _userContextService.GetUserId);

            if (user is null)
            {
                _logger.LogError("Client not found");
                throw new NotFoundException("Client not found");
            }

            var car = _dbContext.Cars
                .Include(x => x.Price)
                .FirstOrDefault(n => n.Id == createOrderDto.CarId);

            if (car is null)
            {
                _logger.LogError("Car not found");
                throw new NotFoundException("Car not found");
            }

            var order = _mapper.Map<Order>(createOrderDto);

            order.CreatedBy = user;
            order.CreatedById = user.Id;

            order.Car = car;
            order.CarId = car.Id;

            order.Value = _orderValueCalculator.CalculateOrderValue(order, car.Price);

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            return order.Id;
        }

        public void UpdateById(int orderId, UpdateOrderDto updateOrderDto)
        {
            var order = GetOrder(orderId);

            if (updateOrderDto.CarId != null)
            {
                var car = GetCar(updateOrderDto.CarId.Value);
                order.Car = car ?? order.Car;
            }

            order.RentalFrom = updateOrderDto.RentalFrom ?? order.RentalFrom;
            order.RentalTo = updateOrderDto.RentalTo ?? order.RentalTo;

            order.Value = _orderValueCalculator.CalculateOrderValue(order, order.Car.Price);

            _logger.LogInformation("Order updated");
            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();
        }

        public void DeleteById(int orderId)
        {
            var order = GetOrder(orderId);

            if (DateTime.Now > order.RentalFrom)
            {
                throw new BadRequestException("You cannot delete an order that has already started");
            }

            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();
        }

        private Order GetOrder(int orderId)
        {
            var order = _dbContext.Orders
                .Include(x => x.CreatedBy)
                .Include(x => x.Car)
                .Include(x => x.Car.Price)
                .FirstOrDefault(x => x.Id == orderId && x.CreatedById == _userContextService.GetUserId);

            if (order is null)
            {
                _logger.LogError("Order not found");
                throw new NotFoundException("Order not found");
            }

            return order;
        }

        private Car GetCar(int carId)
        {
            var car = _dbContext.Cars
               .Include(x => x.Price)
               .FirstOrDefault(c => c.Id == carId);

            if (car is null)
            {
                _logger.LogError("Car not found");
                throw new NotFoundException("Car not found");
            }

            return car;
        }
    }
}