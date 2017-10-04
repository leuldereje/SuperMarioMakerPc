using System.Windows;
using System.Windows.Input;
using CommonLib.Components;

namespace Client.Components {
  public class CustomWindow : Window {
    private bool? _dialogResult;

    protected CustomWindow() {
      MouseDown += WindowMouseDown;
      KeyDown += WindowKeyDown;
    }

    // properties
    protected new bool? DialogResult {
      get => _dialogResult;
      set {
        _dialogResult = value;
        Close();
      }
    }

    // overrides
    public new bool? ShowDialog() {
      base.ShowDialog();

      return DialogResult;
    }

    public new void Close() {
      (DataContext as ViewModel)?.DisableCommands();
      base.Close();
    }

    // ui events
    private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
      if (e.ChangedButton == MouseButton.Left) {
        DragMove();
      }
    }

    private void WindowKeyDown(object sender, KeyEventArgs e) {
      if (e.Key == Key.Escape) {
        Close();
      }
    }
  }
}