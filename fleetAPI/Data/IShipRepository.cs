using System.Collections.Generic;
using FleetAPI.Models.Ships;

namespace FleetAPI.Data
{
    public interface IShipRepository
    {
        void AddShip(Ship ship);
        Ship RemoveShip(string imo);
        Ship GetShipByImo(string imo);

        PassengerShip? GetPassengerShipByImo(string imo);
        TankerShip?   GetTankerShipByImo(string imo);
        bool Exists(string imo);
        IEnumerable<Ship> GetAllShips();
        IEnumerable<Ship> GetShipsByType(ShipType type);
    }
}