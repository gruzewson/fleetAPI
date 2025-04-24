using FleetAPI.Models.Ships;
using FleetAPI.Models.Passengers;
using FleetAPI.Models.Tanks;

namespace FleetAPI.Factories
{
    public class PassengerShipFactory : IShipFactory<PassengerShip>
    {
        public PassengerShip Create(string imo, string name, double length, double width,
                                    IEnumerable<Passenger>? passengers = null,
                                    IEnumerable<Tank>?      tanks = null)
        {
            return new PassengerShip(
                imo, 
                name, 
                length, 
                width, 
                passengers ?? Enumerable.Empty<Passenger>());
        }
    }
}

