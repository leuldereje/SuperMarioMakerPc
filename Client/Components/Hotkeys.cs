using System;
using System.Windows.Forms;
using CommonLib.Utils;

// ReSharper disable SwitchStatementMissingSomeCases

namespace Client.Components {
  public class Hotkeys : IDisposable {
    private static readonly Lazy<Hotkeys> _lazy = new Lazy<Hotkeys>(() => new Hotkeys());

    private Hotkeys() { }

    public static Hotkeys Instance => _lazy.Value;

    public void Setup() {
      KeyboardHook.KeyUp += KeyUp;
    }

    // keyboard events
    private static void KeyUp(KeyboardHookEventArgs e) {
      switch (e.Key) {
        case Keys.Escape:
          Escape(e);
          break;
        case Keys.F11:
          F11(e);
          break;
        default:
          return;
      }
    }

    // helper methods
    private static void Escape(KeyboardHookEventArgs e) {
      if (e.Shift) {
        return;
      }

      ProcessManager.Instance.Stop(Constants.Process.CEMU);
    }

    private static void F11(KeyboardHookEventArgs e) {
      var process = ProcessManager.Instance.GetProcess(Constants.Process.CEMU);

      if (process == null) {
        return;
      }

      var hwnd = process.MainWindowHandle;

      Win32Utils.SetForegroundWindow((int) hwnd);

      SendKeys.SendWait("%~");
    }

    // IDispose
    public void Dispose() {
      KeyboardHook.KeyUp -= KeyUp;
    }
  }
}