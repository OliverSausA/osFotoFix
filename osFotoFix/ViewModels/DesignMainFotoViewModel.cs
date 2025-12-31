namespace osFotoFix.ViewModels;

public class DesignMainFotoViewModel : MainFotoViewModel
{
  public DesignMainFotoViewModel()
  {
    MainMenuItems.Add(new MainMenuItemVM() { Title="Save", IconName="Save" } );
  }
}