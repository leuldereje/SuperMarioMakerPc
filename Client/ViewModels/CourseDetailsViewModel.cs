using System;
using System.IO;
using System.Windows;
using CommonLib.Components;
using CommonLib.Extensions;
using Microsoft.Win32;
using Client.Extensions;
using Client.Game;
using Client.Properties;
using Client.Views;

namespace Client.ViewModels {
  public class CourseDetailsViewModel : ViewModel {
    private Course _course;
    private bool _isViewMode;
    private bool _isImportMode;

    public CourseDetailsViewModel() {
      IsViewMode = true;
    }

    // properties
    public Course Course {
      get => _course;
      set {
        _course = value;
        OnPropertyChanged();
      }
    }

    public bool IsViewMode {
      get => _isViewMode;
      set {
        _isViewMode = value;
        OnPropertyChanged();
      }
    }

    public bool IsImportMode {
      get => _isImportMode;
      set {
        _isImportMode = value;
        OnPropertyChanged();
      }
    }

    // commands
    public Command SaveThumbnailCommand {
      get {
        return BuildCommand(x => {
          var dialog = new SaveFileDialog {
            OverwritePrompt = true,
            Title = "Choose where to save the thumbnail:",
            AddExtension = true,
            DefaultExt = ".jpg",
            Filter = "JPEG File|*.jpg",
            InitialDirectory = Settings.Default.LastDialogPath
          };

          if (dialog.ShowDialog() != true) {
            return;
          }

          if (string.IsNullOrWhiteSpace(dialog.FileName)) {
            return;
          }

          Course.Thumbnail.SaveAsJpg(dialog.FileName);
        });
      }
    }

    public Command ChangeThumbnailCommand {
      get {
        return BuildCommand(x => {
          var dialog = new OpenFileDialog {
            Multiselect = false,
            AddExtension = true,
            DefaultExt = ".jpg",
            Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All Files (*.*)|*.*",
            Title = "Choose a thumbnail:"
          };
          
          var openFilePath = dialog.ShowCustom(Settings.Default.LastDialogPath);

          if (string.IsNullOrWhiteSpace(openFilePath)) {
            return;
          }

          Course.SetThumbnail(openFilePath);
        });
      }
    }

    public Command SaveCommand {
      get {
        return BuildCommand(x => {
          var dialog = new SaveFileDialog {
            Title = "Choose where to save the course:",
            DefaultExt = Constants.System.COURSE_PACKAGE_FILE_EXT,
            Filter = $"{Constants.System.COURSE_PACKAGE_FILE_EXT_DESCRIPTION} (*{Constants.System.COURSE_PACKAGE_FILE_EXT})|*{Constants.System.COURSE_PACKAGE_FILE_EXT}",
            FileName = Course.Title
          };

          var saveFilePath = dialog.ShowCustom();

          if (string.IsNullOrWhiteSpace(saveFilePath)) {
            return;
          }

          Course.SaveAs(saveFilePath);

          PromptWindow.Show("Saved!", MessageBoxButton.OK);
        });
      }
    }

    public Command ClearCommand {
      get {
        return BuildCommand(x => {
          if (PromptWindow.Show("Are you sure you want to clear this course?", MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
            return;
          }

          foreach (var file in Directory.GetFiles(Course.Directory.FullName)) {
            File.Delete(file);
          }

          foreach (var directory in Directory.GetDirectories(Course.Directory.FullName)) {
            Directory.Delete(directory, true);
          }

          File.WriteAllBytes(Path.Combine(Course.Directory.FullName, "course_data.cdt"), Convert.FromBase64String(Files.COURSE_DATA_CDT));
          File.WriteAllBytes(Path.Combine(Course.Directory.FullName, "course_data_sub.cdt"), Convert.FromBase64String(Files.COURSE_DATA_SUB_CDT));
          File.WriteAllBytes(Path.Combine(Course.Directory.FullName, "sound.bwv"), Convert.FromBase64String(Files.SOUND_BWV));
          File.WriteAllBytes(Path.Combine(Course.Directory.FullName, "thumbnail0.tnl"), Convert.FromBase64String(Files.THUMBNAIL0_TNL));
          File.WriteAllBytes(Path.Combine(Course.Directory.FullName, "thumbnail1.tnl"), Convert.FromBase64String(Files.THUMBNAIL1_TNL));

          Course.Reload();
        });
      }
    }

    public Command ImportCommand {
      get { return BuildCommand(x => { /* empty */ }); }
    }
  }
}