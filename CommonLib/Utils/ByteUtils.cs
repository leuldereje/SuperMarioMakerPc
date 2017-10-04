using CommonLib.Extensions;

namespace CommonLib.Utils {
  public static class ByteUtils {
    public static byte[] Allocate(int length, params byte[][] data) {
      var result = new byte[length];

      result.FillWith(data);

      return result;
    }
  }
}