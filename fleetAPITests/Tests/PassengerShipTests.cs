using FleetAPI.Models;
using FleetAPI.Factories;
using Xunit;
using System.Collections.Generic;

namespace FleetAPI.Tests
{
    public class PassengerShipTests
    {
        private readonly IShipFactory<PassengerShip> _factory;

        public PassengerShipTests()
        {
            _factory = new PassengerShipFactory();
            _correctShip = _factory.Create(
                imo: "IMO9074729",
                name: "Test Ship",
                length: 300.0,
                width: 50.0,
                passengers: new List<Passenger>()
            );
            _correctShip.AddPassenger("John", "Doe"); //passengers[0]
        }

        // add passenger should work
        [Fact]
        public void AddPassenger_ShouldAddPassenger_WhenValidData()
        {
            // Act
            ship.AddPassenger("Andrew", "Wandrew");

            // Assert
            var passenger = Assert.Single(_correctShip.Passengers);
            Assert.Equal("Andrew", passenger.Name);
            Assert.Equal("Wandrew", passenger.Surname);
        }












        [Fact]
        public void AddPassenger_ShouldAddPassenger_WhenCapacityNotExceeded()
        {
            // Arrange: create an empty ship and set capacity
            var ship = _factory.Create(
                imo: 1234567,
                name: "Test Ship",
                length: 300f,
                width: 50f,
                passengers: new List<Passenger>()
            );
            ship.MaxPassengers = 2;
            ship.CurrentPassengers = 0;
            ship.CurrentPassengerID = 0;

            // Act
            ship.AddPassenger("John", "Doe");

            // Assert
            Assert.Single(ship.Passengers);
            Assert.Equal("John", ship.Passengers[0].Name);
            Assert.Equal("Doe", ship.Passengers[0].Surname);
        }

        [Fact]
        public void AddPassenger_ShouldNotAddPassenger_WhenCapacityExceeded()
        {
            // Arrange: create a ship at full capacity
            var ship = _factory.Create(
                imo: 1234567,
                name: "Test Ship",
                length: 300f,
                width: 50f,
                passengers: new List<Passenger> { new Passenger(1, "Alice", "Doe"), new Passenger(2, "Bob", "Smith") }
            );
            // MaxPassengers and CurrentPassengers initialized by ctor from initial list

            // Act
            ship.AddPassenger("Charlie", "Brown");

            // Assert: no new passenger added
            Assert.Equal(2, ship.Passengers.Count);
            Assert.DoesNotContain(ship.Passengers, p => p.Name == "Charlie" && p.Surname == "Brown");
        }

        [Fact]
        public void RemovePassengerById_ShouldRemovePassenger_WhenPassengerExists()
        {
            // Arrange: create ship with one passenger
            var ship = _factory.Create(
                imo: 1234567,
                name: "Test Ship",
                length: 300f,
                width: 50f,
                passengers: new List<Passenger> { new Passenger(1, "John", "Doe") }
            );

            // Act
            ship.RemovePassengerById(1);

            // Assert
            Assert.Empty(ship.Passengers);
        }

        [Fact]
        public void UpdatePassenger_ShouldUpdatePassenger_WhenPassengerExists()
        {
            // Arrange: create ship and add one passenger
            var ship = _factory.Create(
                imo: 1234567,
                name: "Test Ship",
                length: 300f,
                width: 50f,
                passengers: new List<Passenger>()
            );
            ship.MaxPassengers = 1;
            ship.CurrentPassengers = 0;
            ship.CurrentPassengerID = 0;
            ship.AddPassenger("John", "Doe");

            // Act
            ship.UpdatePassenger(1, "Jane", "Smith");

            // Assert
            Assert.Single(ship.Passengers);
            Assert.Equal("Jane", ship.Passengers[0].Name);
            Assert.Equal("Smith", ship.Passengers[0].Surname);
        }

        [Fact]
        public void GetPassengerById_ShouldReturnPassenger_WhenPassengerExists()
        {
            // Arrange: create ship and add one passenger
            var ship = _factory.Create(
                imo: 1234567,
                name: "Test Ship",
                length: 300f,
                width: 50f,
                passengers: new List<Passenger>()
            );
            ship.MaxPassengers = 1;
            ship.CurrentPassengers = 0;
            ship.CurrentPassengerID = 0;
            ship.AddPassenger("John", "Doe");

            // Act
            var passenger = ship.GetPassengerById(1);

            // Assert
            Assert.NotNull(passenger);
            Assert.Equal("John", passenger.Name);
            Assert.Equal("Doe", passenger.Surname);
        }
    }
}

// add passenger should work
//add passenger wrong name, wrong surname
// update passenger should work
// update passenger wrong name, wrong surname
// update passenger not found
// remove passenger should work
// remove passenger not found
// get passenger by id should work
// get passenger by id not found
