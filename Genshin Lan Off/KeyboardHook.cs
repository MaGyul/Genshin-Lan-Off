using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Genshin_Lan_Off
{
    public class KeyboardHook
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static readonly int WH_KEYBOARD_LL = 13;
        public static readonly int WM_KEYDOWN = 0x100;
        public static readonly int WM_KEYUP = 0x101;
        public static readonly int WM_SYSKEYDOWN = 0x104;
        public static readonly int WM_SYSKEYUP = 0x105;

        private static IntPtr hook = IntPtr.Zero;

        public static void SetHook(LowLevelKeyboardProc callback)
        {
            IntPtr hInstance = LoadLibrary("User32");

            hook = SetWindowsHookEx(WH_KEYBOARD_LL, callback, hInstance, 0);
        }

        public static void UnHook()
        {
            if (hook == IntPtr.Zero) return;

            UnhookWindowsHookEx(hook);

            hook = IntPtr.Zero;
        }

        public static IntPtr NextHook(int nCode, int wParam, IntPtr lParam)
        {
            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        public static Keys ReadKey(IntPtr lParam)
        {
            return (Keys) Marshal.ReadInt32(lParam);
        }
    }
}
