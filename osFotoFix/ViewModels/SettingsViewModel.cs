using System;

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
  }
}