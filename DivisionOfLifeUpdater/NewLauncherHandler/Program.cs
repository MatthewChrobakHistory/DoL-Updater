using System.Diagnostics;
using System.IO;
using System;

namespace NewLauncherHandler
{
    public static class Program
    {
        public static void Main(string[] arg) {
            int tick;
            int waitTick = Environment.TickCount + 2500;

            if (arg.Length == 0) {
                Console.WriteLine("No args");
                return;
            }

            while (true) {
                tick = Environment.TickCount;

                if (waitTick < tick) {
                    break;
                }
            }

            string fullArg = "";
            for (int i = 0; i < arg.Length; i++) {
                fullArg += arg[i];
                if (i != arg.Length - 1) {
                    fullArg += " ";
                }
            }
            string[] args = fullArg.Split('¨');
            if (args.Length == 2) {
                string oldAppPath = args[0];
                string newAppPath = args[1];

                if (File.Exists(oldAppPath)) {
                    if (File.Exists(newAppPath)) {
                        var fiO = new FileInfo(oldAppPath);
                        var fiN = new FileInfo(newAppPath);

                        //string path = fiO.FullName.Remove(fiO.FullName.Length - fiO.Name.Length);
                        //string fullPath = path + fiN.Name;

                        File.Delete(oldAppPath);
                        File.Copy(newAppPath, oldAppPath);

                        while (!File.Exists(oldAppPath)) {

                        }

                        File.Delete(newAppPath);

                        while (File.Exists(newAppPath)) {

                        }
                        Process.Start(oldAppPath, "NewLauncher");
                    } else {
                        Console.WriteLine("new app not exists");
                    }
                } else {
                    Console.WriteLine("old app not exists");
                }
            } else {
                Console.WriteLine("Length is not 2");
            }
        }
    }
}
