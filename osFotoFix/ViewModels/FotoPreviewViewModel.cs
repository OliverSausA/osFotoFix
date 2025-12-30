using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace osFotoFix.ViewModels
{
  using CommunityToolkit.Mvvm.ComponentModel;
  using Models;

  public partial class FotoPreviewViewModel : ViewModelBase
  {
    [ObservableProperty]
    private ObservableCollection<FotoInfoVM> fotoList = new();
  }
}