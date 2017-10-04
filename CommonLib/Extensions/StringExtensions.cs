using System;
using System.Net;
using System.Text.RegularExpressions;

namespace CommonLib.Extensions {
  public static class StringExtensions {
    public static byte[] HexToBytes(this string str) {
      var chars = str.Length;
      var bytes = new byte[chars / 2];

      for (var i = 0; i < chars; i += 2) {
        bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
      }

      return bytes;
    }

    public static IPAddress ToIpAddress(this string str) {
      if (str.ToLower() == "localhost") {
        return IPAddress.Parse("127.0.0.1");
      }

      if (IPAddress.TryParse(str, out IPAddress ipAddress)) {
        return ipAddress;
      }
        
      var hostEntry = Dns.GetHostEntry(str);

      ipAddress = hostEntry.AddressList.Length > 1
        ? hostEntry.AddressList[0]
        : IPAddress.Any;

      return ipAddress;
    }

    public static bool IsNumeric(this string str) {
      var regex = new Regex("[^0-9]+");

      return !regex.IsMatch(str);
    }
  }
}