using System;
using System.IO;
using System.Threading.Tasks;
using JRGeocoding.Core;

namespace JRGeocoding.Console
{
    public class Program
    {
        private const string apiServer = "https://jrgeocoding.meilcli.net/data/";

        public async static Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.WriteLine("Args must be latitude and longitude");
                return;
            }
            if (double.TryParse(args[0], out double latitude) is false)
            {
                System.Console.WriteLine("First arg must be latitude");
                return;
            }
            if (double.TryParse(args[1], out double longitude) is false)
            {
                System.Console.WriteLine("Second arg must be longitude");
                return;
            }
            bool isLocal = false;
            if (3 <= args.Length && args[2] == "--local")
            {
                isLocal = true;
            }

            var program = new Program();
            await program.RunAsync(latitude, longitude, isLocal);
        }

        private string[] debugDirectories = { "csharp", "JRGeocoding.Console", "bin", "Debug", "net5.0" };

        public async Task RunAsync(double latitude, double longitude, bool isLocal)
        {
            IContentReader contentReader;
            if (isLocal)
            {
                contentReader = new LocalContentReader(getDataDirectory());
            }
            else
            {
                contentReader = new HttpContentReader(apiServer);
            }
            var geocoding = new JapaneseReverseGeocoding(contentReader);
            Address? address = await geocoding.FindAddressAsync(latitude, longitude);
            if (address is not null)
            {
                System.Console.WriteLine($"{address.Prefecture}{address.City}{address.Ward}{address.Town}");
            }
            else
            {
                System.Console.WriteLine("Not found address");
            }
        }

        private string getExecuteOrSolutionDirectory()
        {
            string currentDirectory = Environment.CurrentDirectory;
            string debugDirectoryPath = Path.Combine(debugDirectories);
            if (currentDirectory.EndsWith(debugDirectoryPath))
            {
                return Path.GetFullPath("../../../../../", currentDirectory);
            }
            return currentDirectory;
        }

        private string getDataDirectory()
        {
            return Path.Combine(getExecuteOrSolutionDirectory(), "public", "data");
        }
    }
}
