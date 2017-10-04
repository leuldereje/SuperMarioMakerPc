using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Client.Views.Controls {
  public partial class CourseDetailsControl : UserControl {
    public CourseDetailsControl() {
      InitializeComponent();
    }

    // dependency properties
    public static DependencyProperty ThumbnailClickCommandProperty = DependencyProperty.Register("ThumbnailClickCommand", typeof(ICommand), typeof(CourseDetailsControl));

    public ICommand ThumbnailClickCommand {
      get => (ICommand) GetValue(ThumbnailClickCommandProperty);
      set => SetValue(ThumbnailClickCommandProperty, value);
    }

    public static DependencyProperty ThumbnailClickCommandParameterProperty = DependencyProperty.Register("ThumbnailClickCommandParameter", typeof(object), typeof(CourseDetailsControl));

    public object ThumbnailClickCommandParameter {
      get => GetValue(ThumbnailClickCommandParameterProperty);
      set => SetValue(ThumbnailClickCommandParameterProperty, value);
    }

    public static DependencyProperty ThumbnailProperty = DependencyProperty.Register("Thumbnail", typeof(ImageSource), typeof(CourseDetailsControl));

    public ImageSource Thumbnail {
      get => (ImageSource) GetValue(ThumbnailProperty);
      set => SetValue(ThumbnailProperty, value);
    }

    public static DependencyProperty ThumbnailWideProperty = DependencyProperty.Register("ThumbnailWide", typeof(ImageSource), typeof(CourseDetailsControl));

    public ImageSource ThumbnailWide {
      get => (ImageSource) GetValue(ThumbnailWideProperty);
      set => SetValue(ThumbnailWideProperty, value);
    }

    // ui events
    private void ThumbnailPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
      ThumbnailClickCommand?.Execute(ThumbnailClickCommandParameter);
    }
  }
}