using System.IO;
using System.IO.Compression;

namespace DivisionOfLifeUpdater.IO
{
    public static class Compression
    {
        public static void DecompressDirectory(string file, string directory) {
            // If the directory does not exist, create it.
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(file)) {
                return;
            }

            using (var test = ZipFile.Open(file, System.IO.Compression.ZipArchiveMode.Read)) {
                foreach (var entry in test.Entries) {
                    if (File.Exists(directory + entry.Name)) {
                        File.Delete(directory + entry.Name);
                    }
                }
            }

            // Extract the zipped file's contents to the directory.
            ZipFile.ExtractToDirectory(file, directory);

            File.Delete(file);
        }
    }
}
