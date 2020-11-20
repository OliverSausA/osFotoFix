using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace osFotoFix.Views
{
    public class ImageView : UserControl
    {
        public ImageView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
  public class BitmapFileValueConverter : IValueConverter
  {
    public static BitmapFileValueConverter Instance = new BitmapFileValueConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture )
    {
      if( value == null)
        return null;
      
      try 
      {
        if( value is string filename && targetType == typeof(IBitmap))
        {
          var b = new Bitmap( filename );
          return b;
        }
      }
      catch {
      }
      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture )
    {
      throw new NotSupportedException();
    }
  }
}
