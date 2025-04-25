using Microsoft.AspNetCore.Mvc;
using FleetAPI.Data;
using FleetAPI.Factories;
using FleetAPI.Models.Ships;
using FleetAPI.Models.Passengers;
using FleetAPI.Exceptions;

namespace FleetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipRepositoryController : ControllerBase
    {
        private readonly IShipRepository _repository;
        private readonly IShipFactory<PassengerShip> _passShipFactory;
        // TODO pasship or tankership...


        public ShipRepositoryController(
            IShipRepository repository,
            IShipFactory<PassengerShip> passShipFactory)
        {
            _repository = repository;
            _passShipFactory    = passShipFactory;
        }

        [HttpPost]
        public IActionResult Create([FromBody] ShipDto dto)
        {
            if (_repository.Exists(dto.ImoNumber))
            {
                return Conflict($"Ship with IMO {dto.ImoNumber} already exists.");
            }

            var ship = _passShipFactory.Create(
                dto.ImoNumber,
                dto.Name,
                dto.Length,
                dto.Width,
                dto.Passengers ?? new List<Passenger>(),
                dto.Tanks // assuming ShipDto supports both types
            );

            _repository.AddShip(ship);

            return CreatedAtAction(nameof(Create), new { id = dto.ImoNumber }, ship);
        }

        [HttpGet]
        public IActionResult GetAllShips()
        {
            var ships = _repository.GetAllShips();
            return Ok(ships);
        }

        [HttpGet("{imo}")]
        public IActionResult GetShipByImo(string imo)
        {
            try
            {
                var ship = _repository.GetShipByImo(imo);
                return Ok(ship);
            }
            catch (ShipNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{imo}")]
        public IActionResult RemoveShip(string imo)
        {
            try
            {
                _repository.RemoveShip(imo);
                return NoContent();
            }
            catch (ShipNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("type/{type}")]
        public IActionResult GetShipsByType(ShipType type)
        {
            var ships = _repository.GetShipsByType(type);

            if (!ships.Any())
                return NotFound($"No ships of type '{type}' found.");

            return Ok(ships);
        }
    }
}