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
        private readonly Mock<IShipRegister>              _regMock;
        private readonly PassengerShipController            _ctrl;
        private const string TestImo = "IMO9074729";

        public PassengerShipControllerTests()
        {
            _regMock    = new Mock<IShipRegister>();
            _ctrl = new PassengerShipController(
                _regMock.Object
            );
        }

        [Fact]
        public void AddPassenger_ShouldAdd_WhenValid()
        {
            var ship = new PassengerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Passenger>());
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.AddPassenger(TestImo, new PassengerDto("John", "Kowal"));

            Assert.IsType<NoContentResult>(result);
            Assert.Single(ship.Passengers);
            Assert.Equal("John", ship.Passengers[0].Name);
            Assert.Equal("Kowal", ship.Passengers[0].Surname);
        }

        [Fact]
        public void AddPassenger_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns((PassengerShip)null!);

            var result = _ctrl.AddPassenger(TestImo, new PassengerDto("John", "Kowal"));

            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData("", "Kowal", "Name is required.")]
        [InlineData("John", "", "Surname is required.")]
        public void AddPassenger_ShouldReturnBadRequest_WhenInvalidData(string name, string surname, string expectedError)
        {
            var ship = new PassengerShip(TestImo, "Test Ship", 100.0, 50.0, []);
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.AddPassenger(TestImo, new PassengerDto(name, surname));

            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.Equal(expectedError, badRequestResult.Value);
        }

        [Fact]
        public void RemovePassenger_ShouldRemove_WhenValid()
        {
            var ship = new PassengerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Passenger>
            {
                new Passenger("John", "Kowal")
            });
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.RemovePassenger(TestImo, ship.Passengers[0].PassengerId);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(ship.Passengers);
        }

        [Fact]
        public void RemovePassenger_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns((PassengerShip)null!);

            var result = _ctrl.RemovePassenger(TestImo, Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public void RemovePassenger_ShouldReturnNotFound_WhenPassengerDoesNotExist()
        {
            var ship = new PassengerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Passenger>());
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns(ship);
            var guid = Guid.NewGuid();
            var result = _ctrl.RemovePassenger(TestImo, guid);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Passenger with ID {guid} not found.", ((NotFoundObjectResult)result).Value);
        }

        [Fact]
        public void UpdatePassengerInfo_ShouldUpdate_WhenValid()
        {
            var ship = new PassengerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Passenger>
            {
                new Passenger("John", "Kowal")
            });
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns(ship);

            var passengerDto = new PassengerDto("Janek", "Smith");
            var result = _ctrl.UpdatePassengerInfo(TestImo, ship.Passengers[0].PassengerId, passengerDto);

            Assert.IsType<NoContentResult>(result);
            Assert.Single(ship.Passengers);
            Assert.Equal("Janek", ship.Passengers[0].Name);
            Assert.Equal("Smith", ship.Passengers[0].Surname);
        }

        [Fact]
        public void UpdatePassengerInfo_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns((PassengerShip)null!);

            var passengerDto = new PassengerDto("Janek", "Smith");
            var result = _ctrl.UpdatePassengerInfo(TestImo, Guid.NewGuid(), passengerDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData("", "Kowal", "Name is required.")]
        [InlineData("John", "", "Surname is required.")]
        public void UpdatePassengerInfo_ShouldReturnBadRequest_WhenInvalidData(string name, string surname, string expectedError)
        {
            var ship = new PassengerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Passenger>
            {
                new Passenger("John", "Kowal")
            });
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns(ship);

            var passengerDto = new PassengerDto(name, surname);
            var result = _ctrl.UpdatePassengerInfo(TestImo, ship.Passengers[0].PassengerId, passengerDto);

            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.Equal(expectedError, badRequestResult.Value);
        }

        [Fact]
        public void UpdatePassengerInfo_ShouldReturnNotFound_WhenPassengerDoesNotExist()
        {
            var ship = new PassengerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Passenger>());
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns(ship);

            var guid = Guid.NewGuid();
            var passengerDto = new PassengerDto("Janek", "Smith");
            var result = _ctrl.UpdatePassengerInfo(TestImo, guid, passengerDto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Passenger with ID {guid} not found.", notFound.Value);
        }

        [Fact]
        public void GetPassengerById_ShouldReturnPassenger_WhenValid()
        { ;
            var ship = new PassengerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Passenger>
            {
                new Passenger("John", "Kowal")
            });
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.GetPassengerById(TestImo, ship.Passengers[0].PassengerId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var passenger = Assert.IsType<Passenger>(okResult.Value);
            Assert.Equal("John", passenger.Name);
            Assert.Equal("Kowal", passenger.Surname);
        }

        [Fact]
        public void GetPassengerById_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns((PassengerShip)null!);

            var result = _ctrl.GetPassengerById(TestImo, Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetPassengerById_ShouldReturnNotFound_WhenPassengerDoesNotExist()
        {
            var ship = new PassengerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Passenger>());
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns(ship);
            var guid = Guid.NewGuid();
            var result = _ctrl.GetPassengerById(TestImo, guid);

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Passenger with ID {guid} not found.", ((NotFoundObjectResult)result).Value);
        }
        
        [Fact]
        public void GetAllPassengers_ShouldReturnAllPassengers_WhenValid()
        {
            var ship = new PassengerShip(TestImo, "Test Ship", 100.0, 50.0, new List<Passenger>
            {
                new Passenger("John", "Kowal"),
                new Passenger("Jane", "Doe")
            });
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns(ship);

            var result = _ctrl.GetAllPassengers(TestImo);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var passengers = Assert.IsType<List<Passenger>>(okResult.Value);
            Assert.Equal(2, passengers.Count);
        }
        
        [Fact]
        public void GetAllPassengers_ShouldReturnNotFound_WhenShipDoesNotExist()
        {
            _regMock.Setup(r => r.GetPassengerShipByImo(TestImo)).Returns((PassengerShip)null!);

            var result = _ctrl.GetAllPassengers(TestImo);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
