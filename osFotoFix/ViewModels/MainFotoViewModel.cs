using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

using osFotoFix.Models;
using osFotoFix.Services;

namespace osFotoFix.ViewModels;

/// <summary>
/// TODO: Hier wird die FotoInfoList und das FotoSelected verwaltet. Damit ist dieses ViewModel der Zentrale Kern.
/// </summary>
public partial class MainFotoViewModel : ViewModelBase
{
  public MainFotoViewModel()
  {
    fotoInfoService = new FotoInfoService();
    fotoInfoService.FotoInfoReadEvent += OnFotoInfoRead;
    fotoInfoService.FotoFixedEvent += OnFotoFixed;

    FotoInfoService.GetDateTimeFromStringTests();
    // var settingsService = App.Current.Services.GetRequiredService<UserSettingsService>();
    // SettingsVM = new SettingsViewModel(settingsService);

    var settingsService = UserSettingsService.GetInstance;
    OnNewSourceSelected( settingsService.GetUserSettings.Quelle);
    /*
    FileInfo fileInfo = new FileInfo("Test.jpg");
    var fotoInfo = new FotoInfo(fileInfo, DateTime.Now, ETypeOfCreationDate.Filesystem);
    FotoInfoList.Add(fotoInfo);
    FotoSelected = fotoInfo;
    */

    /*****
    ImageVM = new ImageViewModel(SettingsVM);
    FotoInfoDetailVM = new FotoInfoDetailViewModel();
    FotoPreviewListVM = new FotoPreviewViewModel();
    FotoInfoListVM = new FotoInfoListViewModel(SettingsVM, ImageVM, FotoPreviewListVM, FotoInfoDetailVM);
    *****/

    var alt = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
    if (alt != null) alt.Exit += OnExit;
  }

  private FotoInfoService fotoInfoService;

  [ObservableProperty]
  private ObservableCollection<FotoInfoViewModel> fotoInfoList = new();

  [ObservableProperty]
  private FotoInfoViewModel? fotoSelected = null;

  [ObservableProperty]
  private bool runningReadFoto;

  // public SettingsViewModel SettingsVM { get; set; }

  ///// public FotoInfoListViewModel FotoInfoListVM { get; set; }
  ///// public FotoInfoDetailViewModel FotoInfoDetailVM { get; set; }
  ///// public FotoPreviewViewModel FotoPreviewListVM { get; set; }
  ///// public ImageViewModel ImageVM { get; set; }

  private CancellationTokenSource? CancelReadFotoInfos;
  private async void ReadFotoInfos( string source )
  {
    if ( CancelReadFotoInfos != null ) return;
    if( string.IsNullOrEmpty( source ) ) return;
    var baseDir = new DirectoryInfo( source );
    if( !baseDir.Exists ) return;

    CancelReadFotoInfos = new CancellationTokenSource();
    RunningReadFoto = true;

    FotoSelected = null;
    await fotoInfoService.ReadFotoInfos( baseDir, CancelReadFotoInfos.Token );

    RunningReadFoto = false;
    CancelReadFotoInfos = null;
  }

  private void OnNewSourceSelected( string source ) 
  {
    FotoInfoList.Clear();
    FotoSelected = null;
    // UserSettingsVM.ResetFilterStatistik();
    Task.Run( () => ReadFotoInfos( source ) );
  }

  private void OnFotoInfoRead( object? sender, FotoInfoEventArgs args )
  {
    Dispatcher.UIThread.Post( () => {
      if (args.FotoInfo == null) return;
      var fotoInfo = new FotoInfoViewModel( args.FotoInfo );
      FotoInfoList.Add(fotoInfo);
      OnPropertyChanged(nameof(FotoInfoList));
      /*

      var fotoInfo = new FotoInfoViewModel( args.FotoInfo );
      AllFotoInfos.Add( fotoInfo );
      if( FilterMatch( fotoInfo ) )
      {
        fotoInfo.Index = FotoInfoList.Count;
        FotoInfoList.Add( fotoInfo );
        if( FotoInfoList.Count == 1 )
          FotoSelected = fotoInfo;
      }
      */
    });
  }

  private void OnFotoFixed( object? sender, FotoInfoEventArgs args )
  {
    /*
    Dispatcher.UIThread.InvokeAsync( () => {
      if( args.FotoInfo != null )
      {
        var fotoVM = FotoInfoList.Where( f => f.Foto.ID == args.FotoInfo.ID ).First();
        fotoVM.UpdateView();
      }
      else
      {
        OnFilterChanged();
      }
    });
    */
  }

  private void OnExit( object? sender, ControlledApplicationLifetimeExitEventArgs e)
  {
    // SettingsVM.SaveSettings();
  }
}