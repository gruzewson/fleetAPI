using FleetAPI.Models.Passengers;
using FleetAPI.Models.Tanks;

namespace FleetAPI.Models.Ships
{
    public class ShipDto
    {
        public string ImoNumber { get; set; }
        public string Name     { get; set; } = null!;
        public double Length    { get; set; }
        public double Width     { get; set; }
        public IEnumerable<Passenger>? Passengers { get; set; }
        public IEnumerable<Tank>?      Tanks      { get; set; }
    }
}


