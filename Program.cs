using DuckFarm.Classes;
using DuckFarm.Classes.Factories;
using System.Windows.Forms;

namespace DuckFarm
{
    internal static class Program
    {
        [System.STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            KryptonThemeManager.Initialize(ThemeManager.CurrentTheme);

            IFormFactory formFactory = new FormFactory();
            Application.Run(new DUCKMENU(formFactory));
        }
    }
}
