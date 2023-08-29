using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Models;

namespace CarRentalAPI.MappingProfiles
{
    public class CarRentalMappingProfile : Profile
    {
        public CarRentalMappingProfile()
        {
            CreateMap<Client, ClientDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, m => m.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, m => m.MapFrom(s => s.Address.PostalCode))
                .ForMember(m => m.Orders, m => m.MapFrom(s => s.Orders));

            CreateMap<Car, CarDto>();

            CreateMap<Order, OrderDto>()
                .ForMember(m => m.Car, c => c.MapFrom(s => s.Car))
                .ForMember(m => m.Email, c => c.MapFrom(s => s.Client.Email))
                .ForMember(m => m.FirstName, c => c.MapFrom(s => s.Client.FirstName))
                .ForMember(m => m.LastName, c => c.MapFrom(s => s.Client.LastName));

            CreateMap<AddCarDto, Car>();

            CreateMap<UpdateCarDto, Car>();

            CreateMap<CreateOrderDto, Order>();

            CreateMap<UpdateOrderDto,  Order>();
        }
    }
}
