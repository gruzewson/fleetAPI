class TankerShip : Ship
{
    public required int TanksNumber { get; set; }
    public List<Tank> Tanks { get; private set; } = new List<Tank>();

    // Fill a tank
    public void FillTank(int tankId, int liters)
    {
        var tank = Tanks.FirstOrDefault(t => t.TankID == tankId);
        if (tank != null)
        {
            if (tank.CurrentLitersNumber + liters <= tank.Capacity)
            {
                tank.CurrentLitersNumber += liters;
                Console.WriteLine($"Tank {tankId} filled with {liters} liters.");
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

    // Empty a tank
    public void EmptyTank(int tankId, int liters)
    {
        var tank = Tanks.FirstOrDefault(t => t.TankID == tankId);
        if (tank != null)
        {
            if (tank.CurrentLitersNumber - liters >= 0)
            {
                tank.CurrentLitersNumber -= liters;
                Console.WriteLine($"Tank {tankId} emptied by {liters} liters.");
            }
            else
            {
                Console.WriteLine("Cannot empty tank. Not enough fuel.");
            }
        }
        else
        {
            //exception
            Console.WriteLine("Tank not found.");
        }
    }


}