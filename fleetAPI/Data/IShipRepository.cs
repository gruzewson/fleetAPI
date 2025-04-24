using System.Collections.Generic;
using FleetAPI.Models.Ships;

namespace FleetAPI.Repositories
{
    public interface IShipRepository
    {
        void AddShip(Ship ship);
        void RemoveShip(string imo);
        Ship GetShipByImo(string imo);
        IEnumerable<Ship> GetAllShips();
        IEnumerable<Ship> GetShipsByType(ShipType type);
    }
}