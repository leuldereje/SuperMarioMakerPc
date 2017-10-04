using System;
using System.Windows.Input;

namespace CommonLib.Components {
  public class Command : ICommand {
    private readonly Predicate<object> _canExecute;
    private readonly Action<object> _execute;

    public Command(Action<object> execute = null, Predicate<object> canExecute = null) {
      _canExecute = canExecute;
      _execute = execute;
    }

    // properties
    public bool IsEnabled { get; set; } = true;

    // methods
    public bool CanExecute(object parameter) {
      return _canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object parameter = null) {
      if (!IsEnabled) {
        return;
      }

      var args = new CommandEventArgs(parameter);

      PreExecute(this, args);

      if (args.Handled) {
        return;
      }

      _execute?.Invoke(parameter);

      PostExecute(this, args);
    }

    // events
    public event EventHandler CanExecuteChanged {
      add => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public delegate void CommandEventHandler(object sender, CommandEventArgs e);

    public event CommandEventHandler PreExecute = delegate { };

    public event CommandEventHandler PostExecute = delegate { };
  }
}