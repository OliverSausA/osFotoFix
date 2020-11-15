using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace osFotoFix.ViewModels
{
  using Models;
    public class MainWindowViewModel : ViewModelBase
    {
      public MainWindowViewModel()
      {
        SettingsVM = new SettingsViewModel();
        ImageVM = new ImageViewModel();
        FotoInfoDetailVM = new FotoInfoDetailViewModel();
        FotoInfoListVM = new FotoInfoListViewModel( ImageVM, FotoInfoDetailVM );
        FotoInfoListVM.Update( SettingsVM.Settings );
      }


      public SettingsViewModel SettingsVM {get;set;}

      public FotoInfoListViewModel FotoInfoListVM {get;set;}

      public FotoInfoDetailViewModel FotoInfoDetailVM {get;set;}

      public ImageViewModel ImageVM {get;set;}

      public string Greeting => "Hello World!";
    }
}
