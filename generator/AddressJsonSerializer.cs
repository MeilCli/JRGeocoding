using System.Collections.Generic;
using SpanJson;

namespace JRGeocoding.Generator
{
    public static class AddressJsonSerializer
    {
        public static byte[] Serialize(IEnumerable<Address> addresses)
        {
            using var writer = new JsonWriter<byte>();

            writer.WriteBeginArray();
            bool firstAddress = true;
            foreach (var address in addresses)
            {
                if (firstAddress is false)
                {
                    writer.WriteValueSeparator();
                }
                firstAddress = false;

                writer.WriteBeginObject();

                writer.WriteName("prefecture");
                writer.WriteDoubleQuote();
                writer.WriteUtf8Verbatim(address.Prefecture.Span);
                writer.WriteDoubleQuote();

                writer.WriteValueSeparator();

                writer.WriteName("city");
                if (address.City.Length is not 0)
                {
                    writer.WriteDoubleQuote();
                    writer.WriteUtf8Verbatim(address.City.Span);
                    writer.WriteDoubleQuote();
                }
                else
                {
                    writer.WriteNull();
                }

                writer.WriteValueSeparator();

                writer.WriteName("ward");
                if (address.Ward.Length is not 0)
                {
                    writer.WriteDoubleQuote();
                    writer.WriteUtf8Verbatim(address.Ward.Span);
                    writer.WriteDoubleQuote();
                }
                else
                {
                    writer.WriteNull();
                }

                writer.WriteValueSeparator();

                writer.WriteName("town");
                if (address.Town.Length is not 0)
                {
                    writer.WriteDoubleQuote();
                    writer.WriteUtf8Verbatim(address.Town.Span);
                    writer.WriteDoubleQuote();
                }
                else
                {
                    writer.WriteNull();
                }

                writer.WriteValueSeparator();

                writer.WriteName("polygons");
                writer.WriteBeginArray();
                bool firstPolygon = true;
                foreach (var polygon in address.Polygons)
                {
                    if (firstPolygon is false)
                    {
                        writer.WriteValueSeparator();
                    }
                    firstPolygon = false;

                    writer.WriteBeginObject();

                    writer.WriteName("locations");
                    writer.WriteBeginArray();
                    bool firstLocation = true;
                    foreach (var location in polygon.Locations)
                    {
                        if (firstLocation is false)
                        {
                            writer.WriteValueSeparator();
                        }
                        firstLocation = false;

                        writer.WriteBeginObject();

                        writer.WriteName("latitude");
                        writer.WriteDouble(location.Latitude);

                        writer.WriteValueSeparator();

                        writer.WriteName("longitude");
                        writer.WriteDouble(location.Longitude);

                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();

                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            return writer.ToByteArray();
        }
    }
}
