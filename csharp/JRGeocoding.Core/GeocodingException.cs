using System;

namespace JRGeocoding.Core
{
    public class GeocodingException : Exception
    {
        public GeocodingException(string message) : base(message) { }
    }
}
