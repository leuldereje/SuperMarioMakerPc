using System;
using System.Windows.Forms;

namespace Client.Components {
  public class KeyboardHookEventArgs : EventArgs {
    public KeyboardHookEventArgs(int keyValue, bool shiftDown, bool controlDown, bool altDown) {
      Key = (Keys) keyValue;

      KeyChar = (char) Key;

      if (char.IsLetter(KeyChar)) {
        KeyChar = shiftDown 
          ? char.ToUpper(KeyChar) 
          : char.ToLower(KeyChar);
      }

      Shift = shiftDown;
      Control = controlDown;
      Alt = altDown;
    }

    public Keys Key { get; set; }

    public int KeyValue => (int) Key;

    public char KeyChar { get; set; }

    public bool Shift { get; set; }

    public bool Control { get; set; }

    public bool Alt { get; set; }

    public bool Handled { get; set; }
  }
}