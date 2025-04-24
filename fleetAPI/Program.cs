// Program.cs
using FleetAPI.Repositories;      // IShipRepository, ShipRepository
using FleetAPI.Factories;        // PassengerShipFactory, TankerShipFactory, IShipFactory<Ship>
using FleetAPI.Models;           // string, ShipDto
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1) DI registrations
builder.Services.AddScoped<IShipRepository, ShipRepository>();
builder.Services.AddScoped<PassengerShipFactory>();
builder.Services.AddScoped<TankerShipFactory>();
builder.Services.AddScoped<Func<string, IShipFactory<Ship>>>(sp => type =>
    type == "Passenger"
        ? sp.GetRequiredService<PassengerShipFactory>() as IShipFactory<Ship>
        : sp.GetRequiredService<TankerShipFactory>()    as IShipFactory<Ship>
);

// 2) Add controllers only
builder.Services.AddControllers();

var app = builder.Build();

// 3) Map controllers and run
app.MapControllers();
app.Run();