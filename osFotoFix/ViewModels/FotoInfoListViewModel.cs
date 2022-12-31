using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

using Avalonia.Threading;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;
  using Services;

  public class FotoInfoListViewModel : ViewModelBase
  {
    private FotoInfoService service;
    private FotoPreviewViewModel FotoPreviewVM;
    private FotoInfoDetailViewModel FotoInfoDetailVM;
    private ImageViewModel ImageVM;
    private List<FotoInfoVM> AllFotoInfos;
    public FotoInfoListViewModel( 
              SettingsViewModel UserSettingsVM,
              ImageViewModel ImageVM, 
              FotoPreviewViewModel FotoPreviewVM,
              FotoInfoDetailViewModel FotoInfoDetailVM )
    {
      AllFotoInfos = new List<FotoInfoVM>();
      FotoInfoList = new ObservableCollection<FotoInfoVM>();
      this.UserSettingsVM = UserSettingsVM;

      service = new FotoInfoService();
      service.FotoInfoReadEvent += OnFotoInfoRead;
      service.FotoFixedEvent += OnFotoFixed;

      this.ImageVM = ImageVM;
      this.ImageVM.UndoImageEvent += UndoFoto;
      this.ImageVM.NextImageEvent += SelectNextFoto;
      this.ImageVM.PrevImageEvent += SelectPrevFoto;
      this.ImageVM.TrashImageEvent += TrashFoto;
      this.ImageVM.DelImageEvent += DelFoto;
      this.ImageVM.CopyImageEvent += CopyFoto;
      this.ImageVM.MoveImageEvent += MoveFoto;

      RefreshCmd = ReactiveCommand.Create( OnRefresh );
      CancelCmd = ReactiveCommand.Create( OnCancel );
      UndoAllCmd = ReactiveCommand.Create( OnUndoAll );
      TrashAllCmd = ReactiveCommand.Create( OnTrashAll );
      DelAllCmd = ReactiveCommand.Create( OnDelAll );
      CopyAllCmd = ReactiveCommand.Create( OnCopyAll );
      MoveAllCmd = ReactiveCommand.Create( OnMoveAll );
      DoItCmd = ReactiveCommand.Create( OnDoIt );
      CancelDoItCmd = ReactiveCommand.Create( OnCancelDoIt );
      
      this.FotoPreviewVM = FotoPreviewVM;
      this.FotoInfoDetailVM = FotoInfoDetailVM;

      UserSettingsVM.NewSourceSelectedEvent += OnNewSourceSelected;
      OnNewSourceSelected( UserSettingsVM.Source );
      UserSettingsVM.FilterChangedEvent += OnFilterChanged;
    }
    
    public SettingsViewModel UserSettingsVM { get;set; }
    public ObservableCollection<FotoInfoVM> FotoInfoList { get; set; }

    private FotoInfoVM fotoSelected;
    public FotoInfoVM FotoSelected { 
      get { return fotoSelected; } 
      set { 
        this.RaiseAndSetIfChanged( ref fotoSelected, value );
        fotoSelected = value;
        ImageVM.Foto = value;
        FotoInfoDetailVM.Foto = value;
        FotoPreviewVM.FotoList.Clear();
        if(fotoSelected != null)
        {
          if( fotoSelected.Index >= 2)
            FotoPreviewVM.FotoList.Add(FotoInfoList[fotoSelected.Index -2]);
          if( fotoSelected.Index >= 1)
            FotoPreviewVM.FotoList.Add(FotoInfoList[fotoSelected.Index -1]);
          if( FotoInfoList.Count - fotoSelected.Index > 1)
            FotoPreviewVM.FotoList.Add(FotoInfoList[fotoSelected.Index +1]);
          if( FotoInfoList.Count - fotoSelected.Index > 2)
            FotoPreviewVM.FotoList.Add(FotoInfoList[fotoSelected.Index +2]);
        }
      } 
    }

    private void OnNewSourceSelected( string source ) 
    {
      AllFotoInfos.Clear();
      FotoInfoList.Clear();
      FotoSelected = null;
      UserSettingsVM.ResetFilterStatistik();
      Task.Run( () => ReadFotoInfos( source ) );
    }

    private bool runningReadFoto;
    public bool RunningReadFoto {
      get { return runningReadFoto; }
      set { this.RaiseAndSetIfChanged( ref runningReadFoto, value ); }
    }
    private CancellationTokenSource CancelReadFotoInfos;
    private async void ReadFotoInfos( string source )
    {
      if ( CancelReadFotoInfos != null ) return;
      if( string.IsNullOrEmpty( source ) ) return;
      var baseDir = new DirectoryInfo( source );
      if( !baseDir.Exists ) return;

      CancelReadFotoInfos = new CancellationTokenSource();
      RunningReadFoto = true;

      FotoSelected = null;
      await service.ReadFotoInfos( baseDir, CancelReadFotoInfos.Token );

      RunningReadFoto = false;
      CancelReadFotoInfos = null;
    }

    private void OnFotoInfoRead( object sender, FotoInfoEventArgs args )
    {
      Dispatcher.UIThread.InvokeAsync( () => {
        var fotoInfo = new FotoInfoVM( args.FotoInfo );
        AllFotoInfos.Add( fotoInfo );
        if( FilterMatch( fotoInfo ) )
        {
          fotoInfo.Index = FotoInfoList.Count;
          FotoInfoList.Add( fotoInfo );
          if( FotoInfoList.Count == 1 )
            FotoSelected = fotoInfo;
        }
      });
    }
    private bool FilterMatch( FotoInfoVM fotoInfo ) 
    {
      bool bMatch = true;

      if( fotoInfo.Foto.Action == EAction.done )
        bMatch = false;

      if( fotoInfo.Foto.TypeOfCreationDate == FotoInfo.ETypeOfCreationDate.Exif ) {
        UserSettingsVM.FilterDatumExifCount++;
        if( UserSettingsVM.FilterDatumExif == EFilterState.eOff )
          bMatch = false;
      }
      else
      {
        if( UserSettingsVM.FilterDatumExif == EFilterState.eOn )
          bMatch = false;
      }

      if( fotoInfo.Foto.TypeOfCreationDate == FotoInfo.ETypeOfCreationDate.Filename ) {
        UserSettingsVM.FilterDatumFilenameCount++;
        if( UserSettingsVM.FilterDatumFilename == EFilterState.eOff )
          bMatch = false;
      }
      else
      {
        if( UserSettingsVM.FilterDatumFilename == EFilterState.eOn )
          bMatch = false;
      }

      if( fotoInfo.Foto.TypeOfCreationDate == FotoInfo.ETypeOfCreationDate.Filesystem ) {
        UserSettingsVM.FilterDatumFilechangedCount++;
        if( UserSettingsVM.FilterDatumFilechanged == EFilterState.eOff )
          bMatch = false;
      }
      else
      {
        if( UserSettingsVM.FilterDatumFilechanged == EFilterState.eOn )
          bMatch = false;
      }

      if( fotoInfo.Foto.File.Name.StartsWith( ".trash" ) ) {
        UserSettingsVM.FilterFilenameTrashedCount++;
        if( UserSettingsVM.FilterFilenameTrashed == EFilterState.eOff )
          bMatch = false;
      }
      else
      {
        if( UserSettingsVM.FilterFilenameTrashed == EFilterState.eOn )
          bMatch = false;
      }

      if( fotoInfo.Foto.FileExistsOnTarget )
        UserSettingsVM.FilterDoublesCount++;

      if( UserSettingsVM.FilterDoubles == EFilterState.eOn )
        bMatch &= fotoInfo.Foto.FileExistsOnTarget;
      if( UserSettingsVM.FilterDoubles == EFilterState.eOff )
        bMatch &= !fotoInfo.Foto.FileExistsOnTarget;

      return bMatch;
    }

    private void OnFilterChanged() 
    {
      FotoInfoList.Clear();
      UserSettingsVM.ResetFilterStatistik();
      var s = FotoSelected;
      FotoSelected = null;
      foreach( var item in AllFotoInfos.Where( x => FilterMatch( x ) ) ) {
        item.Index = FotoInfoList.Count;
        FotoInfoList.Add( item );
        if( item == s ) FotoSelected = item;
      }
      if( FotoSelected == null )
        SelectFirstFoto();
    }

    private void OnFotoFixed( object sender, FotoInfoEventArgs args )
    {
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
    }

    protected void SelectFirstFoto()
    {
      if( FotoInfoList.Count > 0 )
        FotoSelected = FotoInfoList[0];
    }
    protected void SelectNextFoto()
    {
      int idx = 0;
      if( fotoSelected != null )
        idx = fotoSelected.Index +1;
      if( idx >= 0 && idx < FotoInfoList.Count )
        FotoSelected = FotoInfoList[ idx ];
    }
    protected void SelectPrevFoto()
    {
      int idx = FotoInfoList.Count -1;
      if( fotoSelected != null )
        idx = fotoSelected.Index -1;
      if( idx >= 0 && idx < FotoInfoList.Count )
        FotoSelected = FotoInfoList[ idx ];
    }

    public ReactiveCommand<Unit, Unit> RefreshCmd { get; }
    public void OnRefresh() {
      OnNewSourceSelected( UserSettingsVM.Source );
    }

    public ReactiveCommand<Unit, Unit> CancelCmd { get; }
    public void OnCancel() {
      if( CancelReadFotoInfos != null )
      {
        CancelReadFotoInfos.Cancel();
        RunningReadFoto = false;
      }
    }

    public ReactiveCommand<Unit, Unit> UndoAllCmd { get; }
    public void OnUndoAll() {
      foreach( var foto in FotoInfoList )
        UndoFoto( foto );
    }

    public ReactiveCommand<Unit, Unit> TrashAllCmd { get; }
    public void OnTrashAll() {
      foreach( var foto in FotoInfoList )
        TrashFoto( foto );
    }

    public ReactiveCommand<Unit, Unit> DelAllCmd { get; }
    public void OnDelAll() {
      foreach( var foto in FotoInfoList )
        DelFoto( foto );
    }

    public ReactiveCommand<Unit, Unit> CopyAllCmd { get; }
    public void OnCopyAll() {
      foreach( var foto in FotoInfoList )
        CopyFoto( foto );
    }

    public ReactiveCommand<Unit, Unit> MoveAllCmd { get; }
    public void OnMoveAll() {
      foreach( var foto in FotoInfoList )
        MoveFoto( foto );
    }

    private bool runningFotoFixIt;
    public bool RunningFotoFixIt {
      get { return runningFotoFixIt; }
      set { this.RaiseAndSetIfChanged( ref runningFotoFixIt, value ); }
    }

    public ReactiveCommand<Unit, Unit> DoItCmd { get; }
    private CancellationTokenSource CancelFotoFixIt;
    public void OnDoIt()
    {
      Task.Run( () => DoIt() );
    }
    public async void DoIt() {
      if( CancelFotoFixIt != null ) return;
      CancelFotoFixIt = new CancellationTokenSource();
      RunningFotoFixIt = true;
      var list = FotoInfoList.Select( f => f.Foto ).Where( r => r.ActionRequiered == true );
      var ok = await service.FotoFixIt( list, CancelFotoFixIt.Token );
      RunningFotoFixIt = false;
      CancelFotoFixIt = null;
    }
    public ReactiveCommand<Unit, Unit> CancelDoItCmd { get; }
    public void OnCancelDoIt()
    {
      if( CancelFotoFixIt != null )
      {
        CancelFotoFixIt.Cancel();
        RunningFotoFixIt = false;
      }
    }

    protected void UndoFoto()
    {
      UndoFoto( fotoSelected );
      SelectNextFoto();
    }
    protected void UndoFoto( FotoInfoVM foto )
    {
      if( foto == null ) return;
      if( foto.Action == EAction.done ) return;
      foto.Action = EAction.ignore;
      foto.Target = "";
      foto.Title = "";
      foto.Description = "";
      foto.NewFileName = "";
      foto.FileExistsOnTarget = false;
      foto.UpdateView();
    }

    protected void DelFoto()
    {
      DelFoto( fotoSelected );
      SelectNextFoto();
    }
    protected void DelFoto( FotoInfoVM foto )
    {
      if( foto == null ) return;
      if( foto.Action == EAction.done ) return;
      UndoFoto( foto );
      foto.Action = EAction.delete;
    }

    protected void CopyFoto()
    {
      CopyFoto( fotoSelected );
      SelectNextFoto();
    }
    protected void CopyFoto( FotoInfoVM foto )
    {
      if( foto == null ) return;
      if( foto.Action == EAction.done ) return;
      foto.Action = EAction.copy;
      foto.Target = UserSettingsVM.Target;
      foto.Title = UserSettingsVM.Title;
      foto.Description = UserSettingsVM.Description;
      foto.NewFileName = service.GetNewFileName( foto.Foto );
      foto.UpdateView();
    }

    protected void MoveFoto()
    {
      MoveFoto( fotoSelected );
      SelectNextFoto();
    }
    protected void MoveFoto( FotoInfoVM foto )
    {
      if( foto == null ) return;
      if( foto.Action == EAction.done ) return;
      foto.Action = EAction.move;
      foto.Target = UserSettingsVM.Target;
      foto.Title = UserSettingsVM.Title;
      foto.Description = UserSettingsVM.Description;
      foto.NewFileName = service.GetNewFileName( foto.Foto );
      foto.UpdateView();
    }

    protected void TrashFoto()
    {
      TrashFoto( fotoSelected );
      SelectNextFoto();
    }
    protected void TrashFoto( FotoInfoVM foto )
    {
      if( foto == null ) return;
      if( foto.Action == EAction.done ) return;
      foto.Action = EAction.trash;
      foto.Target = UserSettingsVM.Trash;
      foto.Title = UserSettingsVM.Title;
      foto.Description = UserSettingsVM.Description;
      foto.NewFileName = service.GetNewFileName( foto.Foto );
      foto.UpdateView();
    }

    /*****
    protected void DoIt( FotoInfoVM foto )
    {
      try
      {
        if( (foto.Foto.TypeOfCreationDate == FotoInfo.ETypeOfCreationDate.Filesystem) )
        {
          foto.Comment = "Exif Infomation ist ungültig!";
          foto.Action = EAction.failed;
        }
        else if( foto.Action == EAction.copy ) {
          foto.NewFileName = service.CopyFoto( foto.Foto );
          foto.Action = EAction.done;
        }
        else if( foto.Action == EAction.move ) {
          foto.NewFileName = service.MoveFoto( foto.Foto );
          foto.Action = EAction.done;
        }
        else if( foto.Action == EAction.trash ) {
          foto.NewFileName = service.MoveFoto( foto.Foto );
          foto.Action = EAction.done;
        }
        else if( foto.Action == EAction.delete ) {
          service.DeleteFoto( foto.Foto );
          foto.Action = EAction.done;
        }
      }
      catch ( Exception e )
      {
        foto.Action = EAction.failed;
        foto.Comment = e.Message;
      }

    }
    *****/
  }
}