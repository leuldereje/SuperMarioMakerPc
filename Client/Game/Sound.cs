// ReSharper disable MemberCanBePrivate.Global
namespace Client.Game {
  public class Sound {
    public Sound(byte[] data) {
      Data = data;
      X = Data[Constants.Sound.LOC_X_OFFSET];
      Y = Data[Constants.Sound.LOC_Y_OFFSET];
      SoundType = Data[Constants.Sound.TYPE_OFFSET];
      Variation = Data[Constants.Sound.VARIATION_OFFSET];
    }

    public byte[] Data { get; }

    public byte X { get; }

    public byte Y { get; }

    public byte SoundType { get; }

    public byte Variation { get; }
  }
}