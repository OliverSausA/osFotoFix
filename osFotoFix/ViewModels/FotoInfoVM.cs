using System;
using System.IO;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;

  public class FotoInfoVM : ViewModelBase
  {
    public FotoInfoVM( FotoInfo foto )
    {
      Action = EAction.ignore;
      Foto = foto;
    }

    public FotoInfo Foto {get;set;}

    public int Index {
      get { return Foto.Index; }
    }

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
      this.RaisePropertyChanged(nameof(FileExistsOnTarget));
      this.RaisePropertyChanged(nameof(Comment));
    }
  }
}