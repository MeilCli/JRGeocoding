using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JRGeocoding.Core
{
    public class Address
    {
        [DataMember(Name = "prefecture")]
#pragma warning disable CS8618 
        public string Prefecture { get; set; }
#pragma warning restore CS8618

        [DataMember(Name = "city")]
        public string? City { get; set; }

        [DataMember(Name = "ward")]
        public string? Ward { get; set; }

        [DataMember(Name = "town")]
        public string? Town { get; set; }

        [DataMember(Name = "polygons")]
#pragma warning disable CS8618
        public IReadOnlyList<Polygon> Polygons { get; set; }
#pragma warning restore CS8618
    }
}
