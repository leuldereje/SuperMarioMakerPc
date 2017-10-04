using System;
using System.Runtime.InteropServices;

namespace CommonLib.Utils {
  public static class Win32Utils {
    [DllImport("Shell32.dll")]
    private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);
    public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

    public static void RefreshIconAssociations() {
      SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern short GetKeyState(int vKey);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr LoadLibrary(string fileName);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern int UnhookWindowsHookEx(int idHook);

    [DllImport("user32.dll")]
    public static extern int SetForegroundWindow(int hwnd);
  }
}