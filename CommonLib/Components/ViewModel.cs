using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommonLib.Annotations;

namespace CommonLib.Components {
  public class ViewModel : INotifyPropertyChanged {
    private readonly Dictionary<string, Command> _commands = new Dictionary<string, Command>();

    protected Command BuildCommand([CallerMemberName] string callerName = "") {
      return BuildCommand(new Command(), callerName);
    }

    protected Command BuildCommand(Action<object> execute, Predicate<object> canExecute = null, [CallerMemberName] string callerName = "") {
      var command = new Command(execute, canExecute);
      return BuildCommand(command, callerName);
    }

    public void EnableCommands() {
      foreach (var command in _commands.Values) {
        command.IsEnabled = true;
      }
    }

    public void DisableCommands() {
      foreach (var command in _commands.Values) {
        command.IsEnabled = false;
      }
    }

    // helper methods
    private Command BuildCommand(Command command, string callerName) {
      if (_commands.ContainsKey(callerName)) {
        return _commands[callerName];
      }

      _commands[callerName] = command;

      return command;
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void ForcePropertyChangeEvent(string propertyName = null) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName ?? string.Empty));
    }
  }
}