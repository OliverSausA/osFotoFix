using System.Collections.Generic;
using System.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
/// using DynamicData.Alias;
using System.Collections.ObjectModel;

namespace osFotoFix.ViewModels;

using osFotoFix.Models;
using osFotoFix.Services;

public class TargetListViewModel : ViewModelBase
{
  private UserSettingsService settings;
  public TargetListViewModel( UserSettingsService settings )
  {
    this.settings = settings;
    Targets = new ObservableCollection<TargetVM>();
    foreach(var item in settings.GetUserSettings.Targets)
      Targets.Add(new TargetVM(item));

    AddCmd = new RelayCommand(() => {
      Targets.Add(new TargetVM(new Target()));
    });
    DelCmd = new RelayCommand(() => {
      if (SelectedTarget != null )
        Targets.Remove(SelectedTarget);
      SelectedTarget = null;

    });
    SaveCmd = new RelayCommand(() => {
      settings.GetUserSettings.Targets = new List<Target>();
      foreach( var item in Targets)
        settings.GetUserSettings.Targets.Add(item.Target);
      settings.SaveUserSettings();
    });
  }

  public ObservableCollection<TargetVM> Targets { get; set; }

  public TargetVM? SelectedTarget { get; set; }

  public ICommand AddCmd { get; set; }
  public ICommand DelCmd { get; set; }
  public ICommand SaveCmd { get; set; }
}