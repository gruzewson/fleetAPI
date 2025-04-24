using System;
using System.Collections.Generic;
using System.Linq;
using FleetAPI.Exceptions;
using FleetAPI.Models.Ships;

namespace FleetAPI.Repositories
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

        public void RemoveShip(string imo)
        {
            var ship = _ships.FirstOrDefault(s => s.ImoNumber == imo)
                       ?? throw new ShipNotFoundException(imo);

            _ships.Remove(ship);
        }

        public Ship GetShipByImo(string imo)
        {
            return _ships.FirstOrDefault(s => s.ImoNumber == imo)
                   ?? throw new ShipNotFoundException(imo);
        }

        public IEnumerable<Ship> GetAllShips() => _ships;

        public IEnumerable<Ship> GetShipsByType(ShipType type)
            => _ships.Where(s => s.ShipType == type);
    }
}
