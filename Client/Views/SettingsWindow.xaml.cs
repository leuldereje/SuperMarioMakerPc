using System.Windows;
using CommonLib.Components;
using Client.ViewModels;

namespace Client.Views {
  public partial class SettingsWindow : Window {
    public SettingsWindow() {
      InitializeComponent();

      ViewModel.SaveCommand.PostExecute += SaveCommandPostExecute;
      ViewModel.CancelCommand.PostExecute += CancelCommandPostExecute;
    }
    
    // properties
    public SettingsViewModel ViewModel => (SettingsViewModel) DataContext;

    // command events
    private void SaveCommandPostExecute(object sender, CommandEventArgs e) {
      DialogResult = true;
    }

    private void CancelCommandPostExecute(object sender, CommandEventArgs e) {
      DialogResult = false;
    }
  }
}