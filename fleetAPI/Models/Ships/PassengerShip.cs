using FleetAPI.Models.Passengers;
using FleetAPI.Exceptions;

namespace FleetAPI.Models.Ships
{
    public class PassengerShip : Ship
    {
        public int PassengerCount { get; set; }
        public List<Passenger> Passengers { get; set; }

        public PassengerShip(string imo, string name, double length,
            double width, IEnumerable<Passenger> passengers)
            : base(imo, name, length, width, ShipType.Passenger)
        {
            Passengers = passengers.Select(p => new Passenger
            (
                p.Name,
                p.Surname
            )).ToList();
            foreach (var passenger in Passengers)
            {
                passenger.PassengerId = Guid.NewGuid();
            }

            PassengerCount = Passengers.Count;
        }
        
        public void AddPassenger(string name, string surname)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidPassengerDataException("Name is required.");
            if (string.IsNullOrWhiteSpace(surname))
                throw new InvalidPassengerDataException("Surname is required.");

            var passenger = new Passenger
            (
                name,
                surname
            );
            passenger.PassengerId = Guid.NewGuid();

            Passengers.Add(passenger);
            PassengerCount++;
        }

        public void UpdatePassengerInfo(Guid passengerId, string newName, string newSurname)
        {
            var passenger = Passengers.FirstOrDefault(p => p.PassengerId == passengerId)
                    ?? throw new PassengerNotFoundException(passengerId);

            if (string.IsNullOrWhiteSpace(newName))
                throw new InvalidPassengerDataException("Name is required.");
            if (string.IsNullOrWhiteSpace(newSurname))
                throw new InvalidPassengerDataException("Surname is required.");

            passenger.Name = newName;
            passenger.Surname = newSurname;
        }
        
        public void RemovePassengerById(Guid passengerId)
        {
            var passenger = Passengers.FirstOrDefault(p => p.PassengerId == passengerId)
                            ?? throw new PassengerNotFoundException(passengerId);
            
            Passengers.Remove(passenger);
            PassengerCount--;
        }
        
        public Passenger GetPassengerById(Guid passengerId)
        {
            var passenger = Passengers
            .FirstOrDefault(p => p.PassengerId == passengerId)
                            ?? throw new PassengerNotFoundException(passengerId);

            return passenger;
        }
        
        public IEnumerable<Passenger> GetAllPassengers()
        {
            return Passengers;
        }
    }
}
