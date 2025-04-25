using FleetAPI.Data;
using FleetAPI.Factories;
using FleetAPI.Models.Ships;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1) Dependency Injection
builder.Services.AddSingleton<IShipRegister, ShipRegister>();
builder.Services.AddScoped<IShipFactory<PassengerShip>, PassengerShipFactory>();
builder.Services.AddScoped<IShipFactory<TankerShip>,   TankerShipFactory>();

// 2) Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// 3) Map attribute-routed controllers
app.MapControllers();

// 4) Run the application
app.Run();
