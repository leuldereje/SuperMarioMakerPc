using System.Windows;
using Server.Components;
using Application = System.Windows.Application;

namespace Server {
  public partial class App : Application {
    public App() {
      ShutdownMode = ShutdownMode.OnExplicitShutdown;
    }

    // app events
    private void OnStartup(object sender, StartupEventArgs e) {
      TrayIcon.Instance.Setup();
    }

    private void OnExit(object sender, ExitEventArgs e) {
      TrayIcon.Instance.CleanUp();
    }
  }
}