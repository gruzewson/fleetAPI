using System.Collections.Generic;
using System.Threading.Tasks;
using FleetAPI.Models;

namespace FleetAPI.Repositories
{
    public interface IShipRepository
    {
        Task<IEnumerable<Ship>> GetAllAsync();
        Task<Ship?> GetByImoAsync(int imo);
        Task AddAsync(Ship ship);
        Task<bool> ExistsAsync(int imo);
        Task SaveChangesAsync();
    }
}