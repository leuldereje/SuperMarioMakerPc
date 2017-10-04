using System.Windows;
using CommonLib.Extensions;
using Server.ViewModel;

namespace Server.Views {
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();

      ViewModel.SaveCommand.PostExecute += SaveCommandPostExecute;
    }

    // properties
    public MainViewModel ViewModel => DataContext as MainViewModel;

    // command events
    private void SaveCommandPostExecute(object sender, CommonLib.Components.CommandEventArgs e) {
      Close();
    }

    // ui events
    private void PortTextBoxPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
      var isNumeric = e.Text.IsNumeric();

      e.Handled = !isNumeric;
    }
  }
}