using FleetAPI.Models;

namespace FleetAPI.Factories
{
    public interface IShipFactory<out T> where T : Ship
    {
        T Create(int imo, string name, float length, float width,
                    IEnumerable<Passenger>? passengers = null,
                    IEnumerable<Tank>?      tanks = null);
    }

}