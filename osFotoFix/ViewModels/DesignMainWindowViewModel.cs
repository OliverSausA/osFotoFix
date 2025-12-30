using System.Collections.Generic;

namespace osFotoFix.ViewModels;
using osFotoFix.Models;

public class DesignMainWindowViewModel : MainWindowViewModel
{
  public DesignMainWindowViewModel()
  {
    Greeting = "Welcome to Avalonia! (Design)";
    ///// MainViewModel = new MainFotoViewModel();
  }

  protected new void CreateNavigation()
  {
    NavigationList.Clear();
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