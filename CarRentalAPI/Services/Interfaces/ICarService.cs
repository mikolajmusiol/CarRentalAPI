using CarRentalAPI.Models;

namespace CarRentalAPI.Services.Interfaces
{
    public interface ICarService
    {
        IEnumerable<CarDto> GetAll();
    }
}
