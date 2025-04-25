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
        private readonly Mock<IShipRepository>              _repoMock;
        private readonly Mock<IShipFactory<TankerShip>> _factoryMock;
        private readonly TankerShipController            _ctrl;

        public TankerShipControllerTests()
        {
            _repoMock    = new Mock<IShipRepository>();
            _factoryMock = new Mock<IShipFactory<TankerShip>>();
            _ctrl = new TankerShipController(
                _repoMock.Object,
                _factoryMock.Object
            );
        }

        [Fact]
        public void FillTank_ShouldFill_WhenValid()
        {
            string imo = "IMO9074729";
            var ship = new TankerShip(imo, "Test Ship", 100.0, 50.0, new List<Tank>());
            var tank = new Tank(FuelType.Diesel, 1000.0);
            ship.Tanks.Add(tank);
            _repoMock.Setup(r => r.GetTankerShipByImo(imo)).Returns(ship);

            var result = _ctrl.FillTank(imo, tank.TankID, 500);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(500, tank.CurrentLitersNumber);
        }

        [Fact]
        public void FillTank_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            string imo = "IMO9074729";
            _repoMock.Setup(r => r.GetTankerShipByImo(imo)).Returns((TankerShip)null);

            var result = _ctrl.FillTank(imo, Guid.NewGuid(), 500);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void FillTank_ShouldReturnBadRequest_WhenTankDoesNotExist()
        {
            string imo = "IMO9074729";
            var ship = new TankerShip(imo, "Test Ship", 100.0, 50.0, new List<Tank>());
            _repoMock.Setup(r => r.GetTankerShipByImo(imo)).Returns(ship);

            var result = _ctrl.FillTank(imo, Guid.NewGuid(), 500);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void FillTank_ShouldReturnBadRequest_WhenTankOverfills()
        {
            string imo = "IMO9074729";
            var ship = new TankerShip(imo, "Test Ship", 100.0, 50.0, new List<Tank>());
            var tank = new Tank(FuelType.Diesel, 1000.0);
            ship.Tanks.Add(tank);
            _repoMock.Setup(r => r.GetTankerShipByImo(imo)).Returns(ship);

            var result = _ctrl.FillTank(imo, tank.TankID, 1500);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void EmptyTank_ShouldEmpty_WhenValid()
        {
            string imo = "IMO9074729";
            var ship = new TankerShip(imo, "Test Ship", 100.0, 50.0, new List<Tank>());
            var tank = new Tank(FuelType.Diesel, 1000.0);
            tank.FillTank(500);
            ship.Tanks.Add(tank);
            _repoMock.Setup(r => r.GetTankerShipByImo(imo)).Returns(ship);

            var result = _ctrl.EmptyTank(imo, tank.TankID);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(0, tank.CurrentLitersNumber);
        }

        [Fact]
        public void EmptyTank_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            string imo = "IMO9074729";
            _repoMock.Setup(r => r.GetTankerShipByImo(imo)).Returns((TankerShip)null);

            var result = _ctrl.EmptyTank(imo, Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void EmptyTank_ShouldReturnBadRequest_WhenTankDoesNotExist()
        {
            string imo = "IMO9074729";
            var ship = new TankerShip(imo, "Test Ship", 100.0, 50.0, new List<Tank>());
            _repoMock.Setup(r => r.GetTankerShipByImo(imo)).Returns(ship);

            var result = _ctrl.EmptyTank(imo, Guid.NewGuid());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void EmptyTank_ShouldReturnBadRequest_WhenTankAlreadyEmpty()
        {
            string imo = "IMO9074729";
            var ship = new TankerShip(imo, "Test Ship", 100.0, 50.0, new List<Tank>());
            var tank = new Tank(FuelType.Diesel, 1000.0);
            ship.Tanks.Add(tank);
            _repoMock.Setup(r => r.GetTankerShipByImo(imo)).Returns(ship);

            var result = _ctrl.EmptyTank(imo, tank.TankID);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetTank_ShouldReturnTank_WhenValid()
        {
            string imo = "IMO9074729";
            var ship = new TankerShip(imo, "Test Ship", 100.0, 50.0, new List<Tank>());
            var tank = new Tank(FuelType.Diesel, 1000.0);
            ship.Tanks.Add(tank);
            _repoMock.Setup(r => r.GetTankerShipByImo(imo)).Returns(ship);

            var result = _ctrl.GetTank(imo, tank.TankID);
;
            var okResult = Assert.IsType<OkObjectResult>(result);
            var gotTank = Assert.IsType<Tank>(okResult.Value);
            Assert.Equal(tank.TankID, gotTank.TankID);
            Assert.Equal(tank.FuelType, gotTank.FuelType);
            Assert.Equal(tank.Capacity, gotTank.Capacity);
            Assert.Equal(tank.CurrentLitersNumber, gotTank.CurrentLitersNumber);
        }

        [Fact]
        public void GetTank_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            string imo = "IMO9074729";
            _repoMock.Setup(r => r.GetTankerShipByImo(imo)).Returns((TankerShip)null);

            var result = _ctrl.GetTank(imo, Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetTank_ShouldReturnBadRequest_WhenTankDoesNotExist()
        {
            string imo = "IMO9074729";
            var ship = new TankerShip(imo, "Test Ship", 100.0, 50.0, new List<Tank>());
            _repoMock.Setup(r => r.GetTankerShipByImo(imo)).Returns(ship);

            var result = _ctrl.GetTank(imo, Guid.NewGuid());

            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}