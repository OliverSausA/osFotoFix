using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace osFotoFix.ViewModels
{
  using CommunityToolkit.Mvvm.ComponentModel;
  using Models;

  public partial class xFotoPreviewViewModel : ViewModelBase
  {
    [ObservableProperty]
    private FotoInfoViewModel? foto = null;
  }
}