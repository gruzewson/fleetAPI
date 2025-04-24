   // IShipRepository, ShipRepository
using FleetAPI.Models.Ships;      // Ship, PassengerShip, TankerShip
using FleetAPI.Models.Passengers;  
using FleetAPI.Models.Tanks;// ShipType
using FleetAPI.Exceptions;
using FleetAPI.Repositories; // Custom exceptions
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// 1) DI registrations
builder.Services.AddScoped<IShipRepository, ShipRepository>();

var app = builder.Build();

// Test the repository and ships
using (var scope = app.Services.CreateScope())
{
    var shipRepository = scope.ServiceProvider.GetRequiredService<IShipRepository>();

    // Add some ships
    try
    {
        var passengerShip = new PassengerShip(
            imo: "IMO9224764",
            name: "Ocean Explorer",
            length: 300.5f,
            width: 50.2f,
            passengers: new List<Passenger>
            {
                new Passenger(1, "John", "Doe"),
                new Passenger(2, "Jane", "Smith")
            }
        );

        var tankerShip = new TankerShip(
            imo: "IMO9829930",
            name: "Oil Titan",
            length: 400.0f,
            width: 60.0f,
            tanks: new List<Tank>
            {
                new Tank(FuelType.Diesel, 100000),
                new Tank(FuelType.HeavyFuel, 200000)
            }
        );

        shipRepository.AddShip(passengerShip);
        shipRepository.AddShip(tankerShip);

        Console.WriteLine("Ships added successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error adding ships: {ex.Message}");
    }

    // Retrieve and display all ships
    Console.WriteLine("\nAll Ships:");
    foreach (var ship in shipRepository.GetAllShips())
    {
        Console.WriteLine(ship);
    }

    // Retrieve a ship by IMO number
    try
    {
        var retrievedShip = shipRepository.GetShipByImo("IMO9224764");
        Console.WriteLine($"\nRetrieved Ship: {retrievedShip}");
    }
    catch (ShipNotFoundException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    // Remove a ship
    try
    {
        shipRepository.RemoveShip("IMO9829930");
        Console.WriteLine("\nShip with IMO1234567 removed.");
    }
    catch (ShipNotFoundException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }

    // Display remaining ships
    Console.WriteLine("\nRemaining Ships:");
    foreach (var ship in shipRepository.GetAllShips())
    {
        Console.WriteLine(ship);
    }
}

app.Run();