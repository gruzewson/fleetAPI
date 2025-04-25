using Microsoft.AspNetCore.Mvc;
using FleetAPI.Data;
using FleetAPI.Models.Passengers;
using FleetAPI.Exceptions;

namespace FleetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassengerShipController : ControllerBase
    {
        private readonly IShipRegister _register;

        public PassengerShipController(
            IShipRegister register)
        {
            _register = register;
        }

        [HttpPost("{imo}/passengers/add")]
        public IActionResult AddPassenger(string imo, [FromBody] PassengerDto passengerDto)
        {
            var ship = _register.GetPassengerShipByImo(imo);
            if (ship == null)
            {
                return NotFound();
            }

            try
            {
                ship.AddPassenger(passengerDto.Name, passengerDto.Surname);
            }
            catch (InvalidPassengerDataException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{imo}/passengers/remove/{passengerId}")]
        public IActionResult RemovePassenger(string imo, [FromBody] Guid passengerId)
        {
            var ship = _register.GetPassengerShipByImo(imo);
            if (ship == null)
            {
                return NotFound();
            }

            ship.RemovePassengerById(passengerId);

            return NoContent();
        }

        [HttpPost("{imo}/passengers/update/{passengerId}")]
        public IActionResult UpdatePassengerInfo(string imo, Guid passengerId, [FromBody] PassengerDto dto)
        {
            var ship = _register.GetPassengerShipByImo(imo);
            if (ship == null)
            {
                return NotFound();
            }

            try
            {
                ship.UpdatePassengerInfo(passengerId, dto.Name, dto.Surname);
            }
            catch (InvalidPassengerDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PassengerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }


        [HttpGet("{imo}/passengers/get/{passengerId}")]
        public IActionResult GetPassengerById(string imo, Guid passengerId)
        {
            var ship = _register.GetPassengerShipByImo(imo);
            if (ship == null)
            {
                return NotFound();
            }

            Passenger passenger;

            try
            {
                passenger = ship.GetPassengerById(passengerId);
            }
            catch (PassengerNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(passenger);
        }

        [HttpGet("{imo}/passengers/getAll")]
        public IActionResult GetAllPassengers(string imo)
        {
            var ship = _register.GetPassengerShipByImo(imo);
            if (ship == null)
            {
                return NotFound();
            }

            var passengers = ship.GetAllPassengers();
            return Ok(passengers);
        }
    }
}
