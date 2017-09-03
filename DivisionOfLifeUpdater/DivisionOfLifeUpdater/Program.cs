using System;
using System.Windows.Forms;

namespace DivisionOfLifeUpdater
{
    static class Program
    {
        public static Menu Menu;

        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            foreach (var str in args) {
                Console.WriteLine(str);
            }

            Menu = new Menu();
            Menu.Show();

            new NewPatcher().BeginProcess(args);

            while (Menu.Visible) {
                Application.DoEvents();
            }
        }
    }
}
