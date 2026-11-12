
using System.Windows.Forms;

namespace DuckFarm.Classes.Helpers
{
    public static class ComboBoxHelper
    {
        /// <summary>
        /// Mencegah roda mouse mengubah pilihan ComboBox.
        /// Dropdown tetap dapat dibuka dan item tetap dapat dipilih dengan klik.
        /// </summary>
        public static void PreventMouseWheelChange(ComboBox comboBox)
        {
            if (comboBox == null)
                return;

            comboBox.MouseWheel -= ComboBox_MouseWheel;
            comboBox.MouseWheel += ComboBox_MouseWheel;
        }

        private static void ComboBox_MouseWheel(
            object sender,
            MouseEventArgs e)
        {
            if (e is HandledMouseEventArgs handledEvent)
            {
                handledEvent.Handled = true;
            }
        }
    }
}
