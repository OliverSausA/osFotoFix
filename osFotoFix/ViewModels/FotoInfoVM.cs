using System;
using System.IO;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;

  public class FotoInfoVM : ViewModelBase
  {
    public FotoInfoVM( FotoInfo foto, int index )
    {
      Foto = foto;
      Index = index;
    }

    public FotoInfo Foto {get;set;}

    public int Index { get; private set; }

    public string Comment {
      get { return Foto.Comment; }
      set { Foto.Comment = value;
            this.RaisePropertyChanged(); }
    }

    public string Target {
      get {return Foto.Target; }
      set { Foto.Target = value;
            this.RaisePropertyChanged(); }
    }

    public string Title {
      get { return Foto.Title; }
      set { Foto.Title = value;
            this.RaisePropertyChanged(); }
    }

    public string Description {
      get { return Foto.Description; }
      set { Foto.Description = value;
            this.RaisePropertyChanged(); }
    }

    public string NewFileName {
      get { return Foto.NewFileName; }
      set { Foto.NewFileName = value;
            this.RaisePropertyChanged(); }
    }

    public bool FileExistsOnTarget {
      get { return Foto.FileExistsOnTarget; }
      set { Foto.FileExistsOnTarget = value;
            this.RaisePropertyChanged(); }
    }

    public EAction Action {
      get { return Foto.Action; }
      set { Foto.Action = value;
            this.RaisePropertyChanged(); }
    }

    public void UpdateView()
    {
      this.RaisePropertyChanged(nameof(Comment));
      this.RaisePropertyChanged(nameof(Target));
      this.RaisePropertyChanged(nameof(Title));
      this.RaisePropertyChanged(nameof(Description));
      this.RaisePropertyChanged(nameof(NewFileName));
      this.RaisePropertyChanged(nameof(FileExistsOnTarget));
      this.RaisePropertyChanged(nameof(Action));
    }
  }
}