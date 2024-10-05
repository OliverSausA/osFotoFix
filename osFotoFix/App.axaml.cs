using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Globalization;
using osFotoFix.ViewModels;
using osFotoFix.Views;
using osFotoFix.Services;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using HarfBuzzSharp;

namespace osFotoFix
{
  public partial class App : Application
  {
    public override void Initialize()
    {
      AvaloniaXamlLoader.Load(this);

      var settings = UserSettingsService.GetInstance.GetUserSettings;
      SelectLanguage(settings.CultureId);

    }

    public override void OnFrameworkInitializationCompleted()
    {
      if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
      {
        desktop.MainWindow = new MainWindow
        {
          DataContext = new MainWindowViewModel(),
        };
      }

      base.OnFrameworkInitializationCompleted();
    }

    public static void SelectLanguage(string culture)
    {
      if( string.IsNullOrEmpty(culture))
        return;
      
      //Copy all MergedDictionarys into a auxiliar list.
      var dictionaryList = Application.Current.Resources.MergedDictionaries.ToList();

      //Search for the specified culture.     
      string requestedCulture = string.Format("StrRes.{0}.xaml", culture);
      foreach( var r in dictionaryList )
      {
        object language;
        r.TryGetResource("LanguageID", App.Current.ActualThemeVariant, out language);
        if (language != null && language.ToString() == culture)
        {
          Application.Current.Resources.MergedDictionaries.Remove(r);
          Application.Current.Resources.MergedDictionaries.Add(r);
        }
      }   

      //Inform the threads of the new culture.     
      Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
      Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
    }

    public static string GetStrRes(string key)
    {
      object res;
      if( App.Current.Resources.TryGetResource( key, ThemeVariant.Default, out res ) )
        return res.ToString();
      else
        return key;
    }
  }
}