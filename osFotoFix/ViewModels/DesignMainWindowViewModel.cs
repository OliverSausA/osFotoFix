using System.Collections.Generic;

namespace osFotoFix.ViewModels;

public class DesignMainWindowViewModel : MainWindowViewModel
{
  protected new void Configure()
  {

  }

  protected new void CreateNavigation()
  {
    NavigationList = new List<NavigationItemVM>();
    NavigationList.Add( new NavigationItemVM() {
      Title = "FotoFix",
      IconName = "FluentIcons.image_library_regular",
    });
    NavigationList.Add( new NavigationItemVM() {
      Title = "Targets",
      IconName = "FluentIcons.target_edit_regular",
    });
  }
}