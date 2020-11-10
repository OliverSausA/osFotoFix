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
      FotoInfoList.Add(
          new FotoInfo( new FileInfo( "TEST.jpg"), DateTime.Now, true ) );
      FotoInfoList.Add(
          new FotoInfo( new FileInfo( "TEST2.jpg"), DateTime.Now, false ) );
    }

    public ObservableCollection<FotoInfo> FotoInfoList {get; set;}
  }
}