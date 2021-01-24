namespace JRGeocoding.Core
{
    public class Line
    {
        public Location Location1 { get; }

        public Location Location2 { get; }

        public Line(Location location1, Location location2) => (Location1, Location2) = (location1, location2);
    }
}
