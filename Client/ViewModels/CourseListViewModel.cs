using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommonLib.Components;
using CommonLib.Utils;
using Client.Game;
using Client.Properties;
using Client.Views;

namespace Client.ViewModels {
  public class CourseListViewModel : ViewModel {
    private ObservableCollection<Course> _courses = new ObservableCollection<Course>();
    private bool _isViewMode;
    private bool _isImportMode;

    public CourseListViewModel() {
      IsViewMode = true;

      if (UiUtils.IsInDesignMode()) {
        return;
      }

      LoadCourses();
    }

    // properties
    public ObservableCollection<Course> Courses {
      get => _courses;
      set {
        _courses = value;
        OnPropertyChanged();
      }
    }

    public Course CourseImport { get; set; }

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
    public Command CourseClickCommand {
      get {
        return BuildCommand(x => {
          var course = (Course) x;

          if (IsImportMode) {
            ImportCourse(course, CourseImport);
          }
          else {
            var window = new CourseDetailsWindow();
            window.ViewModel.Course = course;
            window.ShowDialog();
          }
        });
      }
    }

    // methods
    public void LoadCourses() {
      _courses.Clear();

      var courseDir = Path.Combine(Path.GetDirectoryName(AssemblyUtils.GetFullAssemblyPath()), Settings.Default.DirectoryPathCourses);
      var dirs = Directory.GetDirectories(courseDir);

      Task.Run(() => {
        foreach (var dir in dirs) {
          var course = Course.FromDirectory(dir);

          UiUtils.RunOnUiThread(() => Courses.Add(course));

          Thread.Sleep(5);
        }
      });
    }

    private static void ImportCourse(Course oldCourse, Course newCourse) {
      DirectoryUtils.ClearDirectory(oldCourse.Directory.FullName);

      newCourse.Save(oldCourse.Directory.FullName);
    }
  }
}