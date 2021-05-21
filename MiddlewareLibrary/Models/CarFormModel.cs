
namespace MiddlewareLibrary.Models
{
    public enum Transmission 
    {
        Any,
        Automatic,
        Mechanic
    }

    public enum Engine
    {
        Any,
        Gasoline, 
        Diesel,
        Hybrid,
        Electric
    }

    public enum Gear
    {
        Any,
        Forward,
        Back,
        All 
    }

    public enum SteeringWheel
    {
        Any,
        Left,
        Right
    }

    public enum CarBody 
    {
        Any,
        Sedan,
        Hatchback,
        Coupe,
        Pickup,
        Convertible
    }

    public class CarFormModel
    {
        public string Company { get; set; }

        public string Model { get; set; }

        public int? FromMileage { get; set; }

        public int? ToMileage { get; set; }

        public int? FromEnginePower { get; set; }

        public int? ToEnginePower { get; set; }

        public double? FromEngineVolume { get; set; }

        public double? ToEngineVolume { get; set; }

        public int? FromYear { get; set; }

        public int? ToYear { get; set; }

        public int? FromPrice { get; set; }

        public int? ToPrice { get; set; }

        public Transmission Transmission { get; set; }

        public Engine Engine { get; set; }

        public Gear Gear { get; set; }

        public SteeringWheel SteeringWheel { get; set; }

        public CarBody CarBody { get; set; }
    }
}
