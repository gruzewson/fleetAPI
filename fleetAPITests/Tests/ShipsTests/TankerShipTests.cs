using System.Collections.Generic;
using System.Linq;
using Xunit;
using FleetAPI.Models.Ships;
using FleetAPI.Factories;
using FleetAPI.Models.Tanks;
using FleetAPI.Exceptions;

namespace FleetAPI.Tests.ShipsTests
{
    public class TankerShipTests
    {
        private readonly TankerShip _correctShip;

        public TankerShipTests()
        {
            var factory = new TankerShipFactory();
            _correctShip = factory.Create(
                imo: "IMO9074729",
                name: "Test Ship",
                length: 300f,
                width: 50f,
                tanks: new List<Tank>
                {
                    new Tank(FuelType.Diesel, 100.0),
                    new Tank(FuelType.HeavyFuel, 100.0)
                }
            );
            _correctShip.FillTank(_correctShip.Tanks.First().TankId, 10);
        }
        

        [Fact]
        public void FillTank_ShouldFillTank_WhenValidTankIdAndLiters()
        {
            // Arrange
            var tank = _correctShip.Tanks.First();
            var initialLiters = tank.CurrentLitersNumber; //10
            const int litersToFill = 50;

            // Act
            _correctShip.FillTank(tank.TankId, litersToFill);

            // Assert
            Assert.Equal(initialLiters + litersToFill, tank.CurrentLitersNumber);
        }

        [Fact]
        public void FillTank_ShouldThrowException_WhenTankNotFound()
        {
            // Arrange
            var invalidTankId = Guid.NewGuid();
            const int litersToFill = 50;

            // Act & Assert
            Assert.Throws<TankDoesntExistException>(() => _correctShip.FillTank(invalidTankId, litersToFill));
        }

        [Theory]
        [InlineData(200, typeof(TankOverfillException))]
        [InlineData(-1,  typeof(InvalidTankFillAmountException))]
        public void FillTank_InvalidAmounts_ThrowsExpectedException(
            int litersToFill, Type expectedException)
        {
            // Arrange
            var tank  = _correctShip.Tanks.First();
            var action = () => _correctShip.FillTank(tank.TankId, litersToFill);

            // Act & Assert
            Assert.Throws(expectedException, action);
        }

        [Fact]
        public void EmptyTank_ShouldEmptyTank_WhenValidTankId()
        {
            // Arrange
            var tank = _correctShip.Tanks.First();

            // Act
            _correctShip.EmptyTank(tank.TankId);

            // Assert
            Assert.Equal(0, tank.CurrentLitersNumber);
        }

        [Fact]
        public void EmptyTank_ShouldThrowException_WhenTankNotFound()
        {
            // Arrange
            var invalidTankId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<TankDoesntExistException>(() => _correctShip.EmptyTank(invalidTankId));
        }

        [Fact]
        public void EmptyTank_ShouldThrowException_WhenTankAlreadyEmpty()
        {
            // Arrange
            var tank = _correctShip.Tanks.First();

            // Act
            _correctShip.EmptyTank(tank.TankId);

            // Assert
            Assert.Throws<TankAlreadyEmptyException>(() => _correctShip.EmptyTank(tank.TankId));
        }
        
        [Fact]
        public void GetTank_ShouldReturnTank_WhenValidTankId()
        {
            // Arrange
            var tank = _correctShip.Tanks.First();

            // Act
            var result = _correctShip.GetTank(tank.TankId);

            // Assert
            Assert.Equal(tank, result);
        }
        
        [Fact]
        public void GetTank_ShouldThrowException_WhenTankNotFound()
        {
            // Arrange
            var invalidTankId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<TankDoesntExistException>(() => _correctShip.GetTank(invalidTankId));
        }
        
        [Fact]
        public void GetAllTanks_ShouldReturnAllTanks()
        {
            // Act
            var tanks = _correctShip.GetAllTanks();

            // Assert
            Assert.Equal(_correctShip.Tanks.Count, tanks.Count());
            Assert.Equal(_correctShip.Tanks, tanks);
        }
    }
}
