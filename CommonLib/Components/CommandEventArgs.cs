using System;

namespace CommonLib.Components {
  public class CommandEventArgs : EventArgs {
    public CommandEventArgs(object parameter) {
      Parameter = parameter;
    }

    public object Parameter { get; }

    public bool Handled { get; set; }
  }
}