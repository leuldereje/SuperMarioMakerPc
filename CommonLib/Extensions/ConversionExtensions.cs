namespace CommonLib.Extensions {
  // see: https://social.msdn.microsoft.com/Forums/vstudio/en-US/c878e72e-d42e-417d-b4f6-1935ad96d8ae/converting-small-endian-to-big-endian-using-clong-value?forum=csharpgeneral
  public static class ConversionExtensions {
    public static ushort ToUInt16BE(this ushort value) {
      return (ushort) (((value & 0xff) << 8) | ((value >> 8) & 0xff));
    }

    public static uint ToUInt32BE(this uint value) {
      return (uint) (((((ushort) value).ToUInt16BE() & 0xffff) << 0x10) | (((ushort) (value >> 0x10)).ToUInt16BE() & 0xffff));
    }
  }
}