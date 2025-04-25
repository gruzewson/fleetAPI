using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FleetAPI.Controllers;
using FleetAPI.Data;
using FleetAPI.Factories;
using FleetAPI.Models.Passengers;
using FleetAPI.Models.Ships;
using FleetAPI.Models.Tanks;

namespace FleetAPI.Tests.Controllers
{
    public class TankerShipControllerTests
    {
        private readonly Mock<IShipRegister>              _regMock;
        private readonly TankerShipController            _ctrl;
        private const string TestImo = "IMO9074729";

        public TankerShipControllerTests()
        {
            _regMock    = new Mock<IShipRegister>();
            _ctrl = new TankerShipController(
                _regMock.Object
            );
        }

        [Fact]
        public void FillTank_ShouldFill_WhenValid()
        {
            var ship = new TankerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Tank>());
            var tank = new Tank(FuelType.Diesel, 1000.0);
            ship.Tanks.Add(tank);
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.FillTank(TestImo, tank.TankId, 500);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(500, tank.CurrentLitersNumber);
        }

        [Fact]
        public void FillTank_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns((TankerShip)null!);

            var result = _ctrl.FillTank(TestImo, Guid.NewGuid(), 500);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void FillTank_ShouldReturnBadRequest_WhenTankDoesNotExist()
        {
            var ship = new TankerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Tank>());
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.FillTank(TestImo, Guid.NewGuid(), 500);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void FillTank_ShouldReturnBadRequest_WhenTankOverfills()
        {
            var ship = new TankerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Tank>());
            var tank = new Tank(FuelType.Diesel, 1000.0);
            ship.Tanks.Add(tank);
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.FillTank(TestImo, tank.TankId, 1500);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void EmptyTank_ShouldEmpty_WhenValid()
        {
            var ship = new TankerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Tank>());
            var tank = new Tank(FuelType.Diesel, 1000.0);
            tank.FillTank(500);
            ship.Tanks.Add(tank);
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.EmptyTank(TestImo, tank.TankId);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, tank.CurrentLitersNumber);
        }

        [Fact]
        public void EmptyTank_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns((TankerShip)null!);

            var result = _ctrl.EmptyTank(TestImo, Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void EmptyTank_ShouldReturnBadRequest_WhenTankDoesNotExist()
        {
            var ship = new TankerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Tank>());
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.EmptyTank(TestImo, Guid.NewGuid());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void EmptyTank_ShouldReturnBadRequest_WhenTankAlreadyEmpty()
        {
            var ship = new TankerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Tank>());
            var tank = new Tank(FuelType.Diesel, 1000.0);
            ship.Tanks.Add(tank);
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.EmptyTank(TestImo, tank.TankId);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetTank_ShouldReturnTank_WhenValid()
        {
            var ship = new TankerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Tank>());
            var tank = new Tank(FuelType.Diesel, 1000.0);
            ship.Tanks.Add(tank);
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.GetTank(TestImo, tank.TankId);
;
            var okResult = Assert.IsType<OkObjectResult>(result);
            var gotTank = Assert.IsType<Tank>(okResult.Value);
            Assert.Equal(tank.TankId, gotTank.TankId);
            Assert.Equal(tank.FuelType, gotTank.FuelType);
            Assert.Equal(tank.Capacity, gotTank.Capacity);
            Assert.Equal(tank.CurrentLitersNumber, gotTank.CurrentLitersNumber);
        }

        [Fact]
        public void GetTank_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns((TankerShip)null!);

            var result = _ctrl.GetTank(TestImo, Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetTank_ShouldReturnBadRequest_WhenTankDoesNotExist()
        {
            var ship = new TankerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Tank>());
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.GetTank(TestImo, Guid.NewGuid());

            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public void GetAllTanks_ShouldReturnAllTanks_WhenValid()
        {
            var ship = new TankerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Tank>());
            var tank1 = new Tank(FuelType.Diesel, 1000.0);
            var tank2 = new Tank(FuelType.HeavyFuel, 2000.0);
            ship.Tanks.Add(tank1);
            ship.Tanks.Add(tank2);
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.GetAllTanks(TestImo);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var gotTanks = Assert.IsType<List<Tank>>(okResult.Value);
            Assert.Equal(2, gotTanks.Count);
        }
        
        [Fact]
        public void GetAllTanks_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            _regMock.Setup(r => r.GetTankerShipByImo(TestImo)).Returns((TankerShip)null!);

            var result = _ctrl.GetAllTanks(TestImo);

            Assert.IsType<NotFoundResult>(result);
        }
        

    }
}