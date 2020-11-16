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
      delete
    };

    public FotoInfoVM( FotoInfo foto, int index )
    {
      Action = EAction.ignore;
      Foto = foto;
      Index = index;
    }

    public FotoInfo Foto {get;set;}

    public int Index {get;set;}

    private string newFileLocation;
    public string NewFileLocation {
      get {return newFileLocation; }
      set { this.RaiseAndSetIfChanged( ref newFileLocation, value ); }
    }

    private string newFileName;
    public string NewFileName {
      get { return newFileName; }
      set { this.RaiseAndSetIfChanged( ref newFileName, value ); }
    }

    private EAction action;
    public EAction Action {
      get { return action; }
      set { this.RaiseAndSetIfChanged( ref action, value ); }
    }
  }
}