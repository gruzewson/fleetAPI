using Microsoft.AspNetCore.Mvc;
using FleetAPI.Models.Ships;

using FleetAPI.Factories;
using FleetAPI.Data;


namespace FleetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PassengerShipController : ControllerBase
{
    private readonly IShipRepository _repo;
    private readonly Func<string, IShipFactory<Ship>> _factory;

    public PassengerShipController(
        IShipRepository repo,
        Func<string, IShipFactory<Ship>> factory)
    {
        _repo = repo;
        _factory = factory;
    }

    // [HttpPost]
    // public async Task<IActionResult> AddShip(
    //     [FromQuery] string type,
    //     [FromBody] ShipDto dto)
    // {
    //     // 1. Check for duplicate IMO
    //     if (await _repo.ExistsAsync(dto.ImoNumber))
    //         return Conflict("A ship with that IMO already exists.");
    //
    //     // 2. Use the factory to create the correct subtype
    //     var ship = _factory(type)
    //                .Create(dto.ImoNumber, dto.Name, dto.Length, dto.Width,
    //                        dto.Passengers, dto.Tanks);
    //
    //     // 3. Persist and return
    //     await _repo.AddAsync(ship);
    //     await _repo.SaveChangesAsync();
    //
    //     return CreatedAtAction(nameof(GetShip),
    //                            new { imo = ship.ImoNumber },
    //                            ship);
    // }
    //
    // [HttpGet("{imo}")]
    // public async Task<IActionResult> GetShip(int imo)
    // {
    //     var ship = await _repo.GetByImoAsync(imo);
    //     return ship is null ? NotFound() : Ok(ship);
    // }
}
