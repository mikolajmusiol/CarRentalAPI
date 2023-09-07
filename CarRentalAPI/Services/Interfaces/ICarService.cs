using CarRentalAPI.Models;

namespace CarRentalAPI.Services.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<CarDto>> GetAll();
        Task<CarDto> GetById(int id);
        Task<int> Add(AddCarDto addCarDto);
        Task UpdateById(int id, UpdateCarDto carDto);
        Task DeleteById(int id);
    }
}
