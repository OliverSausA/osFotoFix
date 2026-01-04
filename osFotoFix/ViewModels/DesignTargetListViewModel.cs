using System.Collections.Generic;
using osFotoFix.Models;
using osFotoFix.Services;

namespace osFotoFix.ViewModels;

public class DesignUserSettingsService : UserSettingsService
{
  protected new void ReadUserSettings()
  {
    userSettings = new UserSettings();
    userSettings.Targets = new List<Target>();
    userSettings.Targets.Add(
      new Target() { Title = "Target Title 1", Path = "target/path", Enabled = true, Action = EAction.copy });
    userSettings.Targets.Add(
      new Target() { Title = "another Title 1", Path = "target/other/path", Enabled = false, Action = EAction.move });
  }

}
public class DesignTargetListViewModel : TargetListViewModel
{
  public DesignTargetListViewModel() : base(new DesignUserSettingsService() )
  {
    Targets.Add(
      new TargetViewModel(new Target() { Title = "Target Title 2", Path = "target/path", Enabled = true, Action = EAction.copy }));
    Targets.Add(
      new TargetViewModel(new Target() { Title = "another Title 2", Path = "target/other/path", Enabled = false, Action = EAction.move }));
  }
}