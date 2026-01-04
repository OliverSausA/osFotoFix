using System;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace osFotoFix.Services;

public class DirectoryPicker
{
  public async Task<string> GetFolderName(string current)
  {
    string result = string.Empty;
    try
    {
      var window = App.Current.GetMainWindow();
      if (window != null && window.StorageProvider.CanPickFolder)
      {
        var folder = await window.StorageProvider.TryGetFolderFromPathAsync(current);
        var opt = new Avalonia.Platform.Storage.FolderPickerOpenOptions() {
          Title = "Get picture path", // TODO: Translation
          AllowMultiple = false,
          SuggestedStartLocation = folder
        };
        var path = await window.StorageProvider.OpenFolderPickerAsync(opt);
        if (path.Count == 1)
        {
          string? p = path[0].TryGetLocalPath();
          if (p != null)
            result = p;
        }
      }
    }
    catch (Exception ex)
    //catch
    {
      // _serviceProvider.GetService<ILog>().LogError($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
      ;
      Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
    }
     
    return result;
  }
}