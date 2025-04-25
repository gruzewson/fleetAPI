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
    public class ShipRegisterTests
    {
        private readonly ShipRegister _shipRegister;
        private readonly PassengerShip _correctPassengerShip;
        private readonly TankerShip _correctTankerShip;

        private const string WRONG_IMO = "IMO1234567";
        private const string CORRECT_PASSENGER_IMO = "IMO9224764";
        private const string CORRECT_TANKER_IMO = "IMO9074729";

        public ShipRegisterTests()
        {
            var tankerFactory = new TankerShipFactory();
            var passengerFactory = new PassengerShipFactory();
            _correctTankerShip = tankerFactory.Create(
                imo: CORRECT_TANKER_IMO,
                name: "Tanker Ship",
                length: 300f,
                width: 50f,
                tanks: new List<Tank>
                {
                    new Tank(FuelType.Diesel, 100.0),
                }
            );
            _correctPassengerShip = passengerFactory.Create(
                imo: CORRECT_PASSENGER_IMO,
                name: "Passenger Ship",
                length: 300f,
                width: 50f,
                passengers: []
            );
            _shipRegister = new ShipRegister();
            _shipRegister.AddShip(_correctPassengerShip);
        }

        [Fact]
        public void AddShip_ShouldAddShip_WhenValidData()
        {
            // Act
            _shipRegister.AddShip(_correctTankerShip);

            // Assert
            var ship = _shipRegister.GetShipByImo(CORRECT_TANKER_IMO);
            Assert.Equal(_correctTankerShip, ship);
        }

        [Fact]
        public void AddShip_ShouldThrowException_WhenShipAlreadyExists()
        {
            // Act & Assert
            var ex = Assert.Throws<ShipAlreadyExistsException>(() =>
                _shipRegister.AddShip(_correctPassengerShip));
            Assert.Equal($"Ship with IMO '{CORRECT_PASSENGER_IMO}' already exists.", ex.Message);
        }

        [Fact]
        public void AddShip_ShouldThrowException_WhenShipIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                _shipRegister.AddShip(null!));
            Assert.Equal("Value cannot be null. (Parameter 'ship')", ex.Message);
        }

        [Fact]
        public void RemoveShip_ShouldRemoveShip_WhenValidImo()
        {
            // Act
            _shipRegister.RemoveShip(CORRECT_PASSENGER_IMO);

            // Assert
            Assert.Throws<ShipNotFoundException>(() =>
                _shipRegister.GetShipByImo(CORRECT_PASSENGER_IMO));
        }

        [Fact]
        public void RemoveShip_ShouldThrowException_WhenShipNotFound()
        {
            // Act & Assert
            var ex = Assert.Throws<ShipNotFoundException>(() =>
                _shipRegister.RemoveShip(WRONG_IMO));
            Assert.Equal($"Ship with IMO '{WRONG_IMO}' not found.", ex.Message);
        }

        [Fact]
        public void GetShipByImo_ShouldReturnShip_WhenValidImo()
        {
            // Act
            var ship = _shipRegister.GetShipByImo(CORRECT_PASSENGER_IMO);

            // Assert
            Assert.Equal(_correctPassengerShip, ship);
        }

        [Fact]
        public void GetShipByImo_ShouldThrowException_WhenShipNotFound()
        {
            // Act & Assert
            var ex = Assert.Throws<ShipNotFoundException>(() =>
                _shipRegister.GetShipByImo(WRONG_IMO));
            Assert.Equal($"Ship with IMO '{WRONG_IMO}' not found.", ex.Message);
        }
        
        [Fact]
        public void GetPassengerShipByImo_ShouldReturnPassengerShip_WhenValidImo()
        {
            // Act
            var ship = _shipRegister.GetPassengerShipByImo(CORRECT_PASSENGER_IMO);

            // Assert
            Assert.Equal(_correctPassengerShip, ship);
        }

        [Fact]
        public void GetPassengerShipByImo_ShouldThrowException_WhenShipNotFound()
        {
            // Act & Assert
            var ex = Assert.Throws<ShipNotFoundException>(() =>
                _shipRegister.GetPassengerShipByImo(WRONG_IMO));
            Assert.Equal($"Ship with IMO '{WRONG_IMO}' not found.", ex.Message);
        }

        [Fact]
        public void GetTankerShipByImo_ShouldReturnTankerShip_WhenValidImo()
        {
            // Arrange
            _shipRegister.AddShip(_correctTankerShip);

            // Act
            var ship = _shipRegister.GetTankerShipByImo(CORRECT_TANKER_IMO);

            // Assert
            Assert.Equal(_correctTankerShip, ship);
        }

        [Fact]
        public void GetTankerShipByImo_ShouldThrowException_WhenShipNotFound()
        {
            // Act & Assert
            var ex = Assert.Throws<ShipNotFoundException>(() =>
                _shipRegister.GetTankerShipByImo(WRONG_IMO));
            Assert.Equal($"Ship with IMO '{WRONG_IMO}' not found.", ex.Message);
        }
        
        

        [Fact]
        public void GetAllShips_ShouldReturnAllShips()
        {
            // Arrange
            _shipRegister.AddShip(_correctTankerShip);

            // Act
            var ships = _shipRegister.GetAllShips();

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
            _shipRegister.AddShip(_correctTankerShip);

            // Act
            var ships = _shipRegister.GetShipsByType(type);

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
            var ships = _shipRegister.GetShipsByType(ShipType.Tanker);

            // Assert
            Assert.Empty(ships);

        }
    }
}
