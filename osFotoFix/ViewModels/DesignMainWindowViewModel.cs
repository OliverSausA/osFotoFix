using System.Collections.Generic;

namespace osFotoFix.ViewModels;
using osFotoFix.Models;

public class DesignMainWindowViewModel : MainWindowViewModel
{
  public DesignMainWindowViewModel()
  {
    Greeting = "Welcome to Avalonia! (Design)";
    MainViewModel = new DesignMainFotoViewModel();
  }

  protected new void CreateNavigation()
  {
    NavigationList.Clear();
    NavigationList.Add( new NavigationItemVM() {
      Title = "FotoFix",
      IconName = "ImageBorder",
    });
    NavigationList.Add( new NavigationItemVM() {
      Title = "Targets",
      IconName = "TargetEdit",
    });
  }
 
}