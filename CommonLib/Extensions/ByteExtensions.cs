using System;
using System.Linq;

namespace CommonLib.Extensions {
  public static class ByteExtensions {
    public static byte[] Slice(this byte[] bytes, int index, int size) {
      var result = new byte[size];
      Array.Copy(bytes, index, result, 0, size);
      return result;
    }

    public static byte[] FillWith(this byte[] bytes, params byte[][] data) {
      if (data.Length == 0) {
        return bytes;
      }

      var toCopy = data
        .SelectMany(x => x)
        .ToArray();

      if (toCopy.Length > bytes.Length) {
        throw new InvalidOperationException("Source is too small!");
      }

      Array.Copy(toCopy, bytes, toCopy.Length);

      return bytes;
    }
  }
}