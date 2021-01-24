using System.Collections.Generic;

namespace JRGeocoding.Generator
{
    public record Polygon(IReadOnlyList<Location> Locations)
    {
    }
}
