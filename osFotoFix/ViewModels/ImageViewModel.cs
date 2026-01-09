using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace osFotoFix.ViewModels
{
  using Models;
  public partial class ImageViewModel : ViewModelBase
  {

    public ImageViewModel( SettingsViewModel UserSettingsVM ) {
      this.UserSettingsVM = UserSettingsVM;

      UndoImageCmd = new RelayCommand( OnUndoImage );
      NextImageCmd = new RelayCommand( OnNextImage );
      PrevImageCmd = new RelayCommand( OnPrevImage );
      DelImageCmd = new RelayCommand( OnDelImage );
      CopyImageCmd = new RelayCommand( OnCopyImage );
      TrashImageCmd = new RelayCommand( OnTrashImage );
      MoveImageCmd = new RelayCommand( OnMoveImage );
      CopyImageCmd = new RelayCommand( OnCopyImage );
    }

    public SettingsViewModel UserSettingsVM { get;set; }

    [ObservableProperty]
    private FotoInfoViewModel? foto;

    public ICommand UndoImageCmd { get; }
    public void OnUndoImage() {
      if( UndoImageEvent != null )
        UndoImageEvent();
    }
    public delegate void UndoImage();
    public UndoImage? UndoImageEvent;

    public ICommand NextImageCmd { get; }
    public void OnNextImage() {
      if( NextImageEvent != null )
        NextImageEvent();
    }
    public delegate void NextImage();
    public NextImage? NextImageEvent;

    public ICommand PrevImageCmd { get; }
    public void OnPrevImage() {
      if( PrevImageEvent != null )
        PrevImageEvent();
    }
    public delegate void PrevImage();
    public PrevImage? PrevImageEvent;

    public ICommand TrashImageCmd { get; }
    public void OnTrashImage() {
      if( TrashImageEvent != null )
        TrashImageEvent();
    }
    public delegate void TrashImage();
    public TrashImage? TrashImageEvent;

    public ICommand DelImageCmd { get; }
    public void OnDelImage() {
      if( DelImageEvent != null )
        DelImageEvent();
    }
    public delegate void DelImage();
    public DelImage? DelImageEvent;

    public ICommand MoveImageCmd { get; }
    public void OnMoveImage() {
      if( MoveImageEvent != null )
        MoveImageEvent();
    }
    public delegate void MoveImage();
    public MoveImage? MoveImageEvent;

    public ICommand CopyImageCmd { get; }
    public void OnCopyImage() {
      if( CopyImageEvent != null )
        CopyImageEvent();
    }
    public delegate void CopyImage();
    public CopyImage? CopyImageEvent;

  }
}