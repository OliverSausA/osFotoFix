using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace osFotoFix.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
  
  [ObservableProperty]
  private bool isActive = false;
  partial void OnIsActiveChanged(bool value)
  {
    if (value)
      OnActivated();
    else
      OnDeactivated();
  }

  protected virtual void OnActivated() {}
  protected virtual void OnDeactivated() {}
}
