using System;
using System.IO;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;

  public class FotoInfoVM : ViewModelBase
  {
    public enum EAction {
      ignore = 0,
      copy,
      move,
      delete,
      trash,
      done,
      failed
    };

    public FotoInfoVM( FotoInfo foto, int index )
    {
      Action = EAction.ignore;
      Foto = foto;
      Index = index;
    }

    public FotoInfo Foto {get;set;}

    public int Index {get;set;}
    private string comment;
    public string Comment {
      get { return comment; }
      set { this.RaiseAndSetIfChanged( ref comment, value ); }
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

    private EAction action;
    public EAction Action {
      get { return action; }
      set { this.RaiseAndSetIfChanged( ref action, value ); }
    }

    public void UpdateView()
    {
      this.RaisePropertyChanged(nameof(FileExistsOnTarget));
      this.RaisePropertyChanged(nameof(Comment));
    }
  }
}