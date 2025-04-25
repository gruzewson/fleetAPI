namespace FleetAPI.Models.Passengers
{
    public class Passenger
    {
        public int PassengerID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        
        public Passenger(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }
    }
}