class Passenger
{
    public required int PassengerID { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }

    public Passenger() { }
    
    public Passenger(int passengerID, string name, string surname)
    {
        PassengerID = passengerID;
        Name = name;
        Surname = surname;
    }
}