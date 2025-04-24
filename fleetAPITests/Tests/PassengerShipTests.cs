using System.Collections.Generic;
using System.Linq;
using Xunit;
using FleetAPI.Models.Ships;
using FleetAPI.Factories;
using FleetAPI.Models.Passengers;
using FleetAPI.Exceptions;

namespace FleetAPI.Tests
{
    public class PassengerShipTests
    {
        private readonly PassengerShip _correctShip;

        public PassengerShipTests()
        {
            var factory = new PassengerShipFactory();
            _correctShip = factory.Create(
                imo: "IMO9074729",
                name: "Test Ship",
                length: 300f,
                width: 50f,
                passengers: Enumerable.Empty<Passenger>()
            );
        }

        [Fact]
        public void AddPassenger_ShouldAddPassenger_WhenValidData()
        {
            // Act
            _correctShip.AddPassenger("Andrew", "Wandrew");

            // Assert
            var passenger = Assert.Single(_correctShip.Passengers);
            Assert.Equal("Andrew", passenger.Name);
            Assert.Equal("Wandrew", passenger.Surname);
            Assert.Equal(1, _correctShip.PassengerCount);
            Assert.Equal(1, passenger.PassengerID);
        }

        [Theory]
        [InlineData("", "", "Name is required.")]
        [InlineData("Andrew", "", "Surname is required.")]
        [InlineData("", "Wandrew", "Name is required.")]
        public void AddPassenger_ShouldThrowException_WhenInvalidData(string name, string surname, string expectedMessage)
        {
            // Act & Assert
            var ex = Assert.Throws<InvalidPassengerDataException>(() =>
                _correctShip.AddPassenger(name, surname));
            Assert.Equal(expectedMessage, ex.Message);
        }

        [Fact]
        public void UpdatePassengerInfo_ShouldUpdatePassenger_WhenValidData()
        {
            // Arrange
            _correctShip.AddPassenger("Andrew", "Wandrew");
            var passenger = _correctShip.Passengers.First();

            // Act
            _correctShip.UpdatePassengerInfo(passenger.PassengerID, "NewName", "NewSurname");

            // Assert
            Assert.Equal("NewName", passenger.Name);
            Assert.Equal("NewSurname", passenger.Surname);
        }

        [Theory]
        [InlineData("", "", "Name is required.")]
        [InlineData("Andrew", "", "Surname is required.")]
        [InlineData("", "Wandrew", "Name is required.")]
        public void UpdatePassengerInfo_ShouldThrowException_WhenInvalidData(string newName, string newSurname, string expectedMessage)
        {
            // Arrange
            _correctShip.AddPassenger("Andrew", "Wandrew");
            var passenger = _correctShip.Passengers.First();

            // Act & Assert
            var ex = Assert.Throws<InvalidPassengerDataException>(() =>
                _correctShip.UpdatePassengerInfo(passenger.PassengerID, newName, newSurname));
            Assert.Equal(expectedMessage, ex.Message);
        }

        [Fact]
        public void UpdatePassengerInfo_ShouldThrowException_WhenPassengerNotFound()
        {
            var ex = Assert.Throws<PassengerNotFoundException>(() =>
                _correctShip.UpdatePassengerInfo(999, "NewName", "NewSurname"));
            Assert.Equal("Passenger with ID 999 not found.", ex.Message);
        }

        [Fact]
        public void RemovePassengerById_ShouldRemovePassenger_WhenValidId()
        {
            // Arrange
            _correctShip.AddPassenger("Andrew", "Wandrew");
            var passenger = _correctShip.Passengers.First();

            // Act
            _correctShip.RemovePassengerById(passenger.PassengerID);

            // Assert
            Assert.Empty(_correctShip.Passengers);
            Assert.Equal(0, _correctShip.PassengerCount);
        }

        [Fact]
        public void RemovePassengerById_ShouldThrowException_WhenPassengerNotFound()
        {
            var ex = Assert.Throws<PassengerNotFoundException>(() =>
                _correctShip.RemovePassengerById(999));
            Assert.Equal("Passenger with ID 999 not found.", ex.Message);
        }

        [Fact]
        public void GetPassengerById_ShouldReturnPassenger_WhenValidId()
        {
            // Arrange
            _correctShip.AddPassenger("Andrew", "Wandrew");
            var passenger = _correctShip.Passengers.First();

            // Act
            var result = _correctShip.GetPassengerById(passenger.PassengerID);

            // Assert
            Assert.Equal(passenger, result);
        }

        [Fact]
        public void GetPassengerById_ShouldThrowException_WhenPassengerNotFound()
        {
            var ex = Assert.Throws<PassengerNotFoundException>(() =>
                _correctShip.GetPassengerById(999));
            Assert.Equal("Passenger with ID 999 not found.", ex.Message);
        }
    }
}
