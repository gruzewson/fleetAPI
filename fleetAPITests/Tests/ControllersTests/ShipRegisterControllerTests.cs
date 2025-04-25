using System;
using System.Collections.Generic;
using System.Linq;
using FleetAPI.Controllers;
using FleetAPI.Data;
using FleetAPI.Factories;
using FleetAPI.Models.Passengers;
using FleetAPI.Models.Ships;
using FleetAPI.Models.Tanks;
using FleetAPI.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FleetAPI.Tests.Controllers
{
    public class ShipRegisterControllerTests
    {
        private readonly Mock<IShipRegister>                _regMock;
        private readonly Mock<IShipFactory<PassengerShip>>  _passengerFactoryMock;
        private readonly Mock<IShipFactory<TankerShip>>     _tankerFactoryMock;
        private readonly ShipRegisterController            _ctrl;

        public ShipRegisterControllerTests()
        {
            _regMock             = new Mock<IShipRegister>();
            _passengerFactoryMock = new Mock<IShipFactory<PassengerShip>>();
            _tankerFactoryMock    = new Mock<IShipFactory<TankerShip>>();
            _ctrl = new ShipRegisterController(
                _regMock.Object,
                _passengerFactoryMock.Object,
                _tankerFactoryMock.Object
            );
        }

        [Fact]
        public void Create_ReturnsCreated_WhenRequestValid()
        {
            // Arrange
            var dto = new ShipDto
            {
                ImoNumber  = "IMO9074729",
                Name       = "Test",
                Length     = 100,
                Width      = 20,
                ShipType   = ShipType.Passenger,
                Passengers = new List<Passenger>(),
                Tanks      = null
            };
            var ship = new PassengerShip("IMO9074729", "Test", 100, 20, new List<Passenger>());

            _regMock
                .Setup(r => r.Exists(dto.ImoNumber))
                .Returns(false);

            _passengerFactoryMock
               .Setup(f => f.Create(
                   dto.ImoNumber,
                   dto.Name,
                   dto.Length,
                   dto.Width,
                   dto.Passengers,
                   dto.Tanks))
               .Returns(ship);

            // Act
            var result = _ctrl.Create(dto);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(StatusCodes.Status201Created, created.StatusCode);
            Assert.Same(ship, created.Value);
            _regMock.Verify(r => r.AddShip(ship), Times.Once);
        }

        [Fact]
        public void Create_ReturnsConflict_WhenRequestInvalid()
        {
            // Arrange
            var dto = new ShipDto
            {
                ImoNumber  = "IMO9074729",
                Name       = "Test",
                Length     = 100,
                Width      = 20,
                ShipType   = ShipType.Passenger,
                Passengers = new List<Passenger>(),
                Tanks      = null
            };
            _regMock
                .Setup(r => r.Exists(dto.ImoNumber))
                .Returns(true);

            // Act
            var result = _ctrl.Create(dto);

            // Assert
            var conflict = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(StatusCodes.Status409Conflict, conflict.StatusCode);
            _regMock.Verify(r => r.AddShip(It.IsAny<Ship>()), Times.Never);
        }
        
        [Fact]
        public void Create_ReturnsBadRequest_WhenShipTypeNotSupported()
        {
            // Arrange
            var dto = new ShipDto
            {
                ImoNumber  = "IMO9074729",
                Name       = "Test",
                Length     = 100,
                Width      = 20,
                ShipType   = (ShipType)999, // Invalid ship type
                Passengers = new List<Passenger>(),
                Tanks      = null
            };

            // Act
            var result = _ctrl.Create(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
            Assert.Equal("Unsupported ship type: 999", badRequest.Value);
        }

        [Fact]
        public void GetAllShips_ReturnsOk_WithListOfShips()
        {
            // Arrange
            var ships = new List<Ship>
            {
                new PassengerShip("IMO9074729", "Test Ship 1", 100, 20, new List<Passenger>()),
                new PassengerShip("IMO9224764", "Test Ship 2", 150, 30, new List<Passenger>())
            };
            _regMock
                .Setup(r => r.GetAllShips())
                .Returns(ships);

            // Act
            var result = _ctrl.GetAllShips();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedShips = Assert.IsType<List<Ship>>(okResult.Value);
            Assert.Equal(2, returnedShips.Count);
            Assert.Equal("IMO9074729", returnedShips[0].ImoNumber);
            Assert.Equal("IMO9224764", returnedShips[1].ImoNumber);
        }

        [Fact]
        public void GetAllShips_ReturnsOk_WithEmptyList()
        {
            // Arrange
            _regMock
                .Setup(r => r.GetAllShips())
                .Returns(new List<Ship>());

            // Act
            var result = _ctrl.GetAllShips();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedShips = Assert.IsType<List<Ship>>(okResult.Value);
            Assert.Empty(returnedShips);
        }

        [Fact]
        public void GetShipByImo_ReturnsOk_WhenShipExists()
        {
            // Arrange
            var ship = new PassengerShip("IMO9074729", "Test Ship", 100, 20, new List<Passenger>());
            _regMock
                .Setup(r => r.GetShipByImo("IMO9074729"))
                .Returns(ship);

            // Act
            var result = _ctrl.GetShipByImo("IMO9074729");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Same(ship, okResult.Value);
        }

        [Fact]
        public void GetShipByImo_ReturnsNotFound_WhenShipDoesNotExist()
        {
            // Arrange
            _regMock
                .Setup(r => r.GetShipByImo("IMO9074729"))
                .Throws(new ShipNotFoundException("IMO9074729"));

            // Act
            var result = _ctrl.GetShipByImo("IMO9074729");

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
            Assert.Equal("Ship with IMO 'IMO9074729' not found.", notFound.Value);
        }

        [Fact]
        public void RemoveShip_ReturnsNoContent_WhenShipRemoved()
        {
            // Arrange
            _regMock
                .Setup(r => r.RemoveShip("IMO9074729"))
                .Verifiable();

            // Act
            var result = _ctrl.RemoveShip("IMO9074729");

            // Assert
            var noContent = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContent.StatusCode);
            _regMock.Verify(r => r.RemoveShip("IMO9074729"), Times.Once);
        }

        [Fact]
        public void RemoveShip_ReturnsNotFound_WhenShipDoesNotExist()
        {
            // Arrange
            _regMock
                .Setup(r => r.RemoveShip("IMO9074729"))
                .Throws(new ShipNotFoundException("IMO9074729"));

            // Act
            var result = _ctrl.RemoveShip("IMO9074729");

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
            Assert.Equal("Ship with IMO 'IMO9074729' not found.", notFound.Value);
        }

        [Fact]
        public void GetShipsByType_ShouldReturnShipsOfType()
        {
            // Arrange
            var ships = new List<Ship>
            {
                new PassengerShip("IMO9074729", "Test Ship 1", 100, 20, new List<Passenger>()),
                new PassengerShip("IMO9224764", "Test Ship 2", 150, 30, new List<Passenger>())
            };
            _regMock
                .Setup(r => r.GetShipsByType(ShipType.Passenger))
                .Returns(ships);

            // Act
            var result = _ctrl.GetShipsByType(ShipType.Passenger);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedShips = Assert.IsType<List<Ship>>(okResult.Value);
            Assert.Equal(2, returnedShips.Count);
        }

        [Fact]
        public void GetShipsByType_ReturnsNotFound_WhenNoShipsOfType()
        {
            // Arrange
            _regMock
                .Setup(r => r.GetShipsByType(ShipType.Passenger))
                .Returns(new List<Ship>());

            // Act
            var result = _ctrl.GetShipsByType(ShipType.Passenger);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
            Assert.Equal("No ships of type 'Passenger' found.", notFound.Value);
        }
    }
}
