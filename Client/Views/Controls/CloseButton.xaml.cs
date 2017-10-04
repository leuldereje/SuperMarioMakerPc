using System.Windows;
using System.Windows.Controls;
using CommonLib.Extensions;

namespace Client.Views.Controls {
  public partial class CloseButton : Button {
    public CloseButton() {
      InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e) {
      var window = this.FindParent<Window>();

      window.Close();
    }
  }
}