using System;
using System.Diagnostics;
using System.Reflection;

namespace CommonLib.Utils {
  public static class AssemblyUtils {
    public static string GetFullAssemblyPath() {
      return new Uri(Assembly.GetCallingAssembly().GetName().CodeBase).LocalPath;
    }

    public static string GetAssemblyFileVersion(string filePath = null) {
      FileVersionInfo versionInfo;

      if (string.IsNullOrWhiteSpace(filePath)) {
        var assembly = Assembly.GetCallingAssembly();
        versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
      }
      else {
        versionInfo = FileVersionInfo.GetVersionInfo(filePath);
      }

      var result = versionInfo.FileVersion;

      return result;
    }
  }
}