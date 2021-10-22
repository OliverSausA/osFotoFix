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

namespace osFotoFix
{
  public class App : Application
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
      /*****
      string requestedCulture = string.Format("StrRes.{0}.xaml", culture);
      foreach( var r in dictionaryList )
      {
        var d = r as ResourceInclude;
        if( d.Source.OriginalString.Contains( requestedCulture ))
        {
          Application.Current.Resources.MergedDictionaries.Remove(d);
          Application.Current.Resources.MergedDictionaries.Add(d);
          break;
        }
      }   
      *****/

      //Search for the specified culture.     
      string requestedCulture = string.Format("StrRes.{0}.xaml", culture);
      var resourceDictionary = dictionaryList.
        Cast<ResourceInclude>().
        FirstOrDefault(d => d.Source.OriginalString.Contains(requestedCulture));

      //If we have the requested resource, remove it from the list and place at the end.     
      //Then this language will be our string table to use.      
      if (resourceDictionary != null)
      {
          Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
          Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
      }

      //Inform the threads of the new culture.     
      Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
      Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

    }

    public static string GetStrRes(string key)
    {
      object res;
      if( App.Current.Resources.TryGetResource( key, out res ) )
        return res.ToString();
      else
        return key;
    }
  }
}