using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DuckFarm.Classes
{
    public static class AcrylicBackdrop
    {
        private const int WCA_ACCENT_POLICY = 19;
        private const int ACCENT_DISABLED = 0;
        private const int ACCENT_ENABLE_ACRYLICBLURBEHIND = 4;

        [StructLayout(LayoutKind.Sequential)]
        private struct AccentPolicy
        {
            public int AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowCompositionAttributeData
        {
            public int Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowCompositionAttribute(
            IntPtr hwnd,
            ref WindowCompositionAttributeData data);

        public static void Apply(Form form, bool enabled)
        {
            if (form == null ||
                form.IsDisposed ||
                !form.IsHandleCreated)
            {
                return;
            }

            // Acrylic backdrop hanya ditargetkan untuk Windows 11.
            if (Environment.OSVersion.Version.Build < 22000)
                return;

            AccentPolicy policy = new AccentPolicy
            {
                AccentState = enabled
                    ? ACCENT_ENABLE_ACRYLICBLURBEHIND
                    : ACCENT_DISABLED,

                // Flag 2 digunakan untuk Acrylic.
                AccentFlags = enabled ? 2 : 0,

                // Warna tint gelap dengan transparansi.
                // Alpha lebih rendah berarti tint lebih transparan.
                GradientColor = enabled
                    ? Color.FromArgb(102, 20, 27, 38).ToArgb()
                    : 0,

                AnimationId = 0
            };

            IntPtr memory = IntPtr.Zero;

            try
            {
                int size = Marshal.SizeOf(typeof(AccentPolicy));

                memory = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(policy, memory, false);

                WindowCompositionAttributeData data =
                    new WindowCompositionAttributeData
                    {
                        Attribute = WCA_ACCENT_POLICY,
                        Data = memory,
                        SizeOfData = size
                    };

                SetWindowCompositionAttribute(
                    form.Handle,
                    ref data);
            }
            finally
            {
                if (memory != IntPtr.Zero)
                    Marshal.FreeHGlobal(memory);
            }
        }
    }
}