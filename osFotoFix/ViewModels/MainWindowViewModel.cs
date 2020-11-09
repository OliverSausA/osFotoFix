using System;
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


      }


      public SettingsViewModel SettingsVM {get;set;}
      public string Greeting => "Hello World!";
    }
}
