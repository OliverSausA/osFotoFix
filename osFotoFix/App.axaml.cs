using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace osFotoFix;

using osFotoFix.Services;
using osFotoFix.ViewModels;
using osFotoFix.Views;

public sealed partial class App : Application
{
  public IServiceProvider Services { get; private set; }

  public App()
  {
    Services = ConfigureServices();
  }

  public override void Initialize()
  {
    AvaloniaXamlLoader.Load(this);
  }  
  
  public new static App Current => (App)(Application.Current ?? throw new InvalidOperationException());

  public override void OnFrameworkInitializationCompleted()
  {
    var settings = App.Current.Services.GetService<UserSettingsService>()!.GetUserSettings;
    SelectLanguage(settings.CultureId);

    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
      // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
      // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
      DisableAvaloniaDataAnnotationValidation();
      desktop.MainWindow = new MainWindow
      {
        DataContext = new MainWindowViewModel(),
      };
    }

    base.OnFrameworkInitializationCompleted();
  }

  private void DisableAvaloniaDataAnnotationValidation()
  {
    // Get an array of plugins to remove
    var dataValidationPluginsToRemove =
        BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

    // remove each entry found
    foreach (var plugin in dataValidationPluginsToRemove)
    {
      BindingPlugins.DataValidators.Remove(plugin);
    }
  }

  public static void SelectLanguage(string culture)
  {
    /*****
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
    *****/
  }

  public static string GetStrRes(string key)
  {
    object? res;
    if( App.Current.Resources.TryGetResource( key, ThemeVariant.Default, out res ))
      return res?.ToString() ?? key;
    else
      return key;
    }

  public Window? GetMainWindow()
  {
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
      return desktop.MainWindow;
    else
      return null;
  }

  public IServiceProvider ConfigureServices()
  {
    var services = new ServiceCollection();

    // Services
    services.AddSingleton<ExifService>();
    services.AddSingleton<FotoInfoService>();
    services.AddSingleton<UserSettingsService>();

    // ViewModels
    services.AddTransient<MainFotoViewModel>();
    services.AddTransient<TargetListViewModel>();
    services.AddTransient<SettingsViewModel>();

    return services.BuildServiceProvider();
  }
}