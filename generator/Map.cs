using System.Collections.Generic;
using System.Linq;

namespace JRGeocoding.Generator
{
    public class Map
    {
        public record MagnifiedLocation(int Latitude, int Longitude)
        {

        }

        private readonly int magnification;
        private Dictionary<MagnifiedLocation, List<Address>> map = new();

        public Map(int magnification)
        {
            this.magnification = magnification;
        }

        public void AddAddress(Address address)
        {
            foreach (var polygon in address.Polygons)
            {
                addAddressByPolygon(address, polygon);
            }
        }

        private void addAddressByPolygon(Address address, Polygon polygon)
        {
            int magnify(double value)
            {
                return (int)(value * magnification);
            }

            double minLatitude = polygon.Locations[0].Latitude;
            double maxLatitude = polygon.Locations[0].Latitude;
            double minLongitude = polygon.Locations[0].Longitude;
            double maxLongitude = polygon.Locations[0].Longitude;

            foreach (var location in polygon.Locations)
            {
                if (location.Latitude < minLatitude)
                {
                    minLatitude = location.Latitude;
                }
                if (maxLatitude < location.Latitude)
                {
                    maxLatitude = location.Latitude;
                }
                if (location.Longitude < minLongitude)
                {
                    minLongitude = location.Longitude;
                }
                if (maxLongitude < location.Longitude)
                {
                    maxLongitude = location.Longitude;
                }
            }

            int magnifiedMinLatitude = magnify(minLatitude);
            int magnifiedMaxLatitude = magnify(maxLatitude);
            int magnifiedMinLongitude = magnify(minLongitude);
            int magnifiedMaxLongitude = magnify(maxLongitude);

            for (int magnifiedLatitude = magnifiedMinLatitude; magnifiedLatitude <= magnifiedMaxLatitude; magnifiedLatitude++)
            {
                for (int magnifiedLongitude = magnifiedMinLongitude; magnifiedLongitude <= magnifiedMaxLongitude; magnifiedLongitude++)
                {
                    var magnifiedLocation = new MagnifiedLocation(magnifiedLatitude, magnifiedLongitude);
                    var mappedAddresses = map.GetValueOrDefault(magnifiedLocation) ?? new List<Address>();
                    if (mappedAddresses.Contains(address) is false)
                    {
                        mappedAddresses.Add(address);
                    }
                    map[magnifiedLocation] = mappedAddresses;
                }
            }
        }

        public IEnumerable<KeyValuePair<MagnifiedLocation, IReadOnlyList<Address>>> Values()
        {
            return map.Select(x => new KeyValuePair<MagnifiedLocation, IReadOnlyList<Address>>(x.Key, x.Value));
        }
    }
}
