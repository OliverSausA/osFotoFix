using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace osFotoFix.ViewModels
{
  using System.Linq;
  using Models;
  using Services;
  public partial class MainWindowViewModel : ViewModelBase
  {
    public MainWindowViewModel()
    {
      Configure();
      CreateNavigation();
      NavigationList?.FirstOrDefault()?.Command?.Execute(null);
    }

    protected void Configure()
    {
      // var settingsService = App.Current.ServiceProvider.GetRequiredService<UserSettingsService>();
      // SettingsVM = new SettingsViewModel(settingsService);
    }
    
    [ObservableProperty]
    public string greeting = "Welcome to Avalonia!";
    
    [ObservableProperty]
    private List<NavigationItemVM> navigationList = new();

    [ObservableProperty]
    private SettingsViewModel? settingsVM;

    protected void CreateNavigation()
    {
      NavigationList.Clear();
      NavigationList.Add( new NavigationItemVM() {
        Title = "FotoFix",
        IconName = "FluentIcons.image_library_regular",
        Command = new RelayCommand(() => {
          MainViewModel = App.Current?.Services.GetRequiredService<MainFotoViewModel>() as ViewModelBase; 
        }),
      });
      NavigationList.Add( new NavigationItemVM() {
        Title = "Targets",
        IconName = "FluentIcons.target_edit_regular",
        Command = new RelayCommand(() => {
          MainViewModel = App.Current?.Services.GetRequiredService<TargetListViewModel>() as ViewModelBase; 
        }),
      });
    }

    [ObservableProperty]
    private ViewModelBase? mainViewModel;
    partial void OnMainViewModelChanging(ViewModelBase? value)
    {
      if (MainViewModel != null)
        MainViewModel.IsActive = false;
      if (value != null)
        value.IsActive = true;
    }
  }
}
