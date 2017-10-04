using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace CommonLib.Utils {
  public static class ZipUtils {
    public static void ZipFiles(Dictionary<string, byte[]> files, string zipFilePath) {
      if (File.Exists(zipFilePath)) {
        File.Delete(zipFilePath);
      }

      using (var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Create)) {
        foreach (var key in files.Keys) {
          var entry = zip.CreateEntry(key);
          using (var stream = entry.Open()) {
            using (var ms = new MemoryStream(files[key])) {
              ms.CopyTo(stream);
            }
          }
        }
      }
    }

    public static Dictionary<string, byte[]> GetFiles(string zipFilePath) {
      var result = new Dictionary<string, byte[]>();

      using (var zip = ZipFile.OpenRead(zipFilePath)) {
        foreach (var entry in zip.Entries) {
          var key = entry.Name;
          var data = entry.GetEntryData();

          result.Add(key, data);
        }
      }

      return result;
    }

    private static byte[] GetEntryData(this ZipArchiveEntry entry) {
      byte[] result;

      using (var stream = entry.Open()) {
        using (var ms = new MemoryStream()) {
          stream.CopyTo(ms);

          result = ms.ToArray();
        }
      }

      return result;
    }
  }
}