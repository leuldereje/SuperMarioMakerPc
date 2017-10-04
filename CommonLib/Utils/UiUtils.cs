using System;
using System.ComponentModel;
using System.Windows;

namespace CommonLib.Utils {
  public static class UiUtils {
    public static bool IsInDesignMode() {
      return DesignerProperties.GetIsInDesignMode(new FrameworkElement());
    }

    public static void RunOnUiThread(Action action) {
      Application.Current?.Dispatcher.BeginInvoke(action);
    }
  }
}