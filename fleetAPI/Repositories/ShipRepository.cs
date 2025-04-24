using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetAPI.Models;
using FleetAPI.Factories;

namespace FleetAPI.Repositories
{
    public class ShipRepository : IShipRepository
    {
        private readonly List<Ship> _ships = new();

        public Task<IEnumerable<Ship>> GetAllAsync()
        {
            return Task.FromResult(_ships.AsEnumerable());
        }

        public Task<Ship?> GetByImoAsync(int imo)
        {
            var ship = _ships.FirstOrDefault(s => s.ImoNumber == imo);
            return Task.FromResult(ship);
        }

        public Task AddAsync(Ship ship)
        {
            _ships.Add(ship);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int imo)
        {
            bool exists = _ships.Any(s => s.ImoNumber == imo);
            return Task.FromResult(exists);
        }

        public Task SaveChangesAsync()
        {
            // In-memory, so nothing to persist — no-op for now.
            return Task.CompletedTask;
        }
    }
    
}