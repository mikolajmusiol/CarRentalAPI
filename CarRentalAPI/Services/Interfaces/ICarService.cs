using CarRentalAPI.Models;

namespace CarRentalAPI.Services.Interfaces
{
    public interface ICarService
    {
        IEnumerable<CarDto> GetAll();
        CarDto GetById(int id);
        int Add(AddCarDto addCarDto);
        void UpdateById(int id, UpdateCarDto carDto);
        void DeleteById(int id);
    }
}
