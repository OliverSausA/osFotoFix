using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;
  using Services;
  public partial class MainWindowViewModel : ViewModelBase
  {
    public MainWindowViewModel()
    {
      CreateNavigation();
      /*
      FotoInfoService.GetDateTimeFromStringTests();
      SettingsVM = new SettingsViewModel();
      ImageVM = new ImageViewModel( SettingsVM );
      FotoInfoDetailVM = new FotoInfoDetailViewModel();
      FotoPreviewListVM = new FotoPreviewViewModel();
      FotoInfoListVM = new FotoInfoListViewModel( SettingsVM, ImageVM, FotoPreviewListVM, FotoInfoDetailVM );

      var alt = Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
      alt.Exit += OnExit;
      */
    }

    protected void CreateNavigation()
    {
      NavigationList = new List<NavigationItemVM>();
      NavigationList.Add( new NavigationItemVM() {
        Title = "FotoFix",
        IconName = "image_library_regular"
      });
      NavigationList.Add( new NavigationItemVM() {
        Title = "Targets",
        IconName = "target_edit_regular"
      });
    }

    public List<NavigationItemVM> NavigationList { get; set; }

    private void OnExit( object sender, ControlledApplicationLifetimeExitEventArgs e)
    {
      SettingsVM.SaveSettings();
    }

    public SettingsViewModel SettingsVM {get;set;}

    public FotoInfoListViewModel FotoInfoListVM {get;set;}

    public FotoInfoDetailViewModel FotoInfoDetailVM {get;set;}

    public FotoPreviewViewModel FotoPreviewListVM {get;set;}

    public ImageViewModel ImageVM {get;set;}

  }
}
