
namespace MiddlewareLibrary.Models
{
    public enum Transmission 
    {
        Any,
        Automatic,
        Mechanic
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
    }
}
