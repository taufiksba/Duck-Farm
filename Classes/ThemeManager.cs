
using DuckFarm.Classes.Controls;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace DuckFarm.Classes
{
    public static class ThemeManager
    {
        // =========================================================
        // THEME MODE
        // =========================================================

        public enum ThemeMode
        {
            Light,
            Dark
        }

        private static ThemeMode currentTheme = ThemeMode.Light;

        public static ThemeMode CurrentTheme
        {
            get { return currentTheme; }
        }

        public static bool IsDarkMode
        {
            get { return currentTheme == ThemeMode.Dark; }
        }


        // =========================================================
        // THEME CONTROL
        // =========================================================

        public static void SetTheme(ThemeMode theme)
        {
            currentTheme = theme;
        }

        public static void ToggleTheme()
        {
            currentTheme = currentTheme == ThemeMode.Light
                ? ThemeMode.Dark
                : ThemeMode.Light;
        }


        // =========================================================
        // THEME COLORS
        // =========================================================

        public static Color FormBackColor
        {
            get
            {
                return IsDarkMode
                    ? Color.FromArgb(30, 41, 59)
                    : Color.FromArgb(248, 250, 252);
            }
        }

        public static Color SurfaceColor
        {
            get
            {
                return IsDarkMode
                    ? Color.FromArgb(51, 65, 85)
                    : Color.White;
            }
        }

        public static Color ForeColor
        {
            get
            {
                return IsDarkMode
                    ? Color.FromArgb(241, 245, 249)
                    : Color.FromArgb(30, 41, 59);
            }
        }

        public static Color MutedForeColor
        {
            get
            {
                return IsDarkMode
                    ? Color.FromArgb(203, 213, 225)
                    : Color.FromArgb(100, 116, 139);
            }
        }

        public static Color BorderColor
        {
            get
            {
                return IsDarkMode
                    ? Color.FromArgb(71, 85, 105)
                    : Color.FromArgb(203, 213, 225);
            }
        }

        public static Color AccentColor
        {
            get
            {
                return Color.FromArgb(37, 99, 235);
            }
        }


        // =========================================================
        // APPLY THEME
        // =========================================================

        public static void ApplyTheme(Control control)
        {
            if (control == null)
                return;

            // -----------------------------------------------------
            // FORM
            // -----------------------------------------------------

            Form form = control as Form;

            if (form != null)
            {
                form.BackColor = FormBackColor;
                form.ForeColor = ForeColor;
            }

            // -----------------------------------------------------
            // APP PANEL
            // Harus diperiksa sebelum Panel biasa karena AppPanel
            // merupakan turunan dari Panel.
            // -----------------------------------------------------

            else if (control is AppPanel appPanel)
            {
                appPanel.PanelBackColor = SurfaceColor;
                appPanel.BorderColor = BorderColor;
                appPanel.ForeColor = ForeColor;
            }

            // -----------------------------------------------------
            // PANEL BIASA
            // -----------------------------------------------------

            else if (control is Panel panel)
            {
                panel.BackColor = SurfaceColor;
                panel.ForeColor = ForeColor;
            }

            // -----------------------------------------------------
            // LABEL
            // -----------------------------------------------------

            else if (control is Label label)
            {
                label.ForeColor = ForeColor;
            }

            // -----------------------------------------------------
            // BUTTON
            // -----------------------------------------------------

            else if (control is Button button)
            {
                button.BackColor = AccentColor;
                button.ForeColor = Color.White;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = AccentColor;
            }

            // -----------------------------------------------------
            // TEXTBOX
            // -----------------------------------------------------

            else if (control is TextBox textBox)
            {
                textBox.BackColor = SurfaceColor;
                textBox.ForeColor = ForeColor;
            }

            // -----------------------------------------------------
            // COMBOBOX
            // -----------------------------------------------------

            else if (control is ComboBox comboBox)
            {
                comboBox.BackColor = SurfaceColor;
                comboBox.ForeColor = ForeColor;
            }

            // -----------------------------------------------------
            // DATE TIME PICKER
            // -----------------------------------------------------

            else if (control is DateTimePicker dateTimePicker)
            {
                dateTimePicker.BackColor = SurfaceColor;
                dateTimePicker.ForeColor = ForeColor;

                dateTimePicker.CalendarMonthBackground = SurfaceColor;
                dateTimePicker.CalendarForeColor = ForeColor;
                dateTimePicker.CalendarTitleBackColor = SurfaceColor;
                dateTimePicker.CalendarTitleForeColor = ForeColor;
                dateTimePicker.CalendarTrailingForeColor = MutedForeColor;
            }

            // -----------------------------------------------------
            // CHECKBOX
            // -----------------------------------------------------

            else if (control is CheckBox checkBox)
            {
                checkBox.ForeColor = ForeColor;
            }

            // -----------------------------------------------------
            // RADIOBUTTON
            // -----------------------------------------------------

            else if (control is RadioButton radioButton)
            {
                radioButton.ForeColor = ForeColor;
            }

            // -----------------------------------------------------
            // OTHER CONTROLS
            // -----------------------------------------------------

            else
            {
                control.ForeColor = ForeColor;
            }

            // -----------------------------------------------------
            // RECURSIVE CHILD CONTROLS
            // -----------------------------------------------------

            foreach (Control child in control.Controls)
            {
                ApplyTheme(child);
            }
        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(
        IntPtr hWnd,
        string pszSubAppName,
        string pszSubIdList);
    }
}
