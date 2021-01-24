using System;
using System.Collections.Generic;
using System.Text;

namespace JRGeocoding.Generator
{
    public class GmlReader
    {
        private readonly byte[] source;
        private readonly byte lf = (byte)'\n';
        private readonly byte space = (byte)' ';
        private readonly byte[] startTag = Encoding.UTF8.GetBytes("<gml:featureMember>");
        private readonly byte[] endTag = Encoding.UTF8.GetBytes("</gml:featureMember>");
        private readonly byte[] prefectureStart = Encoding.UTF8.GetBytes("<fme:KEN_NAME>");
        private readonly byte[] prefectureEnd = Encoding.UTF8.GetBytes("</fme:KEN_NAME>");
        private readonly byte[] cityStart = Encoding.UTF8.GetBytes("<fme:GST_NAME>");
        private readonly byte[] cityEnd = Encoding.UTF8.GetBytes("</fme:GST_NAME>");
        private readonly byte[] wardStart = Encoding.UTF8.GetBytes("<fme:CSS_NAME>");
        private readonly byte[] wardEnd = Encoding.UTF8.GetBytes("</fme:CSS_NAME>");
        private readonly byte[] townStart = Encoding.UTF8.GetBytes("<fme:MOJI>");
        private readonly byte[] townEnd = Encoding.UTF8.GetBytes("</fme:MOJI>");
        private readonly byte[] positionsStart = Encoding.UTF8.GetBytes("<gml:posList>");
        private readonly byte[] positionsEnd = Encoding.UTF8.GetBytes("</gml:posList>");

        private int current = 0;

        public GmlReader(byte[] gml)
        {
            this.source = gml;
        }

        public Address? Read()
        {
            static void duplicateGurad(ReadOnlyMemory<byte> source, string name)
            {
                if (0 < source.Length)
                {
                    throw new Exception($"Duplicated {name}");
                }
            }

            bool foundStart = false;
            ReadOnlyMemory<byte> prefecture = Array.Empty<byte>();
            ReadOnlyMemory<byte> city = Array.Empty<byte>();
            ReadOnlyMemory<byte> ward = Array.Empty<byte>();
            ReadOnlyMemory<byte> town = Array.Empty<byte>();
            var polygons = new List<Polygon>();
            while (true)
            {
                ReadOnlyMemory<byte> line = nextLine();
                ReadOnlySpan<byte> lineSpan = line.Span;
                if (line.Length == 0)
                {
                    break;
                }
                if (lineSpan.SequenceEqual(startTag))
                {
                    foundStart = true;
                    continue;
                }
                if (lineSpan.SequenceEqual(endTag))
                {
                    break;
                }
                if (lineSpan.StartsWith(prefectureStart))
                {
                    duplicateGurad(prefecture, nameof(prefecture));
                    int index = lineSpan.IndexOf(prefectureEnd);
                    prefecture = line[prefectureStart.Length..index];
                }
                if (lineSpan.StartsWith(cityStart))
                {
                    duplicateGurad(city, nameof(city));
                    int index = lineSpan.IndexOf(cityEnd);
                    city = line[cityStart.Length..index];
                }
                if (lineSpan.StartsWith(wardStart))
                {
                    duplicateGurad(ward, nameof(ward));
                    int index = lineSpan.IndexOf(wardEnd);
                    ward = line[wardStart.Length..index];
                }
                if (lineSpan.StartsWith(townStart))
                {
                    duplicateGurad(town, nameof(town));
                    int index = lineSpan.IndexOf(townEnd);
                    town = line[townStart.Length..index];
                }
                if (lineSpan.StartsWith(positionsStart))
                {
                    int index = lineSpan.IndexOf(positionsEnd);
                    polygons.Add(parsePositions(line[positionsStart.Length..index]));
                }
            }

            if (foundStart is false)
            {
                return null;
            }
            if (prefecture.Length is 0)
            {
                throw new Exception("Not found prefecture");
            }
            // city can be empty
            // ward can be empty
            // town can be empty
            if (polygons.Count is 0)
            {
                throw new Exception("Found empty positions");
            }

            return new Address(prefecture, city, ward, town, polygons);
        }

        private ReadOnlyMemory<byte> nextLine()
        {
            ReadOnlyMemory<byte> currentSpan = source.AsMemory(current);
            int lineSeparator = currentSpan.Span.IndexOf(lf);
            if (lineSeparator < 0)
            {
                current += currentSpan.Length;
                return currentSpan;
            }
            ReadOnlyMemory<byte> lineSpan = currentSpan[0..lineSeparator];
            current += lineSpan.Length + 1;
            return lineSpan;
        }

        private Polygon parsePositions(ReadOnlyMemory<byte> positions)
        {
            int index = 0;

            ReadOnlyMemory<byte> nextValue(ReadOnlyMemory<byte> positions)
            {
                ReadOnlyMemory<byte> currentSpan = positions[index..];
                int spaceSeparator = currentSpan.Span.IndexOf(space);
                if (spaceSeparator < 0)
                {
                    index += currentSpan.Length;
                    return currentSpan;
                }
                ReadOnlyMemory<byte> valueSpan = currentSpan[0..spaceSeparator];
                index += valueSpan.Length + 1;
                return valueSpan;
            }

            var positionList = new List<double>();
            while (true)
            {
                ReadOnlyMemory<byte> value = nextValue(positions);
                if (value.Length == 0)
                {
                    break;
                }
                positionList.Add(double.Parse(Encoding.UTF8.GetString(value.Span)));
            }

            if (positionList.Count % 2 != 0)
            {
                throw new Exception("Illegal positions");
            }

            var result = new List<Location>();
            for (int i = 0; i < positionList.Count; i += 2)
            {
                result.Add(new Location(positionList[i], positionList[i + 1]));
            }
            return new Polygon(result);
        }
    }
}
