using Microsoft.AspNetCore.Mvc;
using FleetAPI.Data;
using FleetAPI.Factories;
using FleetAPI.Models;
using FleetAPI.Models.Ships;
using FleetAPI.Models.Passengers;
using FleetAPI.Exceptions;

namespace FleetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   public class PassengerShipController : ControllerBase
    {
        private readonly IShipRepository _repository;
        private readonly IShipFactory<PassengerShip> _factory;

        public PassengerShipController(
            IShipRepository repository,
            IShipFactory<PassengerShip> factory)
        {
            _repository = repository;
            _factory    = factory;
        }

        [HttpPost("{imo}/passengers/add")]
        public IActionResult AddPassenger(string imo, [FromBody] PassengerDto passengerDto)
        {
            var ship = _repository.GetPassengerShipByImo(imo);
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

        [HttpDelete("{imo}/passengers/remove")]
        public IActionResult RemovePassenger(string imo, [FromBody] int passengerId)
        {
            var ship = _repository.GetPassengerShipByImo(imo);
            if (ship == null)
            {
                return NotFound();
            }
            ship.RemovePassengerById(passengerId);
            // try
            // {
            //     ship.RemovePassengerById(passengerId);
            // }
            // catch (InvalidPassengerDataException ex)
            // {
            //     return BadRequest(ex.Message);
            // }

            return NoContent();
        }

        [HttpPost("{imo}/passengers/update/{passengerId}")]
        public IActionResult UpdatePassengerInfo(string imo, int passengerId, [FromBody] PassengerDto dto)
        {
            var ship = _repository.GetPassengerShipByImo(imo);
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


        [HttpGet("{imo}/passengers/{passengerId}")]
        public IActionResult GetPassengerById(string imo, int passengerId)
        {
            var ship = _repository.GetPassengerShipByImo(imo);
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
    }
}
