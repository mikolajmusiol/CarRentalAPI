using CarRentalAPI.Models;
using CarRentalAPI.Models.Dtos;

namespace CarRentalAPI.Services.Interfaces
{
    public interface ICarService
    {
        Task<PagedResult<CarDto>> GetAll(QueryModel query);
        Task<CarDto> GetById(int id);
        Task<int> Add(AddCarDto addCarDto);
        Task UpdateById(int id, UpdateCarDto carDto);
        Task DeleteById(int id);
    }
}
