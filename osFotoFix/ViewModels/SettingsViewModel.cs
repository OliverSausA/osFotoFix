using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive;

using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  using Models;
  using Services;

  public class SettingsViewModel : ViewModelBase
  {
    private UserSettingsService service;
    public SettingsViewModel() : base()
    {
      Read();
      SelectSourceCmd = ReactiveCommand.Create( OnSelectSource );
      SelectTargetCmd = ReactiveCommand.Create( OnSelectTarget );
      SelectTrashCmd  = ReactiveCommand.Create( OnSelectTrash );
    }

    public void SaveSettings()
    {
      Write();
    }

    ///// public UserSettings Settings {get; set;}

    private string source;
    public string Source { 
      get { return source; }
      set { 
        this.RaiseAndSetIfChanged( ref source, value ); 
        NewSourceSelectedEvent?.Invoke( source );
      } 
    }
    public delegate void NewSourceSelected( string source );
    public NewSourceSelected NewSourceSelectedEvent;

    public ReactiveCommand<Unit, Unit> SelectSourceCmd { get; }
    public async void OnSelectSource() {
      Source = await GetFolderName( source );
    }

    private string target;
    public string Target { 
      get { return target; }
      set { this.RaiseAndSetIfChanged( ref target, value ); } 
    }
    public ReactiveCommand<Unit, Unit> SelectTargetCmd { get; }
    public async void OnSelectTarget() {
      Target = await GetFolderName( target );
    }

    private string trash;
    public string Trash { 
      get { return trash; }
      set { this.RaiseAndSetIfChanged( ref trash, value ); } 
    }
    public ReactiveCommand<Unit, Unit> SelectTrashCmd { get; }
    public async void OnSelectTrash() {
      Trash = await GetFolderName( trash );
    }

    
    public async Task<string> GetFolderName(string current)
    {
      try
      {
        if( string.IsNullOrEmpty(current) ) 
          current = service.getUserPicturePath();
        var dlg = new OpenFolderDialog() {
          Title = "Wo sollen die Bilder hin?",
          Directory = current
        };

        var result = await dlg.ShowAsync( GetMainWindow() );
        if (result != null)
          return $"{result}";
      }
      catch (Exception ex)
      {
        //  _serviceProvider.GetService<ILog>().LogError($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
      }

      return "";    
    }

    public void ResetFilterStatistik()
    {
      FilterDatumExifCount = 0;
      FilterDatumFilenameCount = 0;
      FilterDatumFilechangedCount = 0;
      FilterFilenameTrashedCount = 0;
    }

    private bool filterDatumExif;
    public bool FilterDatumExif {
      get { return filterDatumExif; }
      set { 
        this.RaiseAndSetIfChanged( ref filterDatumExif, value ); 
        FilterChangedEvent?.Invoke();
        }
    }
    private int filterDatumExifCount;
    public int FilterDatumExifCount {
      get { return filterDatumExifCount; }
      set {
        this.RaiseAndSetIfChanged( ref filterDatumExifCount, value );
      }
    }

    private bool filterDatumFilename;
    public bool FilterDatumFilename {
      get { return filterDatumFilename; }
      set { 
        this.RaiseAndSetIfChanged( ref filterDatumFilename, value ); 
        FilterChangedEvent?.Invoke();
        }
    }
    private int filterDatumFilenameCount;
    public int FilterDatumFilenameCount {
      get { return filterDatumFilenameCount; }
      set {
        this.RaiseAndSetIfChanged( ref filterDatumFilenameCount, value );
      }
    }

    private bool filterDatumFilechanged;
    public bool FilterDatumFilechanged {
      get { return filterDatumFilechanged; }
      set { 
        this.RaiseAndSetIfChanged( ref filterDatumFilechanged, value ); 
        FilterChangedEvent?.Invoke();
        }
    }
    private int filterDatumFilechangedCount;
    public int FilterDatumFilechangedCount {
      get { return filterDatumFilechangedCount; }
      set {
        this.RaiseAndSetIfChanged( ref filterDatumFilechangedCount, value );
      }
    }

    private bool filterFilenameTrashed;
    public bool FilterFilenameTrashed {
      get { return filterFilenameTrashed; }
      set {
        this.RaiseAndSetIfChanged( ref filterFilenameTrashed, value );
        FilterChangedEvent?.Invoke();
      }
    }
    private int filterFilenameTrashedCount;
    public int FilterFilenameTrashedCount {
      get { return filterFilenameTrashedCount; }
      set {
        this.RaiseAndSetIfChanged( ref filterFilenameTrashedCount, value );
      }
    }
    public delegate void FilterChanged();
    public FilterChanged FilterChangedEvent;

    private bool trashCmdActive;
    public bool TrashCmdActive {
      get { return trashCmdActive; }
      set { this.RaiseAndSetIfChanged( ref trashCmdActive, value ); }
    }

    private bool delCmdActive;
    public bool DelCmdActive {
      get { return delCmdActive; }
      set { this.RaiseAndSetIfChanged( ref delCmdActive, value ); }
    }

    private bool moveCmdActive;
    public bool MoveCmdActive {
      get { return moveCmdActive; }
      set { this.RaiseAndSetIfChanged( ref moveCmdActive, value ); }
    }

    private bool copyCmdActive;
    public bool CopyCmdActive {
      get { return copyCmdActive; }
      set { this.RaiseAndSetIfChanged( ref copyCmdActive, value ); }
    }

    private int generalFontSize;
    public string GeneralFontSize {
      get { return generalFontSize.ToString(); }
      set { 
        int size = int.Parse( value );
        this.RaiseAndSetIfChanged( ref generalFontSize, size ); }
    }

    public List<string> FontSizeList {
      get { return new List<string>() { "10", "12", "14", "16", "20", "24", "30" }; }
    }

    private string title;
    public string Title {
      get { return title; }
      set { this.RaiseAndSetIfChanged( ref title, value ); }
    }

    private string description;
    public string Description {
      get { return description; }
      set { this.RaiseAndSetIfChanged( ref description, value ); }
    }


    private void Read()
    {
      service = new UserSettingsService();
      var settings = service.GetUserSettings();
      Source = settings.Quelle;
      Target = settings.Ziel;
      Trash  = settings.Papierkorb;

      FilterDatumExif = settings.FilterDatumExif;
      FilterDatumFilename = settings.FilterDatumFilename;
      FilterDatumFilechanged = settings.FilterDatumFilechanged;

      TrashCmdActive = settings.TrashCmdActive;
      DelCmdActive   = settings.DelCmdActive;
      MoveCmdActive  = settings.MoveCmdActive;
      CopyCmdActive  = settings.CopyCmdActive;
      
      GeneralFontSize = settings.GeneralFontSize.ToString();

      Description = settings.Description;
      Title = settings.Title;
    }
    private void Write()
    {
      service = new UserSettingsService();
      var settings = service.GetUserSettings();
      settings.Quelle = Source;
      settings.Ziel = Target;
      settings.Papierkorb = Trash;

      settings.FilterDatumExif = FilterDatumExif;
      settings.FilterDatumFilename = FilterDatumFilename;
      settings.FilterDatumFilechanged = FilterDatumFilechanged;

      settings.TrashCmdActive = TrashCmdActive;
      settings.DelCmdActive   = DelCmdActive;
      settings.MoveCmdActive  = MoveCmdActive;
      settings.CopyCmdActive  = CopyCmdActive;

      settings.GeneralFontSize = int.Parse(GeneralFontSize);

      settings.Title = Title;
      settings.Description = Description;

      service.SaveUserSettings( settings );
    }

  }
}