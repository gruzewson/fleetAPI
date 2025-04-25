using FleetAPI.Exceptions;

namespace FleetAPI.Models.Ships
{
    
    public abstract class Ship
    {
        public string ImoNumber { get; set; }
        public string ShipName { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public ShipType ShipType { get; set; }
        private const int MINIMAL_LENGTH = 1;
        private const int MINIMAL_WIDTH = 1;

        protected Ship(string imo, string name, double length, double width, ShipType shipType)
        {
            ValidateImoNumber(imo);
            ValidateShipName(name);
            ValidateLength(length);
            ValidateWidth(width);
            
            ImoNumber = imo;
            ShipName = name;
            Length = length;
            Width = width;
            ShipType = shipType;
        }

        private void ValidateImoNumber(string imo)
        {
            if (imo is null)
                throw new InvalidImoNumberException("IMO number must not be null.");

            // Prefix must be "IMO"
            if (!imo.StartsWith("IMO", StringComparison.OrdinalIgnoreCase))
                throw new InvalidImoNumberException("IMO number must start with the letters \"IMO\".");

            // After "IMO" there must be exactly 7 digits
            string digits = imo.Substring(3);
            if (digits.Length != 7)
                throw new InvalidImoNumberException("After the \"IMO\" prefix, exactly 7 digits are required.");

            // All of those 7 must be digits 0–9
            if (!digits.All(char.IsDigit))
                throw new InvalidImoNumberException("The characters after \"IMO\" must all be digits.");

            // Compute the checksum on the first six digits
            int sum = 0;
            int weight = 7;
            for (int i = 0; i < 6; i++)
            {
                sum += (digits[i] - '0') * weight;
                weight--;
            }

            int expectedCheck = sum % 10;
            int actualCheck   = digits[6] - '0';

            if (actualCheck != expectedCheck)
                throw new InvalidImoNumberException(
                    $"Invalid IMO checksum: expected {expectedCheck}, but found {actualCheck}.");
        }

        private void ValidateShipName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidShipNameException();
        }

        private void ValidateLength(double length)
        {
            if (length <= MINIMAL_LENGTH)
                throw new InvalidShipLengthException($"Length must be greater than {MINIMAL_LENGTH}.");
        }

        private void ValidateWidth(double width)
        {
            if (width <= MINIMAL_WIDTH)
                throw new InvalidShipWidthException($"Width must be greater than {MINIMAL_WIDTH}.");
        }

        public override bool Equals(object? obj)
        {
            if (obj is Ship other)
            {
                return ImoNumber == other.ImoNumber;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ImoNumber.GetHashCode();
        }
    }
}