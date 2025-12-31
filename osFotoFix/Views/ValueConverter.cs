using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace osFotoFix.Views
{
  using Models;

  public class EFilterStatusCheckedConverter : IValueConverter
  {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture )
    {
      bool? ret = null;
      if (value is EFilterState state)
      {
        if( state == EFilterState.eOn )
          ret = true;
        else if ( state == EFilterState.eOff )
          ret = false;
      }
      return ret;
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture )
    {
      EFilterState ret = EFilterState.eDisable;
      if (value is bool bOn)
      {
        if( bOn )
          ret = EFilterState.eOn;
        else 
          ret = EFilterState.eOff;
      }
      return ret;
    }
  }

  public class EActionCompareConverter : IValueConverter
  {
    public static EActionCompareConverter Instance = new EActionCompareConverter();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture )
    {
      if( value == null || parameter == null )
        return null;
      
      try 
      {
        ///// string sValue = Enum.GetName( typeof(EAction), value );
        return parameter.ToString();
      }
      catch {
      }
      return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture )
    {
      if( value == null || parameter == null )
        return null;
      foreach( object o in Enum.GetValues(typeof(EAction)))
      {
        if( value.ToString() == o.ToString() )
          return true;
      }
      return false;
    }
  }

  public class BitmapFileValueConverter : IValueConverter
  {
    public static BitmapFileValueConverter Instance = new BitmapFileValueConverter();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture )
    {
      if( value == null)
        return null;
      
      try 
      {
        if( value is string filename && targetType.IsAssignableFrom(typeof(Bitmap)))
        {
          var b = new Bitmap( filename );
          return b;
        }
      }
      catch {
      }
      return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture )
    {
      throw new NotSupportedException();
    }
  }

  public class ActionColorValueConverter : IValueConverter
  {
    /*
      <SolidColorBrush x:Key="Cancel">#7F7FFF</SolidColorBrush>
      <SolidColorBrush x:Key="Warning">#FFF77F</SolidColorBrush>
      <SolidColorBrush x:Key="Alert">#FF7F7F</SolidColorBrush>
      <SolidColorBrush x:Key="OK_Color">#7FFF7F</SolidColorBrush>
    */
    public static ActionColorValueConverter Instance = new ActionColorValueConverter();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture )
    {
      if( value == null)
        return null;
      
      try 
      {
        if( value is EAction ) // && targetType == typeof(SolidColorBrush))
        {
          uint color;
          switch( (EAction) value )
          {
            case EAction.copy: 
            case EAction.move:   color = 0x7FFF7F; break;
            case EAction.trash:  color = 0xFFF77F; break;
            case EAction.delete: color = 0xFF7F7F; break;
            default: color = 0xCCDDEE; break;
          }
          return new SolidColorBrush( color ); 
        }
      }
      catch {
      }
      return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture )
    {
      throw new NotSupportedException();
    }
  }
  public class ActionBitmapValueConverter : IValueConverter
  {
    /*
      <SolidColorBrush x:Key="Cancel">#7F7FFF</SolidColorBrush>
      <SolidColorBrush x:Key="Warning">#FFF77F</SolidColorBrush>
      <SolidColorBrush x:Key="Alert">#FF7F7F</SolidColorBrush>
      <SolidColorBrush x:Key="OK_Color">#7FFF7F</SolidColorBrush>
    */
    public static ActionBitmapValueConverter Instance = new ActionBitmapValueConverter();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture )
    {
      if( value == null)
        return null;
      
      try 
      {
        if( value is EAction ) // is EAction && targetType == typeof(SolidColorBrush))
        {
          uint color;
          switch( (EAction) value )
          {
            case EAction.copy: 
            case EAction.move:   color = 0x7FFF7F; break;
            case EAction.trash:  color = 0xFFF77F; break;
            case EAction.delete: color = 0xFF7F7F; break;
            default: color = 0xAACCDD; break;
          }
          return new SolidColorBrush( color ); 
        }
      }
      catch {
      }
      return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture )
    {
      throw new NotSupportedException();
    }
  }
  public sealed class IconNameToPath : IValueConverter
  {
    public object? Convert( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
      if( value is string resourceName )
      {
        if (App.Current.TryFindResource(resourceName, out var iconPath))
        {
          var icon = iconPath as DrawingGroup;
          return icon;
        }
      }
      return new Avalonia.Data.BindingNotification(new InvalidCastException(), Avalonia.Data.BindingErrorType.Error);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
 
}