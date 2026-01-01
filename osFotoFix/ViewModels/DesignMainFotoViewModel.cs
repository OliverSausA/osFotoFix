namespace osFotoFix.ViewModels;

public class DesignMainFotoViewModel : MainFotoViewModel
{
  public DesignMainFotoViewModel()
  {
    MainMenuItems.Add(new MainMenuItemVM() { Title="Save", IconName="Save" } );
    MainMenuItems.Add(new MainMenuItemVM() { Title="Trash", IconName="Delete" } );
  }
}