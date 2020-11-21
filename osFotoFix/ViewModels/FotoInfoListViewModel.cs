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

    //public string EventName {get;set;}
    //public string Description {get;set;}

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
      foto.NewFileLocation = CreateNewFileLocation( foto.Foto, UserSettingsVM.Title );
      foto.NewFileName = CreateNewFileName( foto.Foto, UserSettingsVM.Description );
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
      foto.NewFileLocation = CreateNewFileLocation( foto.Foto, UserSettingsVM.Title );
      foto.NewFileName = CreateNewFileName( foto.Foto, UserSettingsVM.Description );
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
      foto.NewFileLocation = CreateNewFileLocation( foto.Foto, UserSettingsVM.Title );
      foto.NewFileName = CreateNewFileName( foto.Foto, UserSettingsVM.Description );
    }

    protected string CreateNewFileName( FotoInfo foto, string description )
    {
      return string.Format("{0}_{1}{2}",
               foto.Created.ToString("yyyyMMdd_hhmmss"), 
               description,
               foto.File.Extension );
    }

    protected string CreateNewFileLocation( FotoInfo foto, string title )
    {
      string path = Path.Combine( 
              foto.Created.ToString("yyyy"), 
              foto.Created.ToString("yyyy_MM") );
      if( !string.IsNullOrEmpty( title ) )
        path += "-" + title;
      return path;
    }
     
    protected void DoIt( FotoInfoVM foto )
    {
      try
      {
        if( (foto.Foto.TypeOfCreationDate == FotoInfo.ETypeOfCreationDate.Filesystem) )
        {
          foto.Comment = "Exif Infomation ist ungültig!";
          foto.Action = FotoInfoVM.EAction.failed;
        }
        else if( foto.Action == FotoInfoVM.EAction.copy ) {
          var f = Copy( foto.Foto.File.FullName,
                        UserSettingsVM.Target,
                        foto.NewFileLocation,
                        foto.NewFileName );
          AdjustTimeStamp( f, foto.Foto.Created );
          foto.Action = FotoInfoVM.EAction.done;
          foto.NewFileName = f;
        }
        else if( foto.Action == FotoInfoVM.EAction.move ) {
          var f = Move( foto.Foto.File.FullName,
                        UserSettingsVM.Target,
                        foto.NewFileLocation,
                        foto.NewFileName );
          AdjustTimeStamp( f, foto.Foto.Created );
          foto.Action = FotoInfoVM.EAction.done;
          foto.NewFileName = f;
        }
        else if( foto.Action == FotoInfoVM.EAction.trash ) {
          var f = Move( foto.Foto.File.FullName,
                        UserSettingsVM.Trash,
                        foto.NewFileLocation,
                        foto.NewFileName );
          AdjustTimeStamp( f, foto.Foto.Created );
          foto.Action = FotoInfoVM.EAction.done;
          foto.NewFileName = f;
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

    private string Copy( string source, string target, string targetDir, string targetFile )
    {
      target = Prepare( source, target, targetDir, targetFile );
      File.Copy( source, target );
      return target;
    }
    private string Move( string source, string target, string targetDir, string targetFile )
    {
      target = Prepare( source, target, targetDir, targetFile );
      File.Move( source, target );
      return target;
    }
    private string Prepare( string source, string target, string targetDir, string targetFile )
    {
      target = Path.Combine( target, targetDir );
      var dir = new DirectoryInfo( target );
      dir.Create();
      target = Path.Combine( target, targetFile );
      var file = new FileInfo( target );
      int idx = 0;
      while( file.Exists )
      {
        var next = target.Replace( file.Extension, string.Format("_{0}{1}", ++idx, file.Extension ) );
        file = new FileInfo( next );
      }
      return file.FullName;
    }
    private void AdjustTimeStamp( string file, DateTime timestamp )
    {
      var f = new FileInfo( file );
      if( f.Exists && f.CreationTime != timestamp )
        File.SetCreationTime( file, timestamp );
    }
  }
}