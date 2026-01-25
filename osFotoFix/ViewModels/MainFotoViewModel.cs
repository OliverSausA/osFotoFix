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

namespace osFotoFix.ViewModels;

using System.Linq;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using osFotoFix.Models;
using osFotoFix.Services;

/// <summary>
/// TODO: Hier wird die FotoInfoList und das FotoSelected verwaltet. Damit ist dieses ViewModel der Zentrale Kern.
/// </summary>
public partial class MainFotoViewModel : ViewModelBase
{
  public MainFotoViewModel( FotoInfoService fotoInfoService )
  {
    this.fotoInfoService = fotoInfoService;
    fotoInfoService.FotoInfoReadEvent += OnFotoInfoRead;
    fotoInfoService.FotoFixedEvent += OnFotoFixed;

    FotoInfoService.GetDateTimeFromStringTests();
    var settingsService = App.Current.Services.GetRequiredService<UserSettingsService>();
    SourcePath = settingsService.GetUserSettings.Source;
    PreviewSize = settingsService.GetUserSettings.PreviewSize;

    SelectPathCommand = new AsyncRelayCommand( async () => {
      var directoryPicker = new Services.DirectoryPicker();
      string path = await directoryPicker.GetFolderName( SourcePath );
      if( path != string.Empty ) {
        SourcePath = path;
      }
    });

    CreateMenuItems();

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

  protected virtual void CreateMenuItems()
  {
    MainMenuItems.Add(new MainMenuItemVM()
    {
      Title = "PreviewSizeMinus",
      IconName = "ZoomOut",
      //IconColor = "Green",
      Command = new RelayCommand( () => {
        if( PreviewSize >= 100 )
          PreviewSize -= 50;
      })
    });
    MainMenuItems.Add(new MainMenuItemVM()
    {
      Title = "PreviewSizePlus",
      IconName = "ZoomIn",
      //IconColor = "Green",
      Command = new RelayCommand( () => {
        if( PreviewSize <= 800 )
          PreviewSize += 50;
      })
    });
    MainMenuItems.Add(new MainMenuItemVM()
    {
      Title = "DoIt",
      IconName = "Save",
      IconColor = "Green",
      Command = new AsyncRelayCommand( async () => {
        await OnDoItAsync();
      })
    });
    MainMenuItems.Add(new MainMenuItemVM()
    {
      Title = "Cancel",
      IconName = "Cancel",
      IconColor = "Red",
      Command = new AsyncRelayCommand( async () => {
        await OnDoItAsync();
      })
    });

    var settingsService = App.Current.Services.GetRequiredService<UserSettingsService>();
    foreach( var target in settingsService.GetUserSettings.Targets)
    {
      if (!target.Enabled)
        continue;

      MainMenuItems.Add( new MainMenuItemVM()
      {
        Title = target.Title,
        IconName = target.IconName,
        IconColor = target.IconColor,
        Command = new RelayCommand( () =>
        {
          if( FotoSelected != null ) {
            OnTargetCommand( FotoSelected, target );
          }
        } )
      } );
    }
  }

  private FotoInfoService fotoInfoService;
  private CancellationTokenSource? doItCTS;

  [ObservableProperty]
  private string sourcePath = string.Empty;

  partial void OnSourcePathChanged( string value )
  {
    FotoInfoList.Clear();
    FotoSelected = null;
    // UserSettingsVM.ResetFilterStatistik();
    Task.Run( () => ReadFotoInfos( value ) );

    var settingsService = App.Current.Services.GetRequiredService<UserSettingsService>();
    settingsService.GetUserSettings.Source = SourcePath;
    settingsService.SaveUserSettings();
  }

  public AsyncRelayCommand SelectPathCommand { get; }

  private void OnTargetCommand( FotoInfoViewModel foto, Target target )
  {
    if( foto != null ) {
      foto.Target = target;
      var index = FotoInfoList.IndexOf( foto) +1;
      if ( index < FotoInfoList.Count )
      FotoSelected = FotoInfoList[ index ];
    }
  }

  private async Task OnDoItAsync()
  {
    RunningDoIt = true;
    Progress = 0;
    var progress = new Progress<double>( value => {
      Progress = value;
    } );

    doItCTS = new CancellationTokenSource();

    try
    {
      var fotoList = FotoInfoList.Select( f => f.Foto ).Where( f => f.ActionRequiered ).ToList();

      await fotoInfoService.FotoFixItAsync( 
        fotoList,
        progress,
        doItCTS.Token );
    }
    catch( OperationCanceledException )
    {
      // Ignore
    }
    finally
    {
      RunningDoIt = false;
      doItCTS = null;
    }
  }

  [ObservableProperty]
  private int previewSize;

  partial void OnPreviewSizeChanged( int value )
  {
    var settingsService = App.Current.Services.GetRequiredService<UserSettingsService>();
    settingsService.GetUserSettings.PreviewSize = PreviewSize;
    settingsService.SaveUserSettings();

    foreach( var fotoVM in FotoInfoList ) {
      fotoVM.PreviewSize =  PreviewSize;
    }
  }

  [ObservableProperty]
  private bool runningDoIt;

  [ObservableProperty]
  private double progress;

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

  private void OnFotoInfoRead( object? sender, FotoInfoEventArgs args )
  {
    Dispatcher.UIThread.Post( () => {
      if (args.FotoInfo == null) return;
      var fotoInfo = new FotoInfoViewModel( args.FotoInfo, PreviewSize );
      FotoInfoList.Add(fotoInfo);
      fotoInfo.Index = FotoInfoList.Count;
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
    Dispatcher.UIThread.Post( () => {
      if (args.FotoInfo == null) return;

      var fotoVM = FotoInfoList.Where( f => f.Foto.ID == args.FotoInfo.ID ).FirstOrDefault();
      // fotoVM?.UpdateView();
      FotoInfoList.Remove( fotoVM! );
      OnPropertyChanged(nameof(FotoInfoList));
    });
  }

  private void OnExit( object? sender, ControlledApplicationLifetimeExitEventArgs e)
  {
    // SettingsVM.SaveSettings();
  }
}