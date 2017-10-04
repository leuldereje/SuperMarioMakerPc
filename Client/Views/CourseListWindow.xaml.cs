using System.Windows;
using CommonLib.Components;
using Client.Components;
using Client.ViewModels;

namespace Client.Views {
  public partial class CourseListWindow : CustomWindow {
    public CourseListWindow() {
      InitializeComponent();

      ViewModel.CourseClickCommand.PreExecute += CourseClickCommandPreExecute;
      ViewModel.CourseClickCommand.PostExecute += CourseClickCommandPostExecute;
    }

    // properties
    public CourseListViewModel ViewModel => DataContext as CourseListViewModel;

    // command events
    private void CourseClickCommandPreExecute(object sender, CommandEventArgs e) {
      if (!ViewModel.IsImportMode) {
        return;
      }

      if (PromptWindow.Show("Are you sure you want to overwrite this course?", MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
        e.Handled = true;
      }
    }

    private void CourseClickCommandPostExecute(object sender, CommandEventArgs e) {
      if (!ViewModel.IsImportMode) {
        return;
      }

      PromptWindow.Show("Done!", MessageBoxButton.OK);

      Close();
    }
  }
}