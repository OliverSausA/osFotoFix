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
    private List<KeyValuePair<string,string>> SupportedLanguageList;

    public SettingsViewModel() : base()
    {
      Read();
      SelectSourceCmd = ReactiveCommand.Create( OnSelectSource );
      SelectTargetCmd = ReactiveCommand.Create( OnSelectTarget );
      SelectTrashCmd  = ReactiveCommand.Create( OnSelectTrash );

      SupportedLanguageList = new List<KeyValuePair<string, string>>(){
        new KeyValuePair<string, string>("de-DE", App.GetStrRes("de-DE") ),
        new KeyValuePair<string, string>("en-US", App.GetStrRes("en-US") ),
      };
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
          current = UserSettingsService.getUserPicturePath();
        var dlg = new OpenFolderDialog() {
          Title = "Wo sollen die Bilder hin?",
          Directory = current
        };

        var result = await dlg.ShowAsync( GetMainWindow() );
        if (result != null)
          return $"{result}";
      }
      //catch (Exception ex)
      catch
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
      FilterDoublesCount = 0;
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

    private EFilterState filterDoubles;
    public EFilterState FilterDoubles 
    {
      get { return filterDoubles; }
      set {
        this.RaiseAndSetIfChanged( ref filterDoubles, value );
        FilterChangedEvent?.Invoke();
      }
    }
    /*****
    public bool? FilterDoubles
    {
      get {
        bool? ret = null;
        if( filterDoubles == EFilterState.eOn )
          ret = true;
        else if( filterDoubles == EFilterState.eOff )
          ret = false;
        return ret;
      }

      set {
        if( !value.HasValue )
          filterDoubles = EFilterState.eDisable;
        else if( value.Value )
          filterDoubles = EFilterState.eOn;
        else
          filterDoubles = EFilterState.eOff;
      }
    }
    *****/
    /*****
    public EFilterState FilterDoubles 
    {
      get { return filterDoubles; }
      set {
        var state = EFilterState.eDisable;
        if( filterDoubles == EFilterState.eDisable )
          state = EFilterState.eOn;
        else if( filterDoubles == EFilterState.eOn )
          state = EFilterState.eOff;
        this.RaiseAndSetIfChanged( ref filterDoubles, state );
        FilterChangedEvent?.Invoke();
      }
    }
    *****/

    private int filterDoublesCount;
    public int FilterDoublesCount {
      get { return filterDoublesCount; }
      set {
        this.RaiseAndSetIfChanged( ref filterDoublesCount, value );
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

    private string cultureId;
    public string CultureId {
      get { return cultureId; }
      set { cultureId = value; }
    }

    public List<KeyValuePair<string,string>> SupportedLanguages
    {
      get {
        return SupportedLanguageList;
      }
    }
    public KeyValuePair<string,string> CurrentLanguage
    {
      get { return SupportedLanguageList.Find( x => x.Key == CultureId); }
      set { 
        CultureId = value.Key; 
        App.SelectLanguage(CultureId);
      }
    }

    private bool showFotoInfoDetail;
    public bool ShowFotoInfoDetail { 
      get {return showFotoInfoDetail; }
      set { this.RaiseAndSetIfChanged( ref showFotoInfoDetail, value ); }
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
      var settings = UserSettingsService.GetInstance.GetUserSettings;
      Source = settings.Quelle;
      Target = settings.Ziel;
      Trash  = settings.Papierkorb;

      FilterDatumExif = settings.FilterDatumExif;
      FilterDatumFilename = settings.FilterDatumFilename;
      FilterDatumFilechanged = settings.FilterDatumFilechanged;
      filterDoubles = settings.FilterDoubles;

      TrashCmdActive = settings.TrashCmdActive;
      DelCmdActive   = settings.DelCmdActive;
      MoveCmdActive  = settings.MoveCmdActive;
      CopyCmdActive  = settings.CopyCmdActive;
      
      CultureId = settings.CultureId;
      GeneralFontSize = settings.GeneralFontSize.ToString();
      ShowFotoInfoDetail = settings.ShowFotoInfoDetail;

      Description = settings.Description;
      Title = settings.Title;
    }
    private void Write()
    {
      var settings = UserSettingsService.GetInstance.GetUserSettings;
      settings.Quelle = Source;
      settings.Ziel = Target;
      settings.Papierkorb = Trash;

      settings.FilterDatumExif = FilterDatumExif;
      settings.FilterDatumFilename = FilterDatumFilename;
      settings.FilterDatumFilechanged = FilterDatumFilechanged;
      settings.FilterDoubles = FilterDoubles;

      settings.TrashCmdActive = TrashCmdActive;
      settings.DelCmdActive   = DelCmdActive;
      settings.MoveCmdActive  = MoveCmdActive;
      settings.CopyCmdActive  = CopyCmdActive;

      settings.CultureId = CultureId;
      settings.GeneralFontSize = int.Parse(GeneralFontSize);
      settings.ShowFotoInfoDetail = ShowFotoInfoDetail;

      settings.Title = Title;
      settings.Description = Description;

      UserSettingsService.GetInstance.SaveUserSettings();
    }

		public static string VersionInfo
		{
			get
			{
				var asm = System.Reflection.Assembly.GetExecutingAssembly();
				var fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(asm.Location);
#if PRODUCTION
				string conf = "PRODUCTION";
#elif RELEASE
				string conf = "RELEASE CANDIDATE";
#else
				string conf = "DEBUG";
#endif
				return string.Format("osFotoFix v{0}.{1}.{2}.{3} ({4})",
														 fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart, fvi.FilePrivatePart, conf);
			}
		}
		public static string AppInfo
		{
			get
			{
				return "OliverSausA, OS, 2021";
			}
		}
  }
}