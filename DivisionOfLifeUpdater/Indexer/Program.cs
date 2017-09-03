using System;
using System.Collections.Generic;
using System.IO;

namespace Indexer
{
    public static class Program
    {
        private static void Main(string[] args) {

            // Data collections for storing folder and file information.
            var Files = new Dictionary<string, SoftwareFile>();
            var Folders = new List<string>();

            // Necessary paths.
            string StartupPath = AppDomain.CurrentDomain.BaseDirectory;

            Directory.CreateDirectory(StartupPath + "common\\");

            // Variables for storing the current revision, and the amount of 
            // entries in the dictionary.
            int CurrentRevision = 0;
            int FileCount = 0;

            // Loop through every folder in the base directory.
            foreach (var directory in Directory.GetDirectories(StartupPath)) {

                int rev = CurrentRevision;
                if (directory.ToLower().Contains("\\common\\") || directory.ToLower().EndsWith("\\common")) {
                    Console.WriteLine("Found common folder.");
                    rev = Int32.MaxValue;
                    CurrentRevision--;
                } 

                // Each folder in the base directory represents a different version.
                // To get the version number of each folder, increment the revision variable
                // to signify a different version.
                CurrentRevision++;

                // Loop through all the folders and subfolders in the current patch folder.
                foreach (var file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories)) {
                    // Store the file path as the value, and store 
                    // the file path without the revision folder included as the key.
                    // Key should be something like file.dat
                    // Value should be something like /Version1.0/file.dat
                    string key = file.Remove(0, directory.Length);
                    string value = file.Remove(0, StartupPath.Length);

                    // Get the file info for the file we're examining.
                    var fi = new FileInfo(file);

                    // Update the value in the dictionary if we find a newer version of the key.
                    // Add a new value to the dictionary if we find a new key.
                    if (Files.ContainsKey(key)) {
                        Console.WriteLine("Updating " + key);
                        Files[key].Revision = rev;
                        Files[key].Value = value;
                        Files[key].Size = fi.Length;
                    } else {
                        Console.WriteLine("Adding " + key + ": " + fi.Length);
                        Files.Add(key, new SoftwareFile(value, rev, fi.Length));

                        // Add the folder path for the current file to the list of folder paths.
                        Folders.Add(key.Remove(key.Length - fi.Name.Length));
                        
                        // Increment the number of files that we found for later use.
                        FileCount++;
                    }
                }
            }

            // Store all the information we have from the file dictionary 
            // into a .dat file.
            using (var memory = new MemoryStream()) {
                using (var writer = new BinaryWriter(memory)) {

                    // Include the last revision (the current revision) in 
                    // the file, as well as the amount of required files we found.
                    // This is just metadata for the Updater to process.
                    writer.Write(CurrentRevision);
                    writer.Write(FileCount);
                    foreach (var member in Files) {
                        writer.Write(member.Key);
                        writer.Write(member.Value.Value);
                        writer.Write(member.Value.Revision);
                        writer.Write(member.Value.Size);
                    }
                    File.WriteAllBytes(StartupPath + "FileList.dat", memory.ToArray());
                }
            }

            // Store all the folder paths we found into a .dat file.
            using (var memory = new MemoryStream()) {
                using (var writer = new BinaryWriter(memory)) {

                    // Include the number of folders we found 
                    // so that the Updater can process the folder directories.
                    writer.Write(Folders.Count);
                    foreach (var folder in Folders) {
                        writer.Write(folder);
                    }
                    File.WriteAllBytes(StartupPath + "FolderList.dat", memory.ToArray());
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(false);
        }
    }
}
