using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Dialogs;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;

namespace osFotoFix.ViewModels
{
  using CommunityToolkit.Mvvm.ComponentModel;
  using Models;
  using Services;

  public partial class SettingsViewModel : ViewModelBase
  {
    private List<KeyValuePair<string,string>> SupportedLanguageList;
    private UserSettings userSettings;
    private UserSettingsService settingsService;

    public SettingsViewModel(UserSettingsService settingsService) : base()
    {
      this.settingsService = settingsService;
      userSettings = settingsService.GetUserSettings;
      Read();

      SupportedLanguageList = new List<KeyValuePair<string, string>>(){
        new KeyValuePair<string, string>("de-DE", App.GetStrRes("de-DE") ),
        new KeyValuePair<string, string>("en-US", App.GetStrRes("en-US") ),
      };
    }

    public void SaveSettings()
    {
      Write();
    }

    public async Task<string> GetFolderName(string current)
    {
      try
      {
        if( string.IsNullOrEmpty(current) ) 
          current = settingsService.getUserPicturePath();

        var window = App.Current.GetMainWindow();
        if (window != null && window.StorageProvider.CanPickFolder)
        {
          var opt = new Avalonia.Platform.Storage.FolderPickerOpenOptions() {
            Title = "Get picture path", // TODO: Translation
            AllowMultiple = false
          };
          Uri uri = new Uri(current);
          var path = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions());
          if (path.Count == 1)
          {
            string? p = path[0].TryGetLocalPath();
            if (p != null)
              current = p;
          }
        }
        return current;
      }
      catch (Exception ex)
      //catch
      {
        // _serviceProvider.GetService<ILog>().LogError($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
        ;
        Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
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

    public EFilterState FilterDatumExif {
      get { return userSettings.FilterDatumExif; }
      set { 
        if( userSettings.FilterDatumExif != value )
        {
          userSettings.FilterDatumExif = value;
          ///// this.RaisePropertyChanged( nameof( FilterDatumExif ) );
          OnPropertyChanged();
          FilterChangedEvent?.Invoke();
        }
      }
    }
    [ObservableProperty]
    private int filterDatumExifCount;

    public EFilterState FilterDatumFilename {
      get { return userSettings.FilterDatumFilename; }
      set { 
        if( userSettings.FilterDatumFilename != value )
        {
          userSettings.FilterDatumFilename = value;
          ///// this.RaisePropertyChanged( nameof( FilterDatumFilename ) );
          OnPropertyChanged();
          FilterChangedEvent?.Invoke();
        }
      }
    }
    [ObservableProperty]
    private int filterDatumFilenameCount;

    public EFilterState FilterDatumFilechanged {
      get { return userSettings.FilterDatumFilechanged; }
      set { 
        if( userSettings.FilterDatumFilechanged != value )
        {
          userSettings.FilterDatumFilechanged = value;
          ///// this.RaisePropertyChanged( nameof( FilterDatumFilechanged ) );
          OnPropertyChanged();
          FilterChangedEvent?.Invoke();
        }
      }
    }
    [ObservableProperty]
    private int filterDatumFilechangedCount;

    public EFilterState FilterFilenameTrashed {
      get { return userSettings.FilterFilenameTrashed; }
      set {
        if( userSettings.FilterFilenameTrashed != value )
        {
          userSettings.FilterFilenameTrashed = value;
          ////// this.RaisePropertyChanged( nameof( FilterFilenameTrashed ) );
          OnPropertyChanged();
          FilterChangedEvent?.Invoke();
        }
      }
    }

    [ObservableProperty]
    private int filterFilenameTrashedCount;

    public EFilterState FilterDoubles 
    {
      get { return userSettings.FilterDoubles; }
      set {
        if( userSettings.FilterDoubles != value )
        {
          userSettings.FilterDoubles = value;
          ///// this.RaisePropertyChanged(nameof(FilterDoubles));
          OnPropertyChanged();
          FilterChangedEvent?.Invoke();
        }
      }
    }

    [ObservableProperty]
    private int filterDoublesCount;

    public delegate void FilterChanged();
    public FilterChanged? FilterChangedEvent;

    [ObservableProperty]
    private bool trashCmdActive;

    [ObservableProperty]
    private bool delCmdActive;

    [ObservableProperty]
    private bool moveCmdActive;

    [ObservableProperty]
    private bool copyCmdActive;

    [ObservableProperty]
    private int generalFontSize;
    /*****
    public string GeneralFontSize {
      get { return generalFontSize.ToString(); }
      set { 
        int size = int.Parse( value );
        this.RaiseAndSetIfChanged( ref generalFontSize, size ); }
    }
    *****/

    public List<string> FontSizeList {
      get { return new List<string>() { "10", "12", "14", "16", "20", "24", "30" }; }
    }

    [ObservableProperty]
    private string cultureId = string.Empty;

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

    [ObservableProperty]
    private bool showFotoInfoDetail;

    [ObservableProperty]
    private string source = string.Empty;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    private void Read()
    {
      var settings = settingsService.GetUserSettings;
      Source = settings.Source;

      TrashCmdActive = settings.TrashCmdActive;
      DelCmdActive   = settings.DelCmdActive;
      MoveCmdActive  = settings.MoveCmdActive;
      CopyCmdActive  = settings.CopyCmdActive;
      
      CultureId = settings.CultureId;
      GeneralFontSize = settings.GeneralFontSize;
      ShowFotoInfoDetail = settings.ShowFotoInfoDetail;

      Description = settings.Description;
      Title = settings.Title;
    }
    private void Write()
    {
      var settings = settingsService.GetUserSettings;
      settings.Source = Source;

      settings.TrashCmdActive = TrashCmdActive;
      settings.DelCmdActive   = DelCmdActive;
      settings.MoveCmdActive  = MoveCmdActive;
      settings.CopyCmdActive  = CopyCmdActive;

      settings.CultureId = CultureId;
      settings.GeneralFontSize = GeneralFontSize;
      settings.ShowFotoInfoDetail = ShowFotoInfoDetail;

      settings.Title = Title;
      settings.Description = Description;

      settingsService.SaveUserSettings();
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