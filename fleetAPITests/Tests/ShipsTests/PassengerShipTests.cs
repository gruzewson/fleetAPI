using Xunit;
using FleetAPI.Models.Ships;
using FleetAPI.Factories;
using FleetAPI.Models.Passengers;
using FleetAPI.Exceptions;

namespace FleetAPI.Tests.ShipsTests
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
                passengers: []
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
        }

        [Theory]
        [InlineData("", "", "Name is required.")]
        [InlineData("Andrew", "", "Surname is required.")]
        [InlineData("", "Wandrew", "Name is required.")]
        public void AddPassenger_ShouldThrowException_WhenInvalidData(string name, string surname,
            string expectedMessage)
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
            _correctShip.UpdatePassengerInfo(passenger.PassengerId, "NewName", "NewSurname");

            // Assert
            Assert.Equal("NewName", passenger.Name);
            Assert.Equal("NewSurname", passenger.Surname);
        }

        [Theory]
        [InlineData("", "", "Name is required.")]
        [InlineData("Andrew", "", "Surname is required.")]
        [InlineData("", "Wandrew", "Name is required.")]
        public void UpdatePassengerInfo_ShouldThrowException_WhenInvalidData(string newName, string newSurname,
            string expectedMessage)
        {
            // Arrange
            _correctShip.AddPassenger("Andrew", "Wandrew");
            var passenger = _correctShip.Passengers.First();

            // Act & Assert
            var ex = Assert.Throws<InvalidPassengerDataException>(() =>
                _correctShip.UpdatePassengerInfo(passenger.PassengerId, newName, newSurname));
            Assert.Equal(expectedMessage, ex.Message);
        }

        [Fact]
        public void UpdatePassengerInfo_ShouldThrowException_WhenPassengerNotFound()
        {
            var guid = Guid.NewGuid();
            var ex = Assert.Throws<PassengerNotFoundException>(() =>
                _correctShip.UpdatePassengerInfo(guid, "NewName", "NewSurname"));
            Assert.Equal($"Passenger with ID {guid} not found.", ex.Message);
        }

        [Fact]
        public void RemovePassengerById_ShouldRemovePassenger_WhenValidId()
        {
            // Arrange
            _correctShip.AddPassenger("Andrew", "Wandrew");
            var passenger = _correctShip.Passengers.First();

            // Act
            _correctShip.RemovePassengerById(passenger.PassengerId);

            // Assert
            Assert.Empty(_correctShip.Passengers);
            Assert.Equal(0, _correctShip.PassengerCount);
        }

        [Fact]
        public void RemovePassengerById_ShouldThrowException_WhenPassengerNotFound()
        {
            var guid = Guid.NewGuid();
            var ex = Assert.Throws<PassengerNotFoundException>(() =>
                _correctShip.RemovePassengerById(guid));
            Assert.Equal($"Passenger with ID {guid} not found.", ex.Message);
        }

        [Fact]
        public void GetPassengerById_ShouldReturnPassenger_WhenValidId()
        {
            // Arrange
            _correctShip.AddPassenger("Andrew", "Wandrew");
            var passenger = _correctShip.Passengers.First();

            // Act
            var result = _correctShip.GetPassengerById(passenger.PassengerId);

            // Assert
            Assert.Equal(passenger, result);
        }

        [Fact]
        public void GetPassengerById_ShouldThrowException_WhenPassengerNotFound()
        {
            var guid = Guid.NewGuid();
            var ex = Assert.Throws<PassengerNotFoundException>(() =>
                _correctShip.GetPassengerById(guid));
            Assert.Equal($"Passenger with ID {guid} not found.", ex.Message);
        }

        [Fact]
        public void GetAllPassengers_ShouldReturnAllPassengers()
        {
            // Arrange
            _correctShip.AddPassenger("Andrew", "Wandrew");
            _correctShip.AddPassenger("John", "Doe");

            // Act
            var passengers = _correctShip.GetAllPassengers();

            // Assert
            Assert.Equal(2, passengers.Count());
            Assert.Contains(passengers, p => p.Name == "Andrew" && p.Surname == "Wandrew");
            Assert.Contains(passengers, p => p.Name == "John" && p.Surname == "Doe");
        }
    }
}
