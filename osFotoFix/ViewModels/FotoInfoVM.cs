using System;
using System.IO;
using Avalonia.Media.Imaging;
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
            OnPropertyChanged(); }
    }

    public string Target {
      get {return Foto.Target; }
      set { Foto.Target = value;
            OnPropertyChanged(); }
    }

    public string Title {
      get { return Foto.Title; }
      set { Foto.Title = value;
            OnPropertyChanged(); }
    }

    public string Description {
      get { return Foto.Description; }
      set { Foto.Description = value;
            OnPropertyChanged(); }
    }

    public string NewFileName {
      get { return Foto.NewFileName; }
      set { Foto.NewFileName = value;
            OnPropertyChanged(); }
    }

    public bool FileExistsOnTarget {
      get { return Foto.FileExistsOnTarget; }
      set { Foto.FileExistsOnTarget = value;
            OnPropertyChanged(); }
    }

    public EAction Action {
      get { return Foto.Action; }
      set { Foto.Action = value;
            OnPropertyChanged(); }
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
      OnPropertyChanged(nameof(Comment));
      OnPropertyChanged(nameof(Target));
      OnPropertyChanged(nameof(Title));
      OnPropertyChanged(nameof(Description));
      OnPropertyChanged(nameof(NewFileName));
      OnPropertyChanged(nameof(FileExistsOnTarget));
      OnPropertyChanged(nameof(Action));
    }
  }
}