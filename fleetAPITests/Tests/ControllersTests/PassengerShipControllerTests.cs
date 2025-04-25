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
    public class PassengerShipControllerTests
    {
        private readonly Mock<IShipRepository>              _repoMock;
        private readonly Mock<IShipFactory<PassengerShip>> _factoryMock;
        private readonly PassengerShipController            _ctrl;

        public PassengerShipControllerTests()
        {
            _repoMock    = new Mock<IShipRepository>();
            _factoryMock = new Mock<IShipFactory<PassengerShip>>();
            _ctrl = new PassengerShipController(
                _repoMock.Object,
                _factoryMock.Object
            );
        }

        [Fact]
        public void AddPassenger_ShouldAdd_WhenValid()
        {
            string imo = "IMO9074729";
            var ship = new PassengerShip(imo, "Test Ship", 100.0, 50.0, new List<Passenger>());
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns(ship);

            var result = _ctrl.AddPassenger(imo, new PassengerDto("John", "Kowal"));

            Assert.IsType<NoContentResult>(result);
            Assert.Single(ship.Passengers);
            Assert.Equal("John", ship.Passengers[0].Name);
            Assert.Equal("Kowal", ship.Passengers[0].Surname);
        }

        [Fact]
        public void AddPassenger_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            string imo = "IMO9074729";
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns((PassengerShip)null);

            var result = _ctrl.AddPassenger(imo, new PassengerDto("John", "Kowal"));

            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData("", "Kowal", "Name is required.")]
        [InlineData("John", "", "Surname is required.")]
        public void AddPassenger_ShouldReturnBadRequest_WhenInvalidData(string name, string surname, string expectedError)
        {
            string imo = "IMO9074729";
            var ship = new PassengerShip(imo, "Test Ship", 100.0, 50.0, Enumerable.Empty<Passenger>());
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns(ship);

            var result = _ctrl.AddPassenger(imo, new PassengerDto(name, surname));

            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.Equal(expectedError, badRequestResult.Value);
        }

        [Fact]
        public void RemovePassenger_ShouldRemove_WhenValid()
        {
            string imo = "IMO9074729";
            var ship = new PassengerShip(imo, "Test Ship", 100.0, 50.0, new List<Passenger>
            {
                new Passenger("John", "Kowal")
            });
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns(ship);

            var result = _ctrl.RemovePassenger(imo, 1);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(ship.Passengers);
        }

        [Fact]
        public void RemovePassenger_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            string imo = "IMO9074729";
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns((PassengerShip)null);

            var result = _ctrl.RemovePassenger(imo, 1);

            Assert.IsType<NotFoundResult>(result);
        }

        // TODO invalid ID

        [Fact]
        public void UpdatePassengerInfo_ShouldUpdate_WhenValid()
        {
            string imo = "IMO9074729";
            var ship = new PassengerShip(imo, "Test Ship", 100.0, 50.0, new List<Passenger>
            {
                new Passenger("John", "Kowal")
            });
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns(ship);

            var passengerDto = new PassengerDto("Janek", "Smith");
            var result = _ctrl.UpdatePassengerInfo(imo, 1, passengerDto);

            Assert.IsType<NoContentResult>(result);
            Assert.Single(ship.Passengers);
            Assert.Equal("Janek", ship.Passengers[0].Name);
            Assert.Equal("Smith", ship.Passengers[0].Surname);
        }

        [Fact]
        public void UpdatePassengerInfo_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            string imo = "IMO9074729";
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns((PassengerShip)null);

            var passengerDto = new PassengerDto("Janek", "Smith");
            var result = _ctrl.UpdatePassengerInfo(imo, 1, passengerDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData("", "Kowal", "Name is required.")]
        [InlineData("John", "", "Surname is required.")]
        public void UpdatePassengerInfo_ShouldReturnBadRequest_WhenInvalidData(string name, string surname, string expectedError)
        {
            string imo = "IMO9074729";
            var ship = new PassengerShip(imo, "Test Ship", 100.0, 50.0, new List<Passenger>
            {
                new Passenger("John", "Kowal")
            });
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns(ship);

            var passengerDto = new PassengerDto(name, surname);
            var result = _ctrl.UpdatePassengerInfo(imo, 1, passengerDto);

            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.Equal(expectedError, badRequestResult.Value);
        }

        [Fact]
        public void UpdatePassengerInfo_ShouldReturnNotFound_WhenPassengerDoesNotExist()
        {
            string imo = "IMO9074729";
            var ship = new PassengerShip(imo, "Test Ship", 100.0, 50.0, new List<Passenger>());
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns(ship);

            var passengerDto = new PassengerDto("Janek", "Smith");
            var result = _ctrl.UpdatePassengerInfo(imo, 1, passengerDto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Passenger with ID 1 not found.", notFound.Value);
        }

        [Fact]
        public void GetPassengerById_ShouldReturnPassenger_WhenValid()
        {
            string imo = "IMO9074729";
            var ship = new PassengerShip(imo, "Test Ship", 100.0, 50.0, new List<Passenger>
            {
                new Passenger("John", "Kowal")
            });
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns(ship);

            var result = _ctrl.GetPassengerById(imo, 1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var passenger = Assert.IsType<Passenger>(okResult.Value);
            Assert.Equal("John", passenger.Name);
            Assert.Equal("Kowal", passenger.Surname);
        }

        [Fact]
        public void GetPassengerById_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            string imo = "IMO9074729";
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns((PassengerShip)null);

            var result = _ctrl.GetPassengerById(imo, 1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetPassengerById_ShouldReturnNotFound_WhenPassengerDoesNotExist()
        {
            string imo = "IMO9074729";
            var ship = new PassengerShip(imo, "Test Ship", 100.0, 50.0, new List<Passenger>());
            _repoMock.Setup(r => r.GetPassengerShipByImo(imo)).Returns(ship);

            var result = _ctrl.GetPassengerById(imo, 1);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Passenger with ID 1 not found.", ((NotFoundObjectResult)result).Value);
        }
    }
}
