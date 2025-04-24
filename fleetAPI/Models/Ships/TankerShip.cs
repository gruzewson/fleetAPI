using FleetAPI.Models.Tanks;
using FleetAPI.Exceptions;

namespace FleetAPI.Models.Ships
{
    public class TankerShip : Ship
    {
        public int TanksNumber { get; set; }
        private List<Tank> Tanks { get; } = new List<Tank>();

        public TankerShip(string imo, string name, double length, double width, IEnumerable<Tank> tanks)
            : base(imo, name, length, width, ShipType.Tanker)
        {
            Tanks = tanks.ToList();
            TanksNumber = tanks.Count();
        }

        public void FillTank(Guid tankId, int liters)
        {
            var tank = Tanks.FirstOrDefault(p => p.TankID == tankId)
                    ?? throw new TankDoesntExistException(tankId);

            tank.FillTank(liters);
        }

        public void EmptyTank(Guid tankId)
        {
            var tank = Tanks.FirstOrDefault(p => p.TankID == tankId)
                    ?? throw new TankDoesntExistException(tankId);
            
            tank.FullyEmptyTank();
        }


    }
}