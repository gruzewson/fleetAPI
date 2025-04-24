using FleetAPI.Models.Ships;
using FleetAPI.Models.Passengers;
using FleetAPI.Models.Tanks;

namespace FleetAPI.Factories
{
    public interface IShipFactory<out T> where T : Ship
    {
        T Create(string imo, string name, double length, double width,
                    IEnumerable<Passenger>? passengers = null,
                    IEnumerable<Tank>?      tanks = null);
    }

}