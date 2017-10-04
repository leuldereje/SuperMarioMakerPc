using System.Threading.Tasks;
using CommonLib.Components;
using Client.Components;
using Client.Properties;
using Client.Views;

namespace Client.ViewModels {
  public class LauncherViewModel : ViewModel {
    private bool _isUpdateAvailable;

    public LauncherViewModel() {
      CheckForUpdates();
    }

    // properties
    public bool IsUpdateAvailable {
      get => _isUpdateAvailable;
      set {
        _isUpdateAvailable = value;
        OnPropertyChanged();
      }
    }

    // commands
    public Command PlayCommand {
      get {
        return BuildCommand(x => {
          var isFullscreen = Settings.Default.LaunchInFullscreen;

          ProcessManager.Instance.Start(Constants.Process.CEMU, $"{Settings.Default.FilePathEmulator}", $"{(isFullscreen ? "-f" : string.Empty)} -g \"{Settings.Default.FilePathGame}\"");
        });
      }
    }

    public Command CoursesCommand {
      get {
        return BuildCommand(x => {
          new CourseListWindow().ShowDialog();
        });
      }
    }

    public Command SettingsCommand {
      get { return BuildCommand(x => { new SettingsWindow().ShowDialog(); }); }
    }

    public Command UpdateCommand {
      get {
        return BuildCommand(x => {
          // TODO: fill in
          //
          //
        });
      }
    }

    // methods
    private void CheckForUpdates() {
      Task.Run(() => {
        IsUpdateAvailable = Updater.IsUpdateAvailable();
      });
    }
  }
}