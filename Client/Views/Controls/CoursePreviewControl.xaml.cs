using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Client.Views.Controls {
  public partial class CoursePreviewControl : Button {
    public CoursePreviewControl() {
      InitializeComponent();
    }

    // dependancy properties
    public static DependencyProperty ClickCommandProperty = DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(CoursePreviewControl));

    public ICommand ClickCommand {
      get => (ICommand) GetValue(ClickCommandProperty);
      set => SetValue(ClickCommandProperty, value);
    }

    public static DependencyProperty ClickCommandParameterProperty = DependencyProperty.Register("ClickCommandParameter", typeof(object), typeof(CoursePreviewControl));

    public object ClickCommandParameter {
      get => GetValue(ClickCommandParameterProperty);
      set => SetValue(ClickCommandParameterProperty, value);
    }

    public static DependencyProperty SaveCommandProperty = DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(CoursePreviewControl));

    public ICommand SaveCommand {
      get => (ICommand) GetValue(SaveCommandProperty);
      set => SetValue(SaveCommandProperty, value);
    }
    
    public static DependencyProperty SaveCommandParameterProperty = DependencyProperty.Register("SaveCommandParameter", typeof(object), typeof(CoursePreviewControl));

    public object SaveCommandParameter {
      get => GetValue(SaveCommandParameterProperty);
      set => SetValue(SaveCommandParameterProperty, value);
    }

    public static DependencyProperty ThumbnailWideProperty = DependencyProperty.Register("ThumbnailWide", typeof(ImageSource), typeof(CoursePreviewControl));

    public ImageSource ThumbnailWide {
      get => (ImageSource) GetValue(ThumbnailWideProperty);
      set => SetValue(ThumbnailWideProperty, value);
    }

    public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CoursePreviewControl));

    public string Title {
      get => (string) GetValue(TitleProperty);
      set => SetValue(TitleProperty, value);
    }

    // ui events
    private void ButtonClick(object sender, RoutedEventArgs e) {
      ClickCommand?.Execute(ClickCommandParameter);
    }

    private void SaveButtonPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
      SaveCommand?.Execute(SaveCommandParameter);
    }
  }
}