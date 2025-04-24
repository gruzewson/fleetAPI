using FleetAPI.Models.Passengers;
using FleetAPI.Models.Tanks;

namespace FleetAPI.Models.Ships
{
    public class ShipDto
    {
        public int ImoNumber { get; set; }
        public string Name     { get; set; } = null!;
        public float Length    { get; set; }
        public float Width     { get; set; }
        public IEnumerable<Passenger>? Passengers { get; set; }
        public IEnumerable<Tank>?      Tanks      { get; set; }
    }
}


