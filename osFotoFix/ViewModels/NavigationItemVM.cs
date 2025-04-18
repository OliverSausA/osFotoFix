using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;

public class NavigationItemVM : ReactiveObject
{
  public string Title { get; set; }
  public string IconName { get; set; }
  public UserControl View { get; set; }
  public ICommand Command { get; set; }
}