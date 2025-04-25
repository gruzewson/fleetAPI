using System;
using System.Collections.Generic;
using System.Linq;
using FleetAPI.Exceptions;
using FleetAPI.Models.Ships;

namespace FleetAPI.Data
{
    public class ShipRegister : IShipRegister
    {
        private readonly HashSet<Ship> _ships = [];

        public void AddShip(Ship ship)
        {
            ArgumentNullException.ThrowIfNull(ship);
            
            // HashSet.Add returns false if the item already exists
            if (!_ships.Add(ship)) 
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

        public PassengerShip? GetPassengerShipByImo(string imo)
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