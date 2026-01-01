using System.Collections.Generic;

namespace osFotoFix.ViewModels;
using osFotoFix.Models;

public partial class DesignMainWindowViewModel : MainWindowViewModel
{
  public DesignMainWindowViewModel()
  {
    Greeting = "Welcome to Avalonia! (Design)";
    MainViewModel = new DesignMainFotoViewModel();
  }

  protected override void CreateNavigation()
  {
    NavigationList.Clear();
    NavigationList.Add( new NavigationItemVM() {
      Title = "osFotoFix",
      IconName = "ImageBorder",
    });
    NavigationList.Add( new NavigationItemVM() {
      Title = "Targets",
      IconName = "TargetEdit",
    });
  }
 
}