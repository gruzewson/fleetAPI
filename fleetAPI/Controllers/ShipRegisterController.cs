using Microsoft.AspNetCore.Mvc;
using FleetAPI.Data;
using FleetAPI.Factories;
using FleetAPI.Models.Ships;
using FleetAPI.Models.Passengers;
using FleetAPI.Exceptions;
using FleetAPI.Models.Tanks;

namespace FleetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipRegisterController : ControllerBase
    {
        private readonly IShipRegister _register;
        private readonly IShipFactory<PassengerShip> _passengerShipFactory;
        private readonly IShipFactory<TankerShip> _tankerShipFactory;


        public ShipRegisterController(
            IShipRegister register,
            IShipFactory<PassengerShip> passShipFactory,
            IShipFactory<TankerShip> tankerShipFactory)
            {
                _register = register;
                _passengerShipFactory    = passShipFactory;
                _tankerShipFactory = tankerShipFactory;
            }

        [HttpPost("create")]
        public IActionResult Create([FromBody] ShipDto dto)
        {
            if (_register.Exists(dto.ImoNumber))
            {
                return Conflict($"Ship with IMO {dto.ImoNumber} already exists.");
            }

            try
            {
                Ship ship;
                switch (dto.ShipType)
                {
                    case ShipType.Passenger:
                        ship = _passengerShipFactory.Create(dto.ImoNumber, dto.Name, dto.Length, dto.Width, dto.Passengers, dto.Tanks);
                        break;
                    case ShipType.Tanker:
                        ship = _tankerShipFactory.Create(dto.ImoNumber, dto.Name, dto.Length, dto.Width, dto.Passengers, dto.Tanks);
                        break;
                    default:
                        return BadRequest($"Unsupported ship type: {dto.ShipType}");
                }

                _register.AddShip(ship);
                return CreatedAtAction(nameof(GetShipByImo), new { imo = ship.ImoNumber }, ship);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getShips")]
        public IActionResult GetAllShips()
        {
            var ships = _register.GetAllShips();
            return Ok(ships);
        }

        [HttpGet("getShip/{imo}")]
        public IActionResult GetShipByImo(string imo)
        {
            try
            {
                var ship = _register.GetShipByImo(imo);
                return Ok(ship);
            }
            catch (ShipNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("removeShip/{imo}")]
        public IActionResult RemoveShip(string imo)
        {
            try
            {
                _register.RemoveShip(imo);
                return NoContent();
            }
            catch (ShipNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("getShips/{type}")]
        public IActionResult GetShipsByType(ShipType type)
        {
            var ships = _register.GetShipsByType(type);

            if (!ships.Any())
                return NotFound($"No ships of type '{type}' found.");

            return Ok(ships);
        }
    }
}