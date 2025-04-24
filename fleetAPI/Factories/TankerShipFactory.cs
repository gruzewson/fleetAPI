using FleetAPI.Models.Ships;
using FleetAPI.Models.Passengers;
using FleetAPI.Models.Tanks;

namespace FleetAPI.Factories
{
    public class TankerShipFactory : IShipFactory<TankerShip>
    {
        public TankerShip Create(string imo, string name, double length, double width,
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