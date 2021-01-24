using System.Threading.Tasks;

namespace JRGeocoding.Core
{
    public interface IContentReader
    {
        public ValueTask<string?> ReadContentAsync(string fileName);
    }
}
