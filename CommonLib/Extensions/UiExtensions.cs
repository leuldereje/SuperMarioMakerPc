using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CommonLib.Extensions {
  public static class UiExtensions {
    public static void AddSeparator(this ContextMenu contextMenu) {
      contextMenu.MenuItems.Add("-");
    }

    public static void Add(this ContextMenu contextMenu, string text, Action action) {
      var mi = new MenuItem {
        Text = text
      };

      mi.Click += (s, e) => action();

      contextMenu.MenuItems.Add(mi);
    }

    public static BitmapImage ToBitmapImage(this byte[] bytes) {
      if (bytes == null || bytes.Length == 0) {
        return null;
      }

      var result = new BitmapImage();

      using (var mem = new MemoryStream(bytes)) {
        mem.Position = 0;
        result.BeginInit();
        result.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
        result.CacheOption = BitmapCacheOption.OnLoad;
        result.UriSource = null;
        result.StreamSource = mem;
        result.EndInit();
      }

      result.Freeze();

      return result;
    }

    public static void SaveAsJpg(this BitmapImage image, string filePath) {
      var encoder = new JpegBitmapEncoder();
      encoder.Frames.Add(BitmapFrame.Create(image));

      using (var fs = new FileStream(filePath, FileMode.Create)) {
        encoder.Save(fs);
      }
    }

    public static T FindParent<T>(this DependencyObject dependencyObject) where T : DependencyObject {
      var parent = VisualTreeHelper.GetParent(dependencyObject);

      if (parent == null) {
        return null;
      }

      var result = parent as T ?? FindParent<T>(parent);

      return result;
    }
  }
}