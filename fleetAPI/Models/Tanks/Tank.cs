namespace FleetAPI.Models.Tanks
{
    public class Tank
    {
        public int Capacity { get; set; }
        public int CurrentLitersNumber { get; set; }
        public FuelType FuelType { get; set; }
        public  int TankID { get; set; }
    }

    public void FillTank(int liters, FuelType fuelType)
    {
        var tank = Tanks.FirstOrDefault(t => t.FuelType == fuelType);
        if (tank != null)
        {
            if (tank.CurrentLitersNumber + liters <= tank.Capacity)
            {
                tank.CurrentLitersNumber += liters;
                Console.WriteLine($"Tank {tank.TankID} filled with {liters} liters.");
            }
            else
            {
                Console.WriteLine("Cannot fill tank. Exceeds capacity.");
            }
        }
        else
        {
            //exception
            Console.WriteLine("Tank not found.");
        }
    }
}