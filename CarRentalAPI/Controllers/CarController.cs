using CarRentalAPI.Models;
using CarRentalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/cars")]
    [ApiController]
    [Authorize(Roles = "Employee, Admin")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<CarDto>> GetAllCars()
        {
            var carsDtos = _carService.GetAll();
            return Ok(carsDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<CarDto> GetCar([FromRoute] int id)
        {
            var carDto = _carService.GetById(id);
            return Ok(carDto);
        }

        [HttpPost]
        public ActionResult AddCar([FromBody] AddCarDto carDto)
        {
            int id = _carService.Add(carDto);
            return Created($"api/cars/{id}", null);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCar([FromRoute] int id, [FromBody] UpdateCarDto carDto)
        {
            _carService.UpdateById(id, carDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCar([FromRoute] int id)
        {
            _carService.DeleteById(id);
            return NoContent();
        }
    }
}
