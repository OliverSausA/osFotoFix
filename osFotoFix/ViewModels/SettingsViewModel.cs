using System;
using System.Reactive;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;
  using Services;

  public class SettingsViewModel : ViewModelBase
  {
    private UserSettingsService service;
    public SettingsViewModel() : base()
    {
      service = new UserSettingsService();
      Settings = service.GetUserSettings();
      service.SaveUserSettings( Settings );
    }

    public UserSettings Settings {get; set;}

    public bool TrashCmdActive {
      get { return Settings.TrashCmdActive; }
      set { 
        Settings.TrashCmdActive = value;
        this.RaisePropertyChanged();
      }
    }

    public bool DelCmdActive {
      get { return Settings.DelCmdActive; }
      set { 
        Settings.DelCmdActive = value;
        this.RaisePropertyChanged();
      }
    }

    public bool MoveCmdActive {
      get { return Settings.MoveCmdActive; }
      set { 
        Settings.MoveCmdActive = value;
        this.RaisePropertyChanged();
      }
    }

    public bool CopyCmdActive {
      get { return Settings.CopyCmdActive; }
      set { 
        Settings.CopyCmdActive = value;
        this.RaisePropertyChanged();
      }
    }

  }
}