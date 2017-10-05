using System.IO;
using Microsoft.Win32;
using Client.Properties;

namespace Client.Extensions {
  public static class DialogExtensions {
    public static string ShowCustom(this SaveFileDialog dialog, string initialDirectory = null) {
      dialog.OverwritePrompt = true;
      dialog.AddExtension = true;
      dialog.InitialDirectory = string.IsNullOrWhiteSpace(initialDirectory) 
        ? Settings.Default.LastDialogPath 
        : initialDirectory;

      var dialogResult = dialog.ShowDialog();

      if (dialogResult != true) {
        return null;
      }

      Settings.Default.LastDialogPath = Path.GetDirectoryName(dialog.FileName);
      Settings.Default.Save();

      return dialog.FileName;
    }

    public static string ShowCustom(this OpenFileDialog dialog, string initialDirectory = null) {
      dialog.InitialDirectory = string.IsNullOrWhiteSpace(initialDirectory)
        ? Settings.Default.LastDialogPath 
        : initialDirectory;

      var dialogResult = dialog.ShowDialog();

      if (dialogResult != true) {
        return null;
      }

      Settings.Default.LastDialogPath = Path.GetDirectoryName(dialog.FileName);
      Settings.Default.Save();

      return dialog.FileName;
    }
  }
}