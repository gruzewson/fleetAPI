using FleetAPI.Data;
using FleetAPI.Factories;
using FleetAPI.Models.Ships;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddSingleton<IShipRegister, ShipRegister>();
builder.Services.AddScoped<IShipFactory<PassengerShip>, PassengerShipFactory>();
builder.Services.AddScoped<IShipFactory<TankerShip>,   TankerShipFactory>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.Run();
