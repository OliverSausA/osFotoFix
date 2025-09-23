using System;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;

using osFotoFix.Services;
using osFotoFix.Models;
using System.Diagnostics;
using Avalonia.Controls;
using System.Collections.Generic;

namespace osFotoFix.ViewModels;

public class MainFotoViewModel : ViewModelBase
{
  public MainFotoViewModel()
  {
    FotoInfoService.GetDateTimeFromStringTests();
    var settingsService = App.Current.ServiceProvider.GetRequiredService<UserSettingsService>();
    SettingsVM = new SettingsViewModel(settingsService);
    ImageVM = new ImageViewModel(SettingsVM);
    FotoInfoDetailVM = new FotoInfoDetailViewModel();
    FotoPreviewListVM = new FotoPreviewViewModel();
    FotoInfoListVM = new FotoInfoListViewModel(SettingsVM, ImageVM, FotoPreviewListVM, FotoInfoDetailVM);

    var alt = Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
    alt.Exit += OnExit;
  }

  public SettingsViewModel SettingsVM { get; set; }

  public FotoInfoListViewModel FotoInfoListVM { get; set; }
  public FotoInfoDetailViewModel FotoInfoDetailVM { get; set; }
  public FotoPreviewViewModel FotoPreviewListVM { get; set; }
  public ImageViewModel ImageVM { get; set; }

  private void OnExit( object sender, ControlledApplicationLifetimeExitEventArgs e)
  {
    SettingsVM.SaveSettings();
  }
}
