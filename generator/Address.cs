using System;
using System.Collections.Generic;

namespace JRGeocoding.Generator
{
    /// <summary>
    /// City and Ward and Town can be empty
    /// </summary>
    public record Address(ReadOnlyMemory<byte> Prefecture, ReadOnlyMemory<byte> City, ReadOnlyMemory<byte> Ward, ReadOnlyMemory<byte> Town, IReadOnlyList<Polygon> Polygons)
    {
    }
}
