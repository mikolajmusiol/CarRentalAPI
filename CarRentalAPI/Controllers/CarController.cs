using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Models;
using CarRentalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/cars")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public ActionResult<List<CarDto>> GetAllCars() 
        {
            var carsDtos = _carService.GetAll();
            return Ok(carsDtos);
        }
    }
}
