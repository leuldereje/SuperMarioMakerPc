using CommonLib.Components;

namespace Server.ViewModel {
  public class MainViewModel : CommonLib.Components.ViewModel {
    private int _selectedTabIndex;
    private bool _showSaveAndCloseButton;

    public MainViewModel() {
      SelectedTabIndex = 0;
    }

    // properties
    public int SelectedTabIndex {
      get => _selectedTabIndex;
      set {
        _selectedTabIndex = value;
        OnPropertyChanged();

        ProcessSelectedTabIndex();
      }
    }

    public bool ShowSaveAndCloseButton {
      get => _showSaveAndCloseButton;
      set {
        _showSaveAndCloseButton = value;
        OnPropertyChanged();
      }
    }

    // commands
    public Command SaveCommand {
      get {
        return BuildCommand(x => {
          // TODO: fill in
          //
          //
        });
      }
    }

    public Command ExportCommand {
      get {
        return BuildCommand(x => {
          // TODO: fill in
          //
          //
        });
      }
    }

    public Command ClearCommand {
      get {
        return BuildCommand(x => {
          // TODO: fill in
          //
          //
        });
      }
    }

    // methods
    private void ProcessSelectedTabIndex() {
      ShowSaveAndCloseButton = SelectedTabIndex == 0;
    }
  }
}