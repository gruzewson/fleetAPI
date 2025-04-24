namespace FleetAPI.Exceptions;

// Passenger exceptions
public class InvalidPassengerDataException : Exception
{
    public InvalidPassengerDataException(string message)
        : base(message) { }
}

public class PassengerNotFoundException : Exception
{
    public PassengerNotFoundException(int id)
        : base($"Passenger with ID {id} not found.") { }
}

// Ship exceptions
public class InvalidImoNumberException : Exception
{
    public InvalidImoNumberException(string message)
        : base(message) { }
}

public class InvalidShipNameException : Exception
{
    public InvalidShipNameException()
        : base("Ship name cannot be null or empty.") { }
}

public class InvalidShipLengthException : Exception
{
    public InvalidShipLengthException(string message)
        : base(message) { }
}

public class InvalidShipWidthException : Exception
{
    public InvalidShipWidthException(string message)
        : base(message) { }
}

// Tank exceptions
public class InvalidTankFillAmountException : Exception
{
    public InvalidTankFillAmountException(string message)
        : base(message) { }
}

public class TankOverfillException : Exception
{
    public TankOverfillException(Guid tankId, double attemptLiters, double capacity)
        : base($"Cannot fill tank {tankId}. Adding {attemptLiters} liters would exceed the capacity of {capacity} liters.") { }
}

public class TankAlreadyEmptyException : Exception
{
    public TankAlreadyEmptyException(Guid tankId)
        : base($"Tank {tankId} is already empty.") { }
}

public class TankDoesntExistException : Exception
{
    public TankDoesntExistException(Guid tankId)
        : base($"Tank {tankId} does not exist.") { }
}

public class InvalidTankCapacityException : Exception
{
    public InvalidTankCapacityException()
        : base("Tank capacity must be greater than zero.") { }
}

// Ship Repository
public class ShipAlreadyExistsException : Exception
{
    public ShipAlreadyExistsException(string imo)
        : base($"Ship with IMO '{imo}' already exists.") { }
}

public class ShipNotFoundException : Exception
{
    public ShipNotFoundException(string imo)
        : base($"Ship with IMO '{imo}' not found.") { }
}