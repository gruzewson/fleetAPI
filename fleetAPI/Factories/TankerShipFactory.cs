 using FleetAPI.Models;

namespace FleetAPI.Factories
{
    public class TankerShipFactory : IShipFactory<TankerShip>
    {
        public TankerShip Create(int imo, string name, float length, float width,
                                IEnumerable<Passenger>? passengers = null,
                                IEnumerable<Tank>?       tanks      = null)
        {
            return new TankerShip(
                imo:    imo,
                name:   name,
                length: length,
                width:  width,
                tanks:  tanks ?? Array.Empty<Tank>()
            );
        }
    }
}