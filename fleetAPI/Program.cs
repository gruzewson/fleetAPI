using FleetAPI.Classes;

namespace fleetAPI;
class Program
{
    private static void Main(string[] args)
    {
        PassengerShip ship = new PassengerShip
        {
            MaxPassengers = 2,
            CurrentPassengers = 0,
            imoNumber = 1234567,
            shipName = "Test Ship",
            length = 300,
            width = 50,
            shipType = "Passenger"
        };

        // Add passengers
        ship.AddPassenger("John", "Doe");
        ship.AddPassenger("Jane", "Smith");

        ship.RemovePassengerById(1); // Assuming 1 is the ID of the passenger to be removed

        ship.DisplayPassengers();

        ship.AddPassenger("Alice", "Johnson");

        ship.DisplayPassengers();
    }
}
