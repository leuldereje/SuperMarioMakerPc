using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Client.Views.Controls {
  public partial class CourseThumbnailControl : Button {
    public CourseThumbnailControl() {
      InitializeComponent();
    }

    // dependancy properties
    public static DependencyProperty ClickCommandProperty = DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(CourseThumbnailControl));

    public ICommand ClickCommand {
      get => (ICommand) GetValue(ClickCommandProperty);
      set => SetValue(ClickCommandProperty, value);
    }

    public static DependencyProperty ClickCommandParameterProperty = DependencyProperty.Register("ClickCommandParameter", typeof(object), typeof(CourseThumbnailControl));

    public object ClickCommandParameter {
      get => GetValue(ClickCommandParameterProperty);
      set => SetValue(ClickCommandParameterProperty, value);
    }

    public static DependencyProperty ThumbnailProperty = DependencyProperty.Register("Thumbnail", typeof(ImageSource), typeof(CourseThumbnailControl));

    public ImageSource Thumbnail {
      get => (ImageSource) GetValue(ThumbnailProperty);
      set => SetValue(ThumbnailProperty, value);
    }

    public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CourseThumbnailControl));

    public string Title {
      get => (string) GetValue(TitleProperty);
      set => SetValue(TitleProperty, value);
    }

    // ui events
    private void ButtonClick(object sender, RoutedEventArgs e) {
      ClickCommand?.Execute(ClickCommandParameter);
    }
  }
}