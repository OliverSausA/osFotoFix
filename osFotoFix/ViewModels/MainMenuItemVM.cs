using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace osFotoFix.ViewModels;

public partial class MainMenuItemVM : ObservableObject
{
  [ObservableProperty]
  private string title = string.Empty;

  [ObservableProperty]
  private string iconName = string.Empty;

  [ObservableProperty]
  private ICommand? command;
}
