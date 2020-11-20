using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;
  using Services;

  public class FotoInfoListViewModel : ViewModelBase
  {
    private FotoInfoService service;
    private FotoInfoDetailViewModel FotoInfoDetailVM;
    private ImageViewModel ImageVM;
    public FotoInfoListViewModel( 
              SettingsViewModel UserSettingsVM,
              ImageViewModel ImageVM, 
              FotoInfoDetailViewModel FotoInfoDetailVM )
    {
      this.UserSettingsVM = UserSettingsVM;

      service = new FotoInfoService();
      this.ImageVM = ImageVM;
      this.ImageVM.UndoImageEvent += UndoFoto;
      this.ImageVM.NextImageEvent += SelectNextFoto;
      this.ImageVM.PrevImageEvent += SelectPrevFoto;
      this.ImageVM.TrashImageEvent += TrashFoto;
      this.ImageVM.DelImageEvent += DelFoto;
      this.ImageVM.CopyImageEvent += CopyFoto;
      this.ImageVM.MoveImageEvent += MoveFoto;

      UndoAllCmd = ReactiveCommand.Create( OnUndoAll );
      TrashAllCmd = ReactiveCommand.Create( OnTrashAll );
      DelAllCmd = ReactiveCommand.Create( OnDelAll );
      CopyAllCmd = ReactiveCommand.Create( OnCopyAll );
      MoveAllCmd = ReactiveCommand.Create( OnMoveAll );
      DoItCmd = ReactiveCommand.Create( OnDoIt );
      
      this.FotoInfoDetailVM = FotoInfoDetailVM;

      UserSettingsVM.NewSourceSelectedEvent += OnNewSourceSelected;
      OnNewSourceSelected( UserSettingsVM.Source );
    }
    
    public SettingsViewModel UserSettingsVM { get;set; }
    public ObservableCollection<FotoInfoVM> FotoInfoList { get; set; }

    public string EventName {get;set;}
    public string Description {get;set;}

    private FotoInfoVM fotoSelected;
    public FotoInfoVM FotoSelected { 
      get { return fotoSelected; } 
      set { 
        this.RaiseAndSetIfChanged( ref fotoSelected, value );
        fotoSelected = value;
        ImageVM.Foto = value;
        FotoInfoDetailVM.Foto = value;
      } 
    }

    private void OnNewSourceSelected( string source ) 
    {
      FotoInfoList = new ObservableCollection<FotoInfoVM>();
      var baseDir = new DirectoryInfo( UserSettingsVM.Source );
      var fotos = service.GetFotoInfos( baseDir );

      int index = 0;
      foreach( var foto in fotos )
      {
        FotoInfoList.Add( new FotoInfoVM( foto, index++ ) );
      }
      this.RaisePropertyChanged( nameof( FotoInfoList ) );
      SelectFirstFoto();
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


    public ReactiveCommand<Unit, Unit> DoItCmd { get; }
    public void OnDoIt() {
      foreach( var foto in FotoInfoList )
        DoIt( foto );
    }

    protected void UndoFoto()
    {
      UndoFoto( fotoSelected );
      SelectNextFoto();
    }
    protected void UndoFoto( FotoInfoVM foto )
    {
      if( foto == null ) return;
      if( foto.Action == FotoInfoVM.EAction.done ) return;
      foto.Action = FotoInfoVM.EAction.ignore;
      foto.NewFileLocation = "";
      foto.NewFileName = "";
    }

    protected void DelFoto()
    {
      DelFoto( fotoSelected );
      SelectNextFoto();
    }
    protected void DelFoto( FotoInfoVM foto )
    {
      if( foto == null ) return;
      if( foto.Action == FotoInfoVM.EAction.done ) return;
      foto.Action = FotoInfoVM.EAction.delete;
      foto.NewFileLocation = "";
      foto.NewFileName = "";
    }

    protected void CopyFoto()
    {
      CopyFoto( fotoSelected );
      SelectNextFoto();
    }
    protected void CopyFoto( FotoInfoVM foto )
    {
      if( foto == null ) return;
      if( foto.Action == FotoInfoVM.EAction.done ) return;
      foto.Action = FotoInfoVM.EAction.copy;
      foto.NewFileLocation = CreateNewFileLocation( foto.Foto, EventName );
      foto.NewFileName = CreateNewFileName( foto.Foto, Description );
    }

    protected void MoveFoto()
    {
      MoveFoto( fotoSelected );
      SelectNextFoto();
    }
    protected void MoveFoto( FotoInfoVM foto )
    {
      if( foto == null ) return;
      if( foto.Action == FotoInfoVM.EAction.done ) return;
      foto.Action = FotoInfoVM.EAction.move;
      foto.NewFileLocation = CreateNewFileLocation( foto.Foto, EventName );
      foto.NewFileName = CreateNewFileName( foto.Foto, Description );
    }

    protected void TrashFoto()
    {
      TrashFoto( fotoSelected );
      SelectNextFoto();
    }
    protected void TrashFoto( FotoInfoVM foto )
    {
      if( foto == null ) return;
      if( foto.Action == FotoInfoVM.EAction.done ) return;
      foto.Action = FotoInfoVM.EAction.trash;
      foto.NewFileLocation = CreateNewFileLocation( foto.Foto, EventName );
      foto.NewFileName = CreateNewFileName( foto.Foto, Description );
    }

    protected string CreateNewFileName( FotoInfo foto, string description )
    {
      return string.Format("{0}_{1}{2}",
               foto.Created.ToString("yyyyMMdd_hhmmss"), 
               description,
               foto.File.Extension );
    }

    protected string CreateNewFileLocation( FotoInfo foto, string eventName )
    {
      string path = Path.Combine( 
              foto.Created.ToString("yyyy"), 
              foto.Created.ToString("yyyy_MM") );
      if( !string.IsNullOrEmpty( eventName ) )
        path += "-" + eventName;
      return path;
    }
     
    protected void DoIt( FotoInfoVM foto )
    {
      try
      {
        if( foto.Action == FotoInfoVM.EAction.copy ) {
          File.Copy( foto.Foto.File.FullName, 
                    Path.Combine( UserSettingsVM.Target,
                                  foto.NewFileName ) );
          foto.Action = FotoInfoVM.EAction.done;
        }
        else if( foto.Action == FotoInfoVM.EAction.move ) {
          File.Move( foto.Foto.File.FullName, 
                    Path.Combine( UserSettingsVM.Target,
                                  foto.NewFileName ) );
          foto.Action = FotoInfoVM.EAction.done;
        }
        else if( foto.Action == FotoInfoVM.EAction.trash ) {
          File.Move( foto.Foto.File.FullName, 
                    Path.Combine( UserSettingsVM.Trash,
                                  foto.NewFileName ) );
          foto.Action = FotoInfoVM.EAction.done;
        }
        else if( foto.Action == FotoInfoVM.EAction.delete ) {
          File.Delete( foto.Foto.File.FullName );
          foto.Action = FotoInfoVM.EAction.done;
        }
      }
      catch ( Exception e )
      {
        foto.Action = FotoInfoVM.EAction.failed;
        foto.Comment = e.Message;
      }

    }
  }
}