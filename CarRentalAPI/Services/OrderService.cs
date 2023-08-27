using AutoMapper;
using AutoMapper.Configuration.Conventions;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Models;
using CarRentalAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly CarRentalDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(CarRentalDbContext dbContext, IMapper mapper, ILogger<OrderService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<OrderDto> GetAllOrders()
        {
            var orders = _dbContext.Orders
                .Include(x => x.Client)
                .Include(x => x.Car)
                .ToList();

            return _mapper.Map<List<OrderDto>>(orders); 
        }

        public OrderDto GetOrderById(int id)
        {
            var order = GetOrder(id);

            return _mapper.Map<OrderDto>(order);
        }

        public int CreateOrder(CreateOrderDto createOrderDto)
        {
            
            var client = _dbContext.Clients.FirstOrDefault(c => c.Email == createOrderDto.Email);

            if (client is null)
            {
                _logger.LogError("Client not found");
                throw new NotFoundException("Client not found");
            }

            var car = _dbContext.Cars.FirstOrDefault(n => n.Brand == createOrderDto.Brand && n.Model == createOrderDto.Model);

            if (car is null)
            {
                _logger.LogError("Car not found");
                throw new NotFoundException("Car not found");
            }

            var order = _mapper.Map<Order>(createOrderDto);

            order.Car = car;
            order.Client = client;
            order.Value = car.Price * (int)(order.RentalTo - order.RentalFrom).TotalDays;

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            return order.Id;
        }

        public void UpdateById(int id, UpdateOrderDto updateOrderDto)
        {
            var order = GetOrder(id);

            if (updateOrderDto.Car != null)
            {
                var car = _dbContext.Cars.FirstOrDefault(c => c.Id == updateOrderDto.Car.Id);

                if (car is null)
                {   
                    _logger.LogError("Car not found");
                    throw new NotFoundException("Car not found");
                }

                order.Car = car ?? order.Car;
            }

            order.RentalFrom = updateOrderDto.RentalFrom ?? order.RentalFrom;
            order.RentalTo = updateOrderDto.RentalTo ?? order.RentalTo;
            order.Value = order.Car.Price * (int)(order.RentalTo - order.RentalFrom).TotalDays;

            _logger.LogInformation("Order updated");
            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var order = GetOrder(id);

            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();
        }

        private Order GetOrder(int id)
        {
            var order = _dbContext.Orders
                .Include(x => x.Client)
                .Include(x => x.Car)
                .FirstOrDefault(x => x.Id == id);

            if (order is null)
            {   
                _logger.LogError("Order not found");
                throw new NotFoundException("Order not found");
            }

            return order;
        }
    }
}