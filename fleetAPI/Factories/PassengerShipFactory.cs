using FleetAPI.Models;

namespace FleetAPI.Factories
{
    public class PassengerShipFactory : IShipFactory<PassengerShip>
    {
        public PassengerShip Create(int imo, string name, float length, float width,
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

