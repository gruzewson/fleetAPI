namespace FleetAPI.Classes;

class PassengerShip : Ship
{

    public required int MaxPassengers { get; set; }
    public int CurrentPassengers { get; set; }

    public int CurrentPassengerID { get; set; } = 0;

    // List to store passengers
    public List<Passenger> Passengers { get; private set; } = new List<Passenger>();

    // Method to display the list of passengers
    public void DisplayPassengers()
    {
        Console.WriteLine("Passenger List:");
        foreach (var passenger in Passengers)
        {
            Console.WriteLine($"Name: {passenger.Name}, Age: {passenger.Surname}, ID: {passenger.PassengerID}");
        }
    }

     // Method to add a passenger to the list
    public void AddPassenger(string name, string surname)
    {
        // Check if the ship is at full capacity
        if (CurrentPassengers < MaxPassengers)
        {
            var passenger = new Passenger
            {
                PassengerID = CurrentPassengerID + 1,
                Name = name,
                Surname = surname
            };

            Passengers.Add(passenger);
            CurrentPassengers++;
            CurrentPassengerID++;
            Console.WriteLine($"Passenger {passenger.Name} {passenger.Surname} added.");
        }
        else
        {
            Console.WriteLine("Cannot add passenger. Ship is at full capacity.");
        }
    }

    // Method to update a passenger's information
    public void UpdatePassenger(int passengerId, string newName, string newSurname)
    {
        var passenger = Passengers.FirstOrDefault(p => p.PassengerID == passengerId);
        if (passenger != null)
        {
            passenger.Name = newName;
            passenger.Surname = newSurname;
            Console.WriteLine($"Passenger {passengerId} updated.");
        }
        else
        {
            //exception
            Console.WriteLine("Passenger not found.");
        }
    }

    // Method to remove a passenger by their ID
    public void RemovePassengerById(int passengerId)
    {
        var passenger = Passengers.FirstOrDefault(p => p.PassengerID == passengerId);
        if (passenger != null)
        {
            Passengers.Remove(passenger);
            CurrentPassengers--;
            Console.WriteLine($"Passenger with ID {passengerId} removed.");
        }
        else
        {
            //exception
            Console.WriteLine("Passenger not found.");
        }
    }

    //Method to get a passenger by their ID
    public Passenger GetPassengerById(int passengerId)
    {
        var passenger = Passengers.FirstOrDefault(p => p.PassengerID == passengerId);
        if (passenger != null)
        {
            return passenger;
        }
        else
        {
            //exception
            Console.WriteLine("Passenger not found.");
            return null;
        }
    }
}
