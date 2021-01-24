using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JRGeocoding.Core
{
    public class LocalContentReader : IContentReader
    {
        private readonly string[] paths;

        public LocalContentReader(string dataPath)
        {
            if (Directory.Exists(dataPath) is false)
            {
                throw new GeocodingException("Do not data directory");
            }
            this.paths = Directory.EnumerateFiles(dataPath).Where(x => x.EndsWith(".json")).ToArray();
        }

        public ValueTask<string?> ReadContentAsync(string fileName)
        {
            string? filePath = paths.FirstOrDefault(x => x.EndsWith(fileName));
            if (filePath is null)
            {
                return new ValueTask<string?>((string?)null);
            }
            return new ValueTask<string?>(File.ReadAllText(filePath));
        }
    }
}
