namespace FleetAPI.Models.Passengers
{
    public class PassengerDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public PassengerDto(string name, string surname)
        {
            Name     = name;
            Surname  = surname;
        }
    }
}