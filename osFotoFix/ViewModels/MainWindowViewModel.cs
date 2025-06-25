using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;
  using Services;
  public partial class MainWindowViewModel : ViewModelBase
  {
    public MainWindowViewModel()
    {
      Configure();
      CreateNavigation();
    }

    protected void Configure()
    {
      var settingsService = App.Current.ServiceProvider.GetRequiredService<UserSettingsService>();
      SettingsVM = new SettingsViewModel(settingsService);
    }

    protected void CreateNavigation()
    {
      NavigationList = new List<NavigationItemVM>();
      NavigationList.Add( new NavigationItemVM() {
        Title = "FotoFix",
        IconName = "FluentIcons.image_library_regular",
        Command = ReactiveCommand.Create(() => {
          MainViewModel = App.Current.ServiceProvider.GetRequiredService<MainFotoViewModel>() as ViewModelBase; 
        }),
      });
      NavigationList.Add( new NavigationItemVM() {
        Title = "Targets",
        IconName = "FluentIcons.target_edit_regular",
        Command = ReactiveCommand.Create(() => {
          MainViewModel = App.Current.ServiceProvider.GetRequiredService<TargetListViewModel>() as ViewModelBase; 
        }),
      });
    }

    public List<NavigationItemVM> NavigationList { get; set; }

    public SettingsViewModel SettingsVM {get;set;}

    private ViewModelBase mainViewModel;
    public ViewModelBase MainViewModel
    {
      get { return mainViewModel; }
      set {
        if (mainViewModel != value) {
          if(mainViewModel != null)
            mainViewModel.IsActive = false;
          this.RaiseAndSetIfChanged(ref mainViewModel, value); }
          if( mainViewModel != null )
            mainViewModel.IsActive = true;
      }
    }
  }
}
