using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;

  public class FotoPreviewViewModel : ViewModelBase
  {
    private ObservableCollection<FotoInfo> fotoList = new ObservableCollection<FotoInfo>();
    public ObservableCollection<FotoInfo> FotoList {
      get { return fotoList; }
      set { this.RaiseAndSetIfChanged(ref fotoList, value); }
    }
  }
}