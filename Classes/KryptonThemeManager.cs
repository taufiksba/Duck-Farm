using System.Drawing;
using Krypton.Toolkit;

namespace DuckFarm.Classes
{
    /// <summary>
    /// Adapter antara ThemeManager aplikasi dan global palette Krypton.
    /// ThemeManager aplikasi tetap menjadi sumber keputusan Light/Dark.
    /// </summary>
    public static class KryptonThemeManager
    {
        // KryptonManager adalah komponen instance, bukan kelas static.
        // Instance dipertahankan selama aplikasi berjalan agar pengaturan global aktif.
        private static readonly KryptonManager kryptonManager = new KryptonManager();

        private static bool isInitialized;

        /// <summary>
        /// Menyiapkan konfigurasi Krypton dan palette awal aplikasi.
        /// Aman dipanggil lebih dari satu kali.
        /// </summary>
        public static void Initialize(DuckFarm.Classes.ThemeManager.ThemeMode theme)
        {
            if (!isInitialized)
            {
                // Jangan mengambil alih renderer MenuStrip/StatusStrip WinForms lama.
                kryptonManager.GlobalApplyToolstrips = false;
                kryptonManager.BaseFont = new Font("Segoe UI", 9F);
                isInitialized = true;
            }

            ApplyTheme(theme);
        }

        /// <summary>
        /// Memetakan tema aplikasi ke palette bawaan Krypton.
        /// </summary>
        public static void ApplyTheme(DuckFarm.Classes.ThemeManager.ThemeMode theme)
        {
            switch (theme)
            {
                case DuckFarm.Classes.ThemeManager.ThemeMode.Dark:
                    kryptonManager.GlobalPaletteMode = PaletteMode.Microsoft365BlackDarkMode;
                    break;

                case DuckFarm.Classes.ThemeManager.ThemeMode.Light:
                default:
                    kryptonManager.GlobalPaletteMode = PaletteMode.Microsoft365Blue;
                    break;
            }
        }
    }
}
