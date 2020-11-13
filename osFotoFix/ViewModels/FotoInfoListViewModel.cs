using System;
using System.IO;
using System.Collections.ObjectModel;

namespace osFotoFix.ViewModels
{
  using Models;

  public class FotoInfoListViewModel : ViewModelBase
  {
    public FotoInfoListViewModel()
    {
      FotoInfoList = new ObservableCollection<FotoInfo>();

      var fotoInfo = new FotoInfo( new FileInfo( "TEST.jpg"), DateTime.Now, true );
      fotoInfo.FileLocationOld = "FileLocaltionOld";
      fotoInfo.FileNameOld = "FileNameOle";
      fotoInfo.FileLocationNew = "FileLocaltionNew";
      fotoInfo.FileNameNew = "FileNameNew";
      FotoInfoList.Add( fotoInfo );

      fotoInfo = new FotoInfo( new FileInfo( "TEST.jpg"), DateTime.Now, true );
      fotoInfo.FileLocationOld = "FileLocaltionOld 2";
      fotoInfo.FileNameOld = "FileNameOle 2";
      fotoInfo.FileLocationNew = "FileLocaltionNew 2";
      fotoInfo.FileNameNew = "FileNameNew 2";
      FotoInfoList.Add( fotoInfo );
    }

    public ObservableCollection<FotoInfo> FotoInfoList { get; set; }

    private FotoInfo fotoSelected;
    public FotoInfo FotoSelected { 
      get { return fotoSelected; } 
      set { fotoSelected = value; } 
    }
  }
}