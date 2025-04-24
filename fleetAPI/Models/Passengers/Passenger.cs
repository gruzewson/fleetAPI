namespace FleetAPI.Models.Passengers
{
    public class Passenger
    {
        public int PassengerID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        
        public Passenger(int passengerID, string name, string surname)
        {
            PassengerID = passengerID;
            Name = name;
            Surname = surname;
        }
    }
}