using System.Collections.Generic;
using System.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace osFotoFix.ViewModels;

using osFotoFix.Models;
using osFotoFix.Services;

public class TargetListViewModel : ViewModelBase
{
  private UserSettingsService userSettingsService;
  public TargetListViewModel( UserSettingsService userSettingsService )
  {
    this.userSettingsService = userSettingsService;

    MainMenuItems.Add(new MainMenuItemVM() { 
      Title = "Save", 
      IconName = "Save",
      Command = new RelayCommand( OnSaveCmd ) 
    } );

    MainMenuItems.Add(new MainMenuItemVM() { 
      Title = "Add", 
      IconName = "AddSquare",
      Command = new RelayCommand( () => { 
        AddTarget( new Target()  ); 
      } )
    } );

    Targets = new ObservableCollection<TargetVM>();
    var settings = userSettingsService.GetUserSettings;
    foreach(var item in settings.Targets)
      AddTarget(item);
  }

  public ObservableCollection<TargetVM> Targets { get; set; }

  public TargetVM? SelectedTarget { get; set; }

  private void AddTarget( Target target )
  {
    var targetVM = new TargetVM(target);
    Targets.Add(targetVM);
    targetVM.RemoveThisItemEvent += (target) => {
      System.Diagnostics.Debug.WriteLine("Remove TargetVM from TargetListViewModel");
      if( SelectedTarget == target )
        SelectedTarget = null;    

      if (target != null)
        Targets.Remove(target);
    };
  }

  private void OnSaveCmd()
  {
    var settings = userSettingsService.GetUserSettings;
    settings.Targets = new List<Target>();
    foreach (var item in Targets)
      settings.Targets.Add(item.Target);
    userSettingsService.SaveUserSettings();
  }
}