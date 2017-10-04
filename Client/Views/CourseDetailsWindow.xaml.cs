using CommonLib.Components;
using Client.Components;
using Client.ViewModels;

namespace Client.Views {
  public partial class CourseDetailsWindow : CustomWindow {
    public CourseDetailsWindow() {
      InitializeComponent();

      ViewModel.ImportCommand.PostExecute += ImportCommandPostExecute;
    }

    // properties
    public CourseDetailsViewModel ViewModel => DataContext as CourseDetailsViewModel;

    // command events
    private void ImportCommandPostExecute(object sender, CommandEventArgs e) {
      DialogResult = true;
    }
  }
}