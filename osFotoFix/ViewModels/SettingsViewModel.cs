using System;

namespace osFotoFix.ViewModels
{
  using Models;
  public class SettingsViewModel : ViewModelBase
  {
    public SettingsViewModel() : base()
    {
        Settings = new Settings() {
          Quelle = "/home/oliver/bilder/import",
          Ziel = "/home/oliver/bilder/FotoSammlung",
          Papierkorb = "/home/oliver/bilder/papierkorb"
        };
    }

    public Settings Settings {get; set;}
  }
}