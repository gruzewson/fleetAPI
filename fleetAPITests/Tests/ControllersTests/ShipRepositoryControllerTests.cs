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
using FleetAPI.Controllers;
using FleetAPI.Exceptions;

// TODO every int, imo should be constant 

namespace FleetAPI.Tests.Controllers
{
    public class ShipRepositoryControllerTests
    {
        private readonly Mock<IShipRepository>              _repoMock;
        private readonly Mock<IShipFactory<PassengerShip>> _factoryMock;

        private readonly ShipRepositoryController _ctrl;

        public ShipRepositoryControllerTests()
        {
            _repoMock    = new Mock<IShipRepository>();
            _factoryMock = new Mock<IShipFactory<PassengerShip>>();
            _ctrl = new ShipRepositoryController(
                _repoMock.Object,
                _factoryMock.Object
            );
        }

        [Fact]
        public void Create_ReturnsCreated_WhenRequestValid()
        {
            // Arrange
            var dto = new ShipDto {
                ImoNumber = "IMO9074729",
                Name      = "Test",
                Length    = 100,
                Width     = 20,
                Passengers= new List<Passenger>(),
                Tanks     = null
            };

            var ship = new PassengerShip(
                "IMO9074729", "Test", 100, 20, new List<Passenger>()
            );

            _factoryMock
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
            Assert.Equal(201, created.StatusCode);
            Assert.Same(ship, created.Value);
            _repoMock.Verify(r => r.AddShip(ship), Times.Once);
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
                Passengers = new List<Passenger>(),
                Tanks      = null
            };

            // Simulate that the ship already exists
            _repoMock
                .Setup(r => r.Exists(dto.ImoNumber))
                .Returns(true);

            // Act
            var result = _ctrl.Create(dto);

            // Assert
            // 1) We get a 409 Conflict
            var conflict = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(409, conflict.StatusCode);

            // 2) The repository never had AddShip called
            _repoMock.Verify(r => r.AddShip(It.IsAny<PassengerShip>()), Times.Never);
        }

        [Fact]
        public void GetAllShips_ReturnsOk_WithListOfShips()
        {
            //Arrange
            var ships = new List<PassengerShip>
            {
                new PassengerShip("IMO9074729", "Test Ship 1", 100, 20, new List<Passenger>()),
                new PassengerShip("IMO9224764", "Test Ship 2", 150, 30, new List<Passenger>())
            };
            _repoMock
                .Setup(r => r.GetAllShips())
                .Returns(ships);
            //Act
            var result = _ctrl.GetAllShips();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedShips = Assert.IsType<List<PassengerShip>>(okResult.Value);
            Assert.Equal(2, returnedShips.Count);
            Assert.Equal("IMO9074729", returnedShips[0].ImoNumber);
            Assert.Equal("IMO9224764", returnedShips[1].ImoNumber);
        }

        [Fact]
        public void GetAllShips_ReturnsOk_WithEmptyList()
        {
            // Arrange
            var ships = new List<PassengerShip>();
            _repoMock
                .Setup(r => r.GetAllShips())
                .Returns(ships);

            // Act
            var result = _ctrl.GetAllShips();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedShips = Assert.IsType<List<PassengerShip>>(okResult.Value);
            Assert.Empty(returnedShips);
        }

        [Fact]
        public void GetShipByImo_ReturnsOk_WhenShipExists()
        {
            // Arrange
            var ship = new PassengerShip("IMO9074729", "Test Ship", 100, 20, new List<Passenger>());
            _repoMock
                .Setup(r => r.GetShipByImo("IMO9074729"))
                .Returns(ship);

            // Act
            var result = _ctrl.GetShipByImo("IMO9074729");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedShip = Assert.IsType<PassengerShip>(okResult.Value);
            Assert.Equal(ship, returnedShip);
        }

        [Fact]
        public void GetShipByImo_ReturnsNotFound_WhenShipDoesNotExist()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetShipByImo("IMO9074729"))
                .Throws(new ShipNotFoundException("IMO9074729"));

            // Act
            var result = _ctrl.GetShipByImo("IMO9074729");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Ship with IMO 'IMO9074729' not found.", notFoundResult.Value);
        }

        [Fact]
        public void RemoveShip_ReturnsNoContent_WhenShipRemoved()
        {
            // Arrange
            var ship = new PassengerShip("IMO9074729", "Test Ship", 100, 20, new List<Passenger>());
            _repoMock
                .Setup(r => r.RemoveShip("IMO9074729"))
                .Returns(ship);

            // Act
            var result = _ctrl.RemoveShip("IMO9074729");

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public void RemoveShip_ReturnsNotFound_WhenShipDoesNotExist()
        {
            // Arrange
            _repoMock
                .Setup(r => r.RemoveShip("IMO9074729"))
                .Throws(new ShipNotFoundException("IMO9074729"));

            // Act
            var result = _ctrl.RemoveShip("IMO9074729");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Ship with IMO 'IMO9074729' not found.", notFoundResult.Value);
        } 

        [Fact]
        public void GetShipsByType_ShouldReturnShipsOfType()
        {
            // Arrange
            var ships = new List<PassengerShip>
            {
                new PassengerShip("IMO9074729", "Test Ship 1", 100, 20, new List<Passenger>()),
                new PassengerShip("IMO9224764", "Test Ship 2", 150, 30, new List<Passenger>())
            };
            _repoMock
                .Setup(r => r.GetShipsByType(ShipType.Passenger))
                .Returns(ships);

            // Act
            var result = _ctrl.GetShipsByType(ShipType.Passenger);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedShips = Assert.IsType<List<PassengerShip>>(okResult.Value);
            Assert.Equal(2, returnedShips.Count);
        }

        [Fact]
        public void GetShipsByType_ReturnsNotFound_WhenNoShipsOfType()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetShipsByType(ShipType.Passenger))
                .Returns(new List<PassengerShip>());

            // Act
            var result = _ctrl.GetShipsByType(ShipType.Passenger);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("No ships of type 'Passenger' found.", notFoundResult.Value);
        }
    }
}
