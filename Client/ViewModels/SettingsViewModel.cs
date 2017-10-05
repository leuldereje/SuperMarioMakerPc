using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Client.Extensions;
using Client.Properties;
using CommonLib.Components;
using CommonLib.Utils;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Client.ViewModels {
  public class SettingsViewModel : ViewModel {
    private string _version;
    private bool _launchInFullscreen;
    private string _filePathGame;
    private string _filePathEmulator;
    private string _directoryPathCourses;

    public SettingsViewModel() {
      if (UiUtils.IsInDesignMode()) {
        return;
      }

      LoadVersion();
      LoadSettings();
    }

    // properties
    public string Version {
      get => _version;
      set {
        _version = value;
        OnPropertyChanged();
      }
    }

    public bool LaunchInFullscreen {
      get => _launchInFullscreen;
      set {
        _launchInFullscreen = value;
        OnPropertyChanged();
      }
    }

    public string FilePathGame {
      get => _filePathGame;
      set {
        _filePathGame = value;
        OnPropertyChanged();
      }
    }

    public string FilePathEmulator {
      get => _filePathEmulator;
      set {
        _filePathEmulator = value;
        OnPropertyChanged();
      }
    }

    public string DirectoryPathCourses {
      get => _directoryPathCourses;
      set {
        _directoryPathCourses = value;
        OnPropertyChanged();
      }
    }

    // commands
    public Command SaveCommand {
      get {
        return BuildCommand(x => {
          Settings.Default.LaunchInFullscreen = LaunchInFullscreen;
          Settings.Default.FilePathGame = FilePathGame;
          Settings.Default.FilePathEmulator = FilePathEmulator;
          Settings.Default.DirectoryPathCourses = DirectoryPathCourses;
          Settings.Default.Save();
        });
      }
    }

    public Command CancelCommand {
      get {
        return BuildCommand(x => {
          /* empty */
        });
      }
    }

    public Command RestoreFileAssociationsCommand {
      get {
        return BuildCommand(x => {
          var iconPath = Path.GetFullPath("qblock.ico");
          var command = $"{AssemblyUtils.GetFullAssemblyPath()}";

          RegistryUtils.AddFileTypeAssociation(Constants.System.COURSE_PACKAGE_FILE_EXT, Constants.System.COURSE_PACKAGE_FILE_EXT_KEY, Constants.System.COURSE_PACKAGE_FILE_EXT_DESCRIPTION, command, iconPath);
          Win32Utils.RefreshIconAssociations();
        });
      }
    }

    public Command ClearFileAssociationsCommand {
      get {
        return BuildCommand(x => {
          RegistryUtils.RemoveFileTypeAssociation(Constants.System.COURSE_PACKAGE_FILE_EXT, Constants.System.COURSE_PACKAGE_FILE_EXT_KEY);
          Win32Utils.RefreshIconAssociations();
        });
      }
    }

    public Command BrowseGamePathCommand {
      get {
        return BuildCommand(x => {
          var initDir = string.IsNullOrWhiteSpace(FilePathGame)
            ? @"C:\"
            : Path.GetDirectoryName(FilePathGame);

          var dialog = new OpenFileDialog {
            Multiselect = false,
            DefaultExt = ".rpx",
            Filter = "RPX File|*.rpx",
            Title = "Browse for the game file:"
          };

          var openFilePath = dialog.ShowCustom(initDir);

          if (string.IsNullOrWhiteSpace(openFilePath)) {
            return;
          }

          if (!File.Exists(Path.GetFullPath(openFilePath))) {
            MessageBox.Show("File does not exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          }

          FilePathGame = openFilePath;
        });
      }
    }

    public Command BrowseEmulatorPathCommand {
      get {
        return BuildCommand(x => {
          var initDir = string.IsNullOrWhiteSpace(FilePathEmulator)
            ? @"C:\"
            : Path.GetDirectoryName(FilePathEmulator);

          var dialog = new OpenFileDialog {
            Multiselect = false,
            DefaultExt = ".exe",
            Filter = "EXE File|*.exe",
            Title = "Browse for the emualtor file:"
          };

          var openFilePath = dialog.ShowCustom(initDir);

          if (string.IsNullOrWhiteSpace(openFilePath)) {
            return;
          }

          if (!File.Exists(Path.GetFullPath(openFilePath))) {
            MessageBox.Show("File does not exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          }

          FilePathEmulator = openFilePath;
        });
      }
    }

    public Command BrowseCoursesPathCommand {
      get {
        return BuildCommand(x => {
          var initDir = string.IsNullOrWhiteSpace(DirectoryPathCourses)
            ? @"C:\"
            : Path.GetFullPath(DirectoryPathCourses);

          var dialog = new FolderBrowserDialog {
            Description = "Browse for the courses directory:",
            RootFolder = Environment.SpecialFolder.Desktop,
            SelectedPath = initDir
          };

          dialog.ShowDialog();

          if (string.IsNullOrWhiteSpace(dialog.SelectedPath)) {
            return;
          }

          if (!Directory.Exists(Path.GetFullPath(dialog.SelectedPath))) {
            MessageBox.Show("File does not exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          }

          DirectoryPathCourses = dialog.SelectedPath;
        });
      }
    }

    // methods
    private void LoadVersion() {
      var ver = AssemblyUtils.GetAssemblyFileVersion();

      // we only want to show the major/minor digits
      ver = ver.Substring(0, ver.Length - 4);

      Version = $"v{ver}";
    }

    private void LoadSettings() {
      LaunchInFullscreen = Settings.Default.LaunchInFullscreen;
      FilePathGame = Settings.Default.FilePathGame;
      FilePathEmulator = Settings.Default.FilePathEmulator;
      DirectoryPathCourses = Settings.Default.DirectoryPathCourses;
    }
  }
}