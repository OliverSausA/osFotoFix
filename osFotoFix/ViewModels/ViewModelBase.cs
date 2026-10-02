using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace osFotoFix.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
  private Task? initializationTask;

  [ObservableProperty]
  private ObservableCollection<MainMenuItemVM> mainMenuItems = new();
  
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

  public Task InitializeAsync()
  {
    return initializationTask ??= OnInitializedAsync();
  }

  protected virtual Task OnInitializedAsync()
  {
    return Task.CompletedTask;
  }
}
