using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace DivisionOfLifeUpdater
{
    static class Program
    {
        public static Menu Menu;

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Menu = new Menu();
            Menu.Show();

            new Patcher().Begin();

            while (Menu.Visible) {
                Application.DoEvents();
            }
        }

        public static void Repair() {
            string StartupPath = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var directory in Directory.GetDirectories(StartupPath)) {
                Directory.Delete(directory);
            }

            Begin();
        }
    }
}
