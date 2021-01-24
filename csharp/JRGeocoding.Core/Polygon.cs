using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JRGeocoding.Core
{
    public class Polygon
    {
        [DataMember(Name = "locations")]
#pragma warning disable CS8618
        public IReadOnlyList<Location> Locations { get; set; }
#pragma warning restore CS8618

        public IEnumerable<Line> EnumerateLines()
        {
            if (Locations.Count == 0)
            {
                yield break;
            }

            for (int i = 0; i < Locations.Count; i++)
            {
                if (i < Locations.Count - 1)
                {
                    yield return new Line(Locations[i], Locations[i + 1]);
                }
                else
                {
                    yield return new Line(Locations[i], Locations[0]);
                }
            }
        }
    }
}
