using FleetAPI.Models.Tanks;
using FleetAPI.Exceptions;

namespace FleetAPI.Models.Ships
{
    public class TankerShip : Ship
    {
        public int TanksNumber { get; set; }
        public List<Tank> Tanks { get; }

        public TankerShip(string imo, string name, double length, double width, IEnumerable<Tank> tanks)
            : base(imo, name, length, width, ShipType.Tanker)
        {
            Tanks = tanks.ToList();
            TanksNumber = tanks.Count();
        }

        public void FillTank(Guid tankId, double liters)
        {
            var tank = Tanks.FirstOrDefault(p => p.TankId == tankId)
                    ?? throw new TankDoesntExistException(tankId);

            tank.FillTank(liters);
        }

        public void EmptyTank(Guid tankId)
        {
            var tank = Tanks.FirstOrDefault(p => p.TankId == tankId)
                    ?? throw new TankDoesntExistException(tankId);
            
            tank.FullyEmptyTank();
        }

        public Tank GetTank(Guid tankId)
        {
            var tank = Tanks.FirstOrDefault(p => p.TankId == tankId)
                    ?? throw new TankDoesntExistException(tankId);

            return tank;
        }
        
        public IEnumerable<Tank> GetAllTanks()
        {
            return Tanks;
        }
    }
}