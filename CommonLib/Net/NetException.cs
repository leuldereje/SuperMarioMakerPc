using System;

namespace CommonLib.Net {
  public class NetException : Exception {
    public NetException(string message) : base(message) { }

    public NetException(string message, Exception innerException) : base(message, innerException) { }
  }
}