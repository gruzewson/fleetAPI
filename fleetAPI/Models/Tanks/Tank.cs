using FleetAPI.Exceptions;

namespace FleetAPI.Models.Tanks
{
    public class Tank
    {
        public double Capacity { get; set; }
        public double CurrentLitersNumber { get; set; }
        public FuelType FuelType { get; set; }
        public Guid TankID { get; set; }

        public Tank(FuelType fuelType, double capacity)
        {
            if (capacity <= 0)
                throw new InvalidTankCapacityException();

            TankID = Guid.NewGuid();
            FuelType = fuelType;
            Capacity = capacity;
            CurrentLitersNumber = 0;
        }

        public void FillTank(int liters)
        {
            if (liters <= 0)
                throw new InvalidTankFillAmountException("Liters to fill must be greater than zero.");

            if (CurrentLitersNumber + liters > Capacity)
                throw new TankOverfillException(TankID, liters, Capacity);

            CurrentLitersNumber += liters;
        }

        public void FullyEmptyTank()
        {
            if (CurrentLitersNumber == 0)
                throw new TankAlreadyEmptyException(TankID);

            CurrentLitersNumber = 0;
        }
    }
}
