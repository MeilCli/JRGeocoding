using System.Runtime.Serialization;

namespace JRGeocoding.Core
{
    public class Location
    {
        [DataMember(Name = "latitude")]
        public double Latitude { get; set; }

        [DataMember(Name = "longitude")]
        public double Longitude { get; set; }
    }
}
