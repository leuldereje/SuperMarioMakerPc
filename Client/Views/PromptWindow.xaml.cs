using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Client.Annotations;
using Client.Components;

namespace Client.Views {
  public partial class PromptWindow : CustomWindow, INotifyPropertyChanged {
    private string _text;
    private bool _showOk;
    private bool _showYesNo;

    public PromptWindow() {
      InitializeComponent();
    }

    // properties
    public string Text {
      get => _text;
      set {
        _text = value;
        OnPropertyChanged();
      }
    }

    public MessageBoxResult Result { get; private set; }

    public bool ShowOk {
      get => _showOk;
      set {
        _showOk = value;
        OnPropertyChanged();
      }
    }

    public bool ShowYesNo {
      get => _showYesNo;
      set {
        _showYesNo = value;
        OnPropertyChanged();
      }
    }

    // static methods
    public static MessageBoxResult Show(string text, MessageBoxButton button) {
      var messageBox = new PromptWindow {
        Text = text
      };

      switch (button) {
        case MessageBoxButton.OK:
          messageBox.ShowOk = true;
          break;
        //case MessageBoxButton.OKCancel:
        //  break;
        //case MessageBoxButton.YesNoCancel:
        //  break;
        case MessageBoxButton.YesNo:
          messageBox.ShowYesNo = true;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(button), button, null);
      }

      messageBox.ShowDialog();

      return messageBox.Result;
    }

    // ui events
    private void OkClicked(object sender, RoutedEventArgs e) {
      Result = MessageBoxResult.OK;
      Close();
    }

    private void YesClicked(object sender, RoutedEventArgs e) {
      Result = MessageBoxResult.Yes;
      Close();
    }

    private void NoClicked(object sender, RoutedEventArgs e) {
      Result = MessageBoxResult.No;
      Close();
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}