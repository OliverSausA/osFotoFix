using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace osFotoFix.ViewModels
{
  using System.Runtime.Versioning;
  using CommunityToolkit.Mvvm.ComponentModel;
  using Models;

  public partial class FotoPreviewViewModel : ViewModelBase
  {
    [ObservableProperty]
    private string filePath = "";

    [ObservableProperty]
    private double zoom = 1.0;

    [RelayCommand]
    public void ZoomIn()
    {
      Zoom *= 1.2;
    }
    [RelayCommand]
    public void ZoomOut()
    {
      Zoom /= 1.2;
      if (Zoom < 0.1) Zoom = 0.1;
    }
    public void SetZoom(double zoom)
    {
      Zoom = Math.Clamp( zoom, 0.05, 20);
    }

    [ObservableProperty]
    private double offsetX;
    [ObservableProperty]
    private double offsetY;

    [RelayCommand]
    public void FitToWindow()
    {
      Zoom = 1.0;
      OffsetX = 0;
      OffsetY = 0;
    }

    public void SetFotoInfo( FotoInfoViewModel? fotoInfo )
    {
      if (fotoInfo == null)
        FilePath = "";
      else
        FilePath = fotoInfo.Foto.File.FullName;
    }

  }
}