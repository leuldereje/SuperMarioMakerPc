using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace Client.Components {
  public class ProcessManager {
    private static readonly Lazy<ProcessManager> _lazy = new Lazy<ProcessManager>(() => new ProcessManager());
    private readonly Dictionary<string, Process> _processes = new Dictionary<string, Process>();

    private ProcessManager() { }

    public static ProcessManager Instance => _lazy.Value;

    public Process Start(string key, string name, string arguments = null) {
      var process = Process.Start(name, arguments);

      if (process == null) {
        throw new ApplicationException("Process could not be tracked.");
      }

      process.EnableRaisingEvents = true;
      process.Exited += (s, e) => {
        Application.Current.Dispatcher.BeginInvoke((Action) (() => {
          ProcessExited(this, key);
        }));
      };

      if (!_processes.ContainsKey(key)) {
        _processes.Add(key, process);
      }
      else {
        _processes[key] = process;
      }

      return process;
    }

    public void Stop(string key) {
      if (!_processes.ContainsKey(key)) {
        return;
      }

      try {
        _processes[key].Kill();
      }
      catch {
        // ignored
      }

      _processes.Remove(key);
    }

    public Process GetProcess(string key) {
      return _processes[key];
    }

    // events
    public delegate void ProcessExitedEventHandler(object sender, string key);

    public event ProcessExitedEventHandler ProcessExited = delegate { };
  }
}