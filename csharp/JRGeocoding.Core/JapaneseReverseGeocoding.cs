using System.Collections.Generic;
using System.Threading.Tasks;
using Utf8Json;

namespace JRGeocoding.Core
{
    public class JapaneseReverseGeocoding
    {
        private const int mapMagnification = 10;

        private readonly IContentReader contentReader;

        public JapaneseReverseGeocoding(IContentReader contentReader)
        {
            this.contentReader = contentReader;
        }

        public async Task<Address?> FindAddressAsync(double latitude, double longitude)
        {
            int magnifiedLatitude = (int)(latitude * mapMagnification);
            int magnifiedLongitude = (int)(longitude * mapMagnification);
            string fileName = $"{magnifiedLatitude}-{magnifiedLongitude}.json";
            string? content = await contentReader.ReadContentAsync(fileName);
            if (content is null)
            {
                return null;
            }
            var addresses = JsonSerializer.Deserialize<IReadOnlyList<Address>>(content);
            foreach (var address in addresses)
            {
                foreach (var polygon in address.Polygons)
                {
                    if (isInside(latitude, longitude, polygon))
                    {
                        return address;
                    }
                }
            }

            return null;
        }

        // Winding Number Algorithm
        // ref: https://www.nttpc.co.jp/technology/number_algorithm.html
        // ref: https://gist.github.com/vlasky/d0d1d97af30af3191fc214beaf379acc
        private bool isInside(double latitude, double longitude, Polygon polygon)
        {
            int wn = 0;

            foreach (Line line in polygon.EnumerateLines())
            {
                if (line.Location1.Latitude <= latitude)
                {
                    if (latitude < line.Location2.Latitude)
                    {
                        var left = (line.Location2.Longitude - line.Location1.Longitude) * (latitude - line.Location1.Latitude);
                        var right = (longitude - line.Location1.Longitude) * (line.Location2.Latitude - line.Location1.Latitude);
                        if (0 < left - right)
                        {
                            wn += 1;
                        }
                    }
                }
                else
                {
                    if (line.Location2.Latitude <= latitude)
                    {
                        var left = (line.Location2.Longitude - line.Location1.Longitude) * (latitude - line.Location1.Latitude);
                        var right = (longitude - line.Location1.Longitude) * (line.Location2.Latitude - line.Location1.Latitude);
                        if (left - right < 0)
                        {
                            wn -= 1;
                        }
                    }
                }
            }

            return 1 <= wn;
        }
    }
}
