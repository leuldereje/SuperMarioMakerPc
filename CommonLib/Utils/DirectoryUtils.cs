using System.IO;

namespace CommonLib.Utils {
  public static class DirectoryUtils {
    public static void ClearDirectory(string directoryPath) {
      foreach (var file in Directory.GetFiles(directoryPath)) {
        File.Delete(file);
      }

      foreach (var directory in Directory.GetDirectories(directoryPath)) {
        Directory.Delete(directory, true);
      }
    }
  }
}