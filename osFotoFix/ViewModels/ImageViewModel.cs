using System;
using System.Reactive;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;
  public class ImageViewModel : ViewModelBase
  {

    public ImageViewModel() {
      NextImageCmd = ReactiveCommand.Create( OnNextImage );
      PrevImageCmd = ReactiveCommand.Create( OnPrevImage );
    }

    private FotoInfo foto;
    public FotoInfo Foto {
      get => foto;
      set => this.RaiseAndSetIfChanged( ref foto, value );
    }

    public ReactiveCommand<Unit, Unit> NextImageCmd { get; }

    public void OnNextImage() {
      if( NextImageEvent != null )
        NextImageEvent();
    }
    public delegate void NextImage();
    public NextImage NextImageEvent;

    public ReactiveCommand<Unit, Unit> PrevImageCmd { get; }
    public void OnPrevImage() {
      if( PrevImageEvent != null )
        PrevImageEvent();
    }
    public delegate void PrevImage();
    public NextImage PrevImageEvent;

  }
}