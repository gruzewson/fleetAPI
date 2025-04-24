using FleetAPI.Models.Passengers;
using FleetAPI.Exceptions;

namespace FleetAPI.Models.Ships
{
    public class PassengerShip : Ship
    {
        public int PassengerCount { get; set; }
        public List<Passenger> Passengers { get; set; }
        public int CurrentPassengerID { get; set; } = 0;

        public PassengerShip(string imo, string name, double length,
            double width, IEnumerable<Passenger> passengers)
            : base(imo, name, length, width, ShipType.Passenger)
        {
            Passengers = passengers.Select(p => new Passenger
            (
                ++CurrentPassengerID,
                p.Name,
                p.Surname
            )).ToList();

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
                CurrentPassengerID + 1,
                name,
                surname
            );

            Passengers.Add(passenger);
            PassengerCount++;
            CurrentPassengerID++;
        }

        public void UpdatePassengerInfo(int passengerId, string newName, string newSurname)
        {
            var passenger = Passengers.FirstOrDefault(p => p.PassengerID == passengerId)
                    ?? throw new PassengerNotFoundException(passengerId);

            if (string.IsNullOrWhiteSpace(newName))
                throw new InvalidPassengerDataException("Name is required.");
            if (string.IsNullOrWhiteSpace(newSurname))
                throw new InvalidPassengerDataException("Surname is required.");

            passenger.Name = newName;
            passenger.Surname = newSurname;
        }
        
        public void RemovePassengerById(int passengerId)
        {
            var passenger = Passengers.FirstOrDefault(p => p.PassengerID == passengerId)
                            ?? throw new PassengerNotFoundException(passengerId);
            
            Passengers.Remove(passenger);
            PassengerCount--;
        }
        
        public Passenger GetPassengerById(int passengerId)
        {
            var passenger = Passengers
            .FirstOrDefault(p => p.PassengerID == passengerId)
                            ?? throw new PassengerNotFoundException(passengerId);

            return passenger;
        }

        public override string ToString()
        {
            //list all passengers
            string passengerList = string.Join(", ", Passengers.Select(p => $"{p.Name} {p.Surname}"));
            return $"Passenger Ship: {ShipName} (IMO: {ImoNumber}), Length: {Length}, Width: {Width}, " +
                   $"Passenger Count: {PassengerCount}, Passengers: [{passengerList}]";
        }
    }
}
