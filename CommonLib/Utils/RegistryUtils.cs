using Microsoft.Win32;

namespace CommonLib.Utils {
  public static class RegistryUtils {
    // see: http://www.java2s.com/Code/CSharp/Development-Class/RegistryFileAssociation.htm
    public static void AddFileTypeAssociation(string ext, string name, string description, string exePath, string iconPath = null) {
      var hkcu = Registry.CurrentUser;
      var softwareKey = hkcu.OpenSubKey("Software");
      var classesKey = softwareKey?.OpenSubKey("Classes", RegistryKeyPermissionCheck.ReadWriteSubTree);

      var extKey = classesKey?.CreateSubKey(ext);
      extKey?.SetValue(null, name);

      var nameKey = classesKey?.CreateSubKey(name);
      nameKey?.SetValue(null, description);

      if (!string.IsNullOrWhiteSpace(iconPath)) {
        var iconKey = nameKey?.CreateSubKey("DefaultIcon");
        iconKey?.SetValue(null, iconPath);
        iconKey?.Dispose();
      }

      var shellKey = nameKey?.CreateSubKey("shell");
      var shellOpenKey = shellKey?.CreateSubKey("open");

      var shellOpenCommandKey = shellOpenKey?.CreateSubKey("command");
      shellOpenCommandKey?.SetValue(null, $"{exePath} \"%1\"");

      // clean up
      hkcu.Dispose();
      softwareKey?.Dispose(); 
      classesKey?.Dispose();
      extKey?.Dispose();
      nameKey?.Dispose();
      shellKey?.Dispose();
      shellOpenKey?.Dispose();
      shellOpenCommandKey?.Dispose();
    }

    public static void RemoveFileTypeAssociation(string ext, string name) {
      var hkcu = Registry.CurrentUser;
      var softwareKey = hkcu.OpenSubKey("Software");
      var classesKey = softwareKey?.OpenSubKey("Classes", RegistryKeyPermissionCheck.ReadWriteSubTree);

      classesKey?.DeleteSubKeyTree(ext, false);
      classesKey?.DeleteSubKeyTree(name, false);

      // clean up
      hkcu.Dispose();
      softwareKey?.Dispose();
      classesKey?.Dispose();
    }
  }
}