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
        public async Task<ActionResult<List<CarDto>>> GetAllCars()
        {
            var carsDtos = await _carService.GetAll();
            return Ok(carsDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CarDto>> GetCar([FromRoute] int id)
        {
            var carDto = await _carService.GetById(id);
            return Ok(carDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddCar([FromBody] AddCarDto carDto)
        {
            int id = await _carService.Add(carDto);
            return Created($"api/cars/{id}", null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCar([FromRoute] int id, [FromBody] UpdateCarDto carDto)
        {
            await _carService.UpdateById(id, carDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCar([FromRoute] int id)
        {
            await _carService.DeleteById(id);
            return NoContent();
        }
    }
}