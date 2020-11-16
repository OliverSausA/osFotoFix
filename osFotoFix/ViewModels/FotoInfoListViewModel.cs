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
    private UserSettings userSettings;
    private FotoInfoService service;
    private FotoInfoDetailViewModel FotoInfoDetailVM;
    private ImageViewModel ImageVM;
    public FotoInfoListViewModel( ImageViewModel ImageVM, FotoInfoDetailViewModel FotoInfoDetailVM )
    {
      service = new FotoInfoService();
      this.ImageVM = ImageVM;
      this.ImageVM.NextImageEvent += SelectNextFoto;
      this.ImageVM.PrevImageEvent += SelectPrevFoto;
      this.ImageVM.DelImageEvent += DelFoto;
      this.ImageVM.CopyImageEvent += CopyFoto;
      this.ImageVM.MoveImageEvent += MoveFoto;
      
      this.FotoInfoDetailVM = FotoInfoDetailVM;
      FotoInfoList = new ObservableCollection<FotoInfoVM>();

      /*
      var fotoInfo = new FotoInfo( new FileInfo( "TEST.jpg"), DateTime.Now, true );
      fotoInfo.FileLocationOld = "FileLocaltionOld";
      fotoInfo.FileNameOld = "FileNameOle";
      fotoInfo.FileLocationNew = "FileLocaltionNew";
      fotoInfo.FileNameNew = "FileNameNew";
      FotoInfoList.Add( fotoInfo );
      fotoInfo.Index = FotoInfoList.Count -1;

      fotoInfo = new FotoInfo( new FileInfo( "TEST.jpg"), DateTime.Now, true );
      fotoInfo.FileLocationOld = "FileLocaltionOld 2";
      fotoInfo.FileNameOld = "FileNameOle 2";
      fotoInfo.FileLocationNew = "FileLocaltionNew 2";
      fotoInfo.FileNameNew = "FileNameNew 2";
      FotoInfoList.Add( fotoInfo );
      fotoInfo.Index = FotoInfoList.Count -1;
      */
    }

    public ObservableCollection<FotoInfoVM> FotoInfoList { get; set; }

    

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

    public void Update( UserSettings userSettings )
    {
      this.userSettings = userSettings;
      FotoInfoList = new ObservableCollection<FotoInfoVM>();
      var baseDir = new DirectoryInfo( userSettings.Quelle );
      var fotos = service.GetFotoInfos( baseDir );

      int index = 0;
      foreach( var foto in fotos )
      {
        FotoInfoList.Add( new FotoInfoVM( foto, index++ ) );
      }
    }

    protected void SelectNextFoto()
    {
      if( fotoSelected == null ) return;
      int idx = fotoSelected.Index +1;
      if( idx < FotoInfoList.Count )
        FotoSelected = FotoInfoList[ idx ];
    }
    protected void SelectPrevFoto()
    {
      if( fotoSelected == null ) return;
      int idx = fotoSelected.Index -1;
      if( idx >= 0 )
        FotoSelected = FotoInfoList[ idx ];
    }
    protected void DelFoto()
    {
      if( fotoSelected == null ) return;
      fotoSelected.Action = FotoInfoVM.EAction.delete;
      fotoSelected.NewFileLocation = "";
      fotoSelected.NewFileName = "";
      SelectNextFoto();
    }
    protected void CopyFoto()
    {
      if( fotoSelected == null ) return;
      fotoSelected.Action = FotoInfoVM.EAction.copy;
      fotoSelected.NewFileLocation = CreateNewFileLocation( fotoSelected.Foto, ImageVM.EventName );
      fotoSelected.NewFileName = CreateNewFileName( fotoSelected.Foto, ImageVM.Description );
      SelectNextFoto();
    }
    protected void MoveFoto()
    {
      if( fotoSelected == null ) return;
      fotoSelected.Action = FotoInfoVM.EAction.move;
      fotoSelected.NewFileLocation = CreateNewFileLocation( fotoSelected.Foto, ImageVM.EventName );
      fotoSelected.NewFileName = CreateNewFileName( fotoSelected.Foto, ImageVM.Description );
      SelectNextFoto();
    }

    protected string CreateNewFileName( FotoInfo foto, string description )
    {
      return string.Format("{0}_{1}.{2}",
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
  }
}