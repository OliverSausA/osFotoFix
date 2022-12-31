using System;
using System.IO;
using Avalonia.Media.Imaging;
using ReactiveUI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace osFotoFix.ViewModels
{
  using Models;

  public class FotoInfoVM : ViewModelBase
  {
    public FotoInfoVM( FotoInfo foto )
    {
      Foto = foto;
    }

    public FotoInfo Foto {get;set;}

    public int Index { get; set; }

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

    private Bitmap thumpnail;
    public Bitmap Thumpnail {
      get {
        if( thumpnail == null )
        {
          using (var image = Image.Load( Foto.File.FullName ))
          {
            int w = 300;
            // int h = w * (image.Width / image.Height);
            image.Mutate( x => x.Resize(w, 0));
            using( var memstream = new MemoryStream() )
            {
              image.Save(memstream, new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder());
              memstream.Position = 0;
              thumpnail = new Avalonia.Media.Imaging.Bitmap(memstream);
            }
          }
        }
        return thumpnail;
      }
    }
    public bool ThumpnailCallback() {
      return false;
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