using System.Collections.Generic;
using System.Linq;
using Xunit;
using FleetAPI.Data;
using FleetAPI.Models.Ships;
using FleetAPI.Exceptions;
using FleetAPI.Factories;
using FleetAPI.Models.Tanks;
using FleetAPI.Models.Passengers;

namespace FleetAPI.Tests.ShipsTests
{
    public class ShipRepoTests
    {
        private readonly ShipRepository _shipRepository;
        private readonly PassengerShip _correctPassengerShip;
        private readonly TankerShip _correctTankerShip;

        public ShipRepoTests()
        {
            var tankerFactory = new TankerShipFactory();
            var passengerFactory = new PassengerShipFactory();
            _correctTankerShip = tankerFactory.Create(
                imo: "IMO9074729",
                name: "Tanker Ship",
                length: 300f,
                width: 50f,
                tanks: new List<Tank>
                {
                    new Tank(FuelType.Diesel, 100.0),
                }
            );
            _correctPassengerShip = passengerFactory.Create(
                imo: "IMO9224764",
                name: "Passenger Ship",
                length: 300f,
                width: 50f,
                passengers: Enumerable.Empty<Passenger>()
            );
            _shipRepository = new ShipRepository();
            _shipRepository.AddShip(_correctPassengerShip);
        }

        [Fact]
        public void AddShip_ShouldAddShip_WhenValidData()
        {
            // Act
            _shipRepository.AddShip(_correctTankerShip);

            // Assert
            var ship = _shipRepository.GetShipByImo("IMO9074729");
            Assert.Equal(_correctTankerShip, ship);
        }

        [Fact]
        public void AddShip_ShouldThrowException_WhenShipAlreadyExists()
        {
            // Act & Assert
            var ex = Assert.Throws<ShipAlreadyExistsException>(() =>
                _shipRepository.AddShip(_correctPassengerShip));
            Assert.Equal("Ship with IMO 'IMO9224764' already exists.", ex.Message);
        }

        [Fact]
        public void AddShip_ShouldThrowException_WhenShipIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                _shipRepository.AddShip(null));
            Assert.Equal("Value cannot be null. (Parameter 'ship')", ex.Message);
        }

        [Fact]
        public void RemoveShip_ShouldRemoveShip_WhenValidImo()
        {
            // Act
            _shipRepository.RemoveShip("IMO9224764");

            // Assert
            Assert.Throws<ShipNotFoundException>(() =>
                _shipRepository.GetShipByImo("IMO9224764"));
        }

        [Fact]
        public void RemoveShip_ShouldThrowException_WhenShipNotFound()
        {
            // Act & Assert
            var ex = Assert.Throws<ShipNotFoundException>(() =>
                _shipRepository.RemoveShip("IMO1234567"));
            Assert.Equal("Ship with IMO 'IMO1234567' not found.", ex.Message);
        }

        [Fact]
        public void GetShipByImo_ShouldReturnShip_WhenValidImo()
        {
            // Act
            var ship = _shipRepository.GetShipByImo("IMO9224764");

            // Assert
            Assert.Equal(_correctPassengerShip, ship);
        }

        [Fact]
        public void GetShipByImo_ShouldThrowException_WhenShipNotFound()
        {
            // Act & Assert
            var ex = Assert.Throws<ShipNotFoundException>(() =>
                _shipRepository.GetShipByImo("IMO1234567"));
            Assert.Equal("Ship with IMO 'IMO1234567' not found.", ex.Message);
        }

        [Fact]
        public void GetAllShips_ShouldReturnAllShips()
        {
            // Arrange
            _shipRepository.AddShip(_correctTankerShip);

            // Act
            var ships = _shipRepository.GetAllShips();

            // Assert
            Assert.Contains(_correctPassengerShip, ships);
            Assert.Contains(_correctTankerShip, ships);
            Assert.Equal(2, ships.Count());
        }

        [Theory]
        [InlineData(ShipType.Tanker)]
        [InlineData(ShipType.Passenger)]
        public void GetShipsByType_ShouldReturnShipsOfType(ShipType type)
        {
            // Arrange
            _shipRepository.AddShip(_correctTankerShip);

            // Act
            var ships = _shipRepository.GetShipsByType(type);

            // Assert
            if (type == ShipType.Tanker)
            {
                Assert.Contains(_correctTankerShip, ships);
                Assert.DoesNotContain(_correctPassengerShip, ships);
            }
            else
            {
                Assert.Contains(_correctPassengerShip, ships);
                Assert.DoesNotContain(_correctTankerShip, ships);
            }
        }

        [Fact]
        public void GetShipsByType_ShouldReturnEmptyList_WhenNoShipsOfType()
        {
            // Act
            var ships = _shipRepository.GetShipsByType(ShipType.Tanker);

            // Assert
            Assert.Empty(ships);

        }
    }
}
