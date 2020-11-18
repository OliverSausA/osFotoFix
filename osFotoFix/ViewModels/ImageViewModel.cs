using System;
using System.Reactive;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;
  public class ImageViewModel : ViewModelBase
  {

    public ImageViewModel( SettingsViewModel UserSettingsVM ) {
      this.UserSettingsVM = UserSettingsVM;

      UndoImageCmd = ReactiveCommand.Create( OnUndoImage );
      NextImageCmd = ReactiveCommand.Create( OnNextImage );
      PrevImageCmd = ReactiveCommand.Create( OnPrevImage );
      DelImageCmd = ReactiveCommand.Create( OnDelImage );
      CopyImageCmd = ReactiveCommand.Create( OnCopyImage );
      TrashImageCmd = ReactiveCommand.Create( OnTrashImage );
      MoveImageCmd = ReactiveCommand.Create( OnMoveImage );
      CopyImageCmd = ReactiveCommand.Create( OnCopyImage );
    }

    public SettingsViewModel UserSettingsVM { get;set; }

    private FotoInfoVM foto;
    public FotoInfoVM Foto {
      get => foto;
      set => this.RaiseAndSetIfChanged( ref foto, value );
    }

    public ReactiveCommand<Unit, Unit> UndoImageCmd { get; }
    public void OnUndoImage() {
      if( UndoImageEvent != null )
        UndoImageEvent();
    }
    public delegate void UndoImage();
    public UndoImage UndoImageEvent;

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
    public PrevImage PrevImageEvent;

    public ReactiveCommand<Unit, Unit> TrashImageCmd { get; }
    public void OnTrashImage() {
      if( TrashImageEvent != null )
        TrashImageEvent();
    }
    public delegate void TrashImage();
    public TrashImage TrashImageEvent;

    public ReactiveCommand<Unit, Unit> DelImageCmd { get; }
    public void OnDelImage() {
      if( DelImageEvent != null )
        DelImageEvent();
    }
    public delegate void DelImage();
    public DelImage DelImageEvent;

    public ReactiveCommand<Unit, Unit> MoveImageCmd { get; }
    public void OnMoveImage() {
      if( MoveImageEvent != null )
        MoveImageEvent();
    }
    public delegate void MoveImage();
    public MoveImage MoveImageEvent;

    public ReactiveCommand<Unit, Unit> CopyImageCmd { get; }
    public void OnCopyImage() {
      if( CopyImageEvent != null )
        CopyImageEvent();
    }
    public delegate void CopyImage();
    public CopyImage CopyImageEvent;

  }
}