using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace osFotoFix.Models;

public partial class NavigationItemVM : ObservableObject
{
  [ObservableProperty]
  private string title = string.Empty;

  [ObservableProperty]
  private string iconName = string.Empty;

  [ObservableProperty]
  private UserControl? view;

  [ObservableProperty]
  private ICommand? command;
}