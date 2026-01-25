using System;
using System.IO;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;



namespace osFotoFix.ViewModels
{
  using System.Threading;
  using System.Threading.Tasks;
  using Models;

  public partial class FotoInfoViewModel : ViewModelBase
  {
    public FotoInfoViewModel( FotoInfo foto, int previewSize )
    {
      Foto = foto;
      CreateNewFileName();
      PreviewSize = previewSize;
    }

    public FotoInfo Foto {get;set;}

    [ObservableProperty]
    private Target? target = null;
    partial void OnTargetChanged(Target? value)
    {
      if( value != null )
      {
        Title = value.Title;
        Description = value.Description;
        TargetPath = value.Path;
        Action = value.Action;
      }
    }

    public int Index { get; set; }

    public string Comment {
      get { return Foto.Comment; }
      set { Foto.Comment = value;
            CreateNewFileName();
            OnPropertyChanged(); }
    }

    public string TargetPath {
      get {return Foto.TargetPath; }
      set { Foto.TargetPath = value;
            OnPropertyChanged(); }
    }

    public string Title {
      get { return Foto.Title; }
      set { Foto.Title = value;
            CreateNewFileName();
            OnPropertyChanged(); }
    }

    public string Description {
      get { return Foto.Description; }
      set { Foto.Description = value;
            CreateNewFileName();
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

    private void CreateNewFileName()
    {
      string subpath = string.Format("{0}",
        Foto.Created.ToString("yyyy_MM"));
      if (!string.IsNullOrEmpty(Foto.Title))
        subpath += "-" + Foto.Title;

      string filename = string.Format("{0}",
        Foto.Created.ToString("yyyyMMdd_HHmmss"));
      if (!string.IsNullOrEmpty(Foto.Description))
        filename += "_" + Foto.Description;
      if (!string.IsNullOrEmpty(Foto.Comment))
        filename += "_" + Foto.Comment;
      filename += Foto.File.Extension;

      NewFileName = Path.Combine( 
        Foto.Created.ToString("yyyy"), 
        subpath,
        filename );
    }

    [ObservableProperty]
    private Bitmap? thumpnail;

    [ObservableProperty]
    private int previewSize;

    partial void OnPreviewSizeChanged( int value )
    {
      thumbnailCts?.Cancel();
      thumbnailCts = new CancellationTokenSource();
      _ = CreateThumbnailAsync( value, thumbnailCts.Token );
    }
    private static readonly SemaphoreSlim thumbnailSemaphore = 
        new SemaphoreSlim(Environment.ProcessorCount);
    private CancellationTokenSource? thumbnailCts;

    private async Task CreateThumbnailAsync(int size, CancellationToken token)
    {
      await thumbnailSemaphore.WaitAsync(token);
      try
      {
        var bitmap = await Task.Run(() =>
        {
          token.ThrowIfCancellationRequested();
          using var image = Image.Load( Foto.File.FullName );
          image.Mutate( x => x.Resize(size, 0));

          using var memstream = new MemoryStream();
          image.Save(memstream, new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder());
          memstream.Position = 0;
          return new Avalonia.Media.Imaging.Bitmap(memstream);
        }, token);

        await Dispatcher.UIThread.InvokeAsync( () => {
          Thumpnail = bitmap;
        } );
      }
      catch (OperationCanceledException)
      {
        // Ignore
      }
      finally
      {
        thumbnailSemaphore.Release();
      }
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