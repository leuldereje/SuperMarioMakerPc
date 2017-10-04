using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Client.Components;
using Client.Game;
using Client.Views;

namespace Client {
  public partial class App : Application {
    public App() {
      ShutdownMode = ShutdownMode.OnExplicitShutdown;

      Current.Dispatcher.UnhandledException += OnUnhandledException;
    }

    // overrides
    protected override void OnStartup(StartupEventArgs e) {
      if (e.Args.Any()) {
        LoadCoursePackage(e.Args[0]);
        Shutdown();
      }

      Hotkeys.Instance.Setup();
    }

    // methods
    private static void LoadCoursePackage(string filePath) {
      if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath)) {
        return;
      }

      var course = Course.FromArchive(filePath);

      var detailsWindow = new CourseDetailsWindow();
      detailsWindow.ViewModel.IsViewMode = false;
      detailsWindow.ViewModel.IsImportMode = true;
      detailsWindow.ViewModel.Course = course;

      if (detailsWindow.ShowDialog() != true) {
        return;
      }

      var importWindow = new CourseListWindow();
      importWindow.ViewModel.IsViewMode = false;
      importWindow.ViewModel.IsImportMode = true;
      importWindow.ViewModel.CourseImport = course;
      importWindow.ShowDialog();
    }

    // app events
    private static void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
      var msg = e.Exception.Message;
      MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

      e.Handled = true;
    }
  }
}
