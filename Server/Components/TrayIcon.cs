using System;
using System.Windows.Forms;
using CommonLib.Extensions;
using Server.Views;
using Application = System.Windows.Application;

namespace Server.Components {
  public class TrayIcon {
    private static readonly Lazy<TrayIcon> _lazy = new Lazy<TrayIcon>(() => new TrayIcon());
    private NotifyIcon _icon;

    private TrayIcon() { }

    // properties
    public static TrayIcon Instance => _lazy.Value;

    // methods
    public void Setup() {
      _icon = new NotifyIcon {
        Text = "SMM Server",
        Icon = Properties.Resources.icon,
        ContextMenu = new ContextMenu()
      };

      _icon.DoubleClick += IconClicked;
      
      _icon.ContextMenu.Add("Show", ShowClicked);
      _icon.ContextMenu.AddSeparator();
      _icon.ContextMenu.Add("Exit", Application.Current.Shutdown);

      _icon.Visible = true;
    }

    public void CleanUp() {
      _icon.Visible = false;
      _icon.Dispose();
    }
    
    // icon events
    private static void IconClicked(object sender, EventArgs e) {
      ShowClicked();
    }

    private static void ShowClicked() {
      new MainWindow().ShowDialog();
    }
  }
}