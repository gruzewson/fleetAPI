using System;
using System.Collections.Generic;
using System.Linq;
using FleetAPI.Exceptions;
using FleetAPI.Models.Ships;

namespace FleetAPI.Data
{
    public class ShipRepository : IShipRepository
    {
        private readonly HashSet<Ship> _ships = new();

        public void AddShip(Ship ship)
        {
            if (ship == null)
                throw new ArgumentNullException(nameof(ship));

            if (!_ships.Add(ship)) // HashSet.Add returns false if the item already exists
                throw new ShipAlreadyExistsException(ship.ImoNumber);
        }

        public Ship RemoveShip(string imo)
        {
            var ship = _ships.FirstOrDefault(s => s.ImoNumber == imo)
                       ?? throw new ShipNotFoundException(imo);

            _ships.Remove(ship);
            return ship;
        }

        public Ship GetShipByImo(string imo)
        {
            return _ships.FirstOrDefault(s => s.ImoNumber == imo)
                   ?? throw new ShipNotFoundException(imo);
        }

        public PassengerShip? GetPassengerShipByImo(string imo) //TODO test
            => _ships
                .OfType<PassengerShip>()                   
                .FirstOrDefault(s => s.ImoNumber == imo);  

        public TankerShip? GetTankerShipByImo(string imo)
            => _ships
                .OfType<TankerShip>()                     
                .FirstOrDefault(s => s.ImoNumber == imo);

        public bool Exists(string imo) => _ships.Any(s => s.ImoNumber == imo);

        public IEnumerable<Ship> GetAllShips() => _ships;

        public IEnumerable<Ship> GetShipsByType(ShipType type)
        {
            var ships = _ships.Where(s => s.ShipType == type).ToList();

            // Return an empty collection if no ships are found
            return ships;
        } 

    }
}