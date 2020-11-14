using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;

  public class FotoInfoListViewModel : ViewModelBase
  {
    private FotoInfoDetailViewModel FotoInfoDetailVM;
    private ImageViewModel ImageVM;
    public FotoInfoListViewModel( ImageViewModel ImageVM, FotoInfoDetailViewModel FotoInfoDetailVM )
    {
      this.ImageVM = ImageVM;
      this.ImageVM.NextImageEvent += SelectNextFoto;
      this.ImageVM.PrevImageEvent += SelectPrevFoto;
      
      this.FotoInfoDetailVM = FotoInfoDetailVM;
      FotoInfoList = new ObservableCollection<FotoInfo>();

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
    }

    public ObservableCollection<FotoInfo> FotoInfoList { get; set; }

    private FotoInfo fotoSelected;
    public FotoInfo FotoSelected { 
      get { return fotoSelected; } 
      set { 
        this.RaiseAndSetIfChanged( ref fotoSelected, value );
        fotoSelected = value;
        ImageVM.Foto = value;
        FotoInfoDetailVM.Foto = value;
      } 
    }

    public void SelectNextFoto()
    {
      if( fotoSelected == null ) return;
      int idx = fotoSelected.Index +1;
      if( idx < FotoInfoList.Count )
        FotoSelected = FotoInfoList[ idx ];
    }
    public void SelectPrevFoto()
    {
      if( fotoSelected == null ) return;
      int idx = fotoSelected.Index -1;
      if( idx >= 0 )
        FotoSelected = FotoInfoList[ idx ];
    }
  }
}