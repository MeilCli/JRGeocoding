using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using SpanJson;

namespace JRGeocoding.Generator
{
    public class Program
    {
        // should not change this value
        // if change this value, will be longer executing time of generator
        private const int mapMagnification = 10;

        public static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private string[] debugDirectories = { "generator", "bin", "Debug", "net5.0" };

        public void Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var map = new Map(mapMagnification);
            foreach (string filePath in Directory.EnumerateFiles(getSourceDirectory()))
            {
                if (filePath.EndsWith(".zip"))
                {
                    Console.WriteLine($"Start Read {filePath}");
                    GmlReader gmlReader = readGml(filePath);
                    while (true)
                    {
                        Address? result = gmlReader.Read();
                        if (result is null)
                        {
                            break;
                        }
                        map.AddAddress(result);
                    }
                    Console.WriteLine($"End Read {filePath}");
                }
            }

            string dataDirectory = getDataDirectory();
            if (Directory.Exists(dataDirectory))
            {
                Directory.Delete(dataDirectory, true);
            }
            if (Directory.Exists(dataDirectory) is false)
            {
                Directory.CreateDirectory(dataDirectory);
            }

            Console.WriteLine("Start Write Address Json");
            var fileNames = new List<string>();
            foreach ((var location, var addresses) in map.Values())
            {
                string fileName = $"{location.Latitude}-{location.Longitude}.json";
                string filePath = Path.Combine(dataDirectory, fileName);
                byte[] json = AddressJsonSerializer.Serialize(addresses);
                File.WriteAllBytes(filePath, json);
                fileNames.Add(fileName);
            }
            Console.WriteLine("End Write Address Json");

            Console.WriteLine("Start Write Path Json");
            File.WriteAllBytes(Path.Combine(dataDirectory, "path.json"), JsonSerializer.Generic.Utf8.Serialize(fileNames));
            Console.WriteLine("End Write Path Json");

            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            Console.WriteLine($"Total: {timeSpan.TotalSeconds} seconds");
        }

        private string getExecuteOrSolutionDirectory()
        {
            string currentDirectory = Environment.CurrentDirectory;
            string debugDirectoryPath = Path.Combine(debugDirectories);
            if (currentDirectory.EndsWith(debugDirectoryPath))
            {
                return Path.GetFullPath("../../../../", currentDirectory);
            }
            return currentDirectory;
        }

        private string getSourceDirectory()
        {
            return Path.Combine(getExecuteOrSolutionDirectory(), "source");
        }

        private string getDataDirectory()
        {
            return Path.Combine(getExecuteOrSolutionDirectory(), "public", "data");
        }

        private GmlReader readGml(string zipPath)
        {
            GmlReader readZipArchiveEntry(ZipArchiveEntry zipArchiveEntry)
            {
                using var ms = new MemoryStream();
                Stream stream = zipArchiveEntry.Open();
                byte[] buffer = new byte[16 * 1024];
                int read;

                while (0 < (read = stream.Read(buffer, 0, buffer.Length)))
                {
                    ms.Write(buffer, 0, read);
                }

                return new GmlReader(ms.ToArray());
            }

            using ZipArchive archive = ZipFile.OpenRead(zipPath);

            ZipArchiveEntry? result = null;
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (entry.FullName.EndsWith(".gml"))
                {
                    if (result is not null)
                    {
                        throw new Exception($"Found multiple gml file at {zipPath}");
                    }
                    result = entry;
                }
            }
            if (result is not null)
            {
                return readZipArchiveEntry(result);
            }

            throw new Exception($"Not found gml file at {zipPath}");
        }
    }
}
