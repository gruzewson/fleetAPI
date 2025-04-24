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