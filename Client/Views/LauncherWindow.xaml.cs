using System.Threading;
using System.Windows;
using CommonLib.Components;
using Client.Components;
using Client.ViewModels;

namespace Client.Views {
  public partial class LauncherWindow : CustomWindow {
    public LauncherWindow() {
      InitializeComponent();

      ViewModel.PlayCommand.PreExecute += PlayCommandPreExecute;
      ViewModel.CoursesCommand.PreExecute += CoursesCommandPreExecute;
      ViewModel.CoursesCommand.PostExecute += CoursesCommandPostExecute;

      ProcessManager.Instance.ProcessExited += ProcessExited;
    }

    // properties
    public LauncherViewModel ViewModel => (LauncherViewModel) DataContext;

    // command events
    private void PlayCommandPreExecute(object sender, CommandEventArgs e) {
      Hide();

      Thread.Sleep(50);
    }

    private void CoursesCommandPreExecute(object sender, CommandEventArgs e) {
      Hide();

      Thread.Sleep(50);
    }

    private void CoursesCommandPostExecute(object sender, CommandEventArgs e) {
      Show();
    }

    // ui events
    private void WindowClosed(object sender, System.EventArgs e) {
      Application.Current.Shutdown();
    }

    // other events
    private void ProcessExited(object sender, string key) {
      if (key != Constants.Process.CEMU) {
        return;
      }

      Show();
    }
  }
}