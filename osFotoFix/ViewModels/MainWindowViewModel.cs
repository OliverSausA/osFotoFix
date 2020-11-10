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
        FotoInfoListVM = new FotoInfoListViewModel();
      }


      public SettingsViewModel SettingsVM {get;set;}

      public FotoInfoListViewModel FotoInfoListVM {get;set;}

      public string Greeting => "Hello World!";
    }
}
