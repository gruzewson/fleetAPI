using Microsoft.AspNetCore.Mvc;
using FleetAPI.Data;
using FleetAPI.Exceptions;

namespace FleetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TankerShipController : ControllerBase
    {
        private readonly IShipRegister _register;

        public TankerShipController(
            IShipRegister register)
        {
            _register = register;
        }

        [HttpPost("{imo}/tanks/fill/{tankId}")]
        public IActionResult FillTank(string imo, [FromBody] Guid tankId, double liters)
        {
            var ship = _register.GetTankerShipByImo(imo);
            if (ship == null)
            {
                return NotFound();
            }

            try
            {
                ship.FillTank(tankId, liters);
            }
            catch (TankDoesntExistException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TankOverfillException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost("{imo}/tanks/empty/{tankId}")]
        public IActionResult EmptyTank(string imo, [FromBody] Guid tankId)
        {
            var ship = _register.GetTankerShipByImo(imo);
            if (ship == null)
            {
                return NotFound();
            }

            try
            {
                ship.EmptyTank(tankId);
            }
            catch (TankDoesntExistException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TankAlreadyEmptyException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpGet("{imo}/tanks/get/{tankId}")]
        public IActionResult GetTank(string imo, Guid tankId)
        {
            var ship = _register.GetTankerShipByImo(imo);
            if (ship == null)
            {
                return NotFound();
            }

            try
            {
                var tank = ship.GetTank(tankId);
                return Ok(tank);
            }
            catch (TankDoesntExistException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{imo}/tanks/getAll")]
        public IActionResult GetAllTanks(string imo)
        {
            var ship = _register.GetTankerShipByImo(imo);
            if (ship == null)
            {
                return NotFound();
            }

            var tanks = ship.GetAllTanks();
            return Ok(tanks);
        }
    }
}