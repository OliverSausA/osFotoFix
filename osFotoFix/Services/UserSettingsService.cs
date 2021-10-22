using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace osFotoFix.Services
{
  using Models;
  public class UserSettingsService
  {
    private UserSettingsService() {
      ReadUserSettings();
    }

    private static UserSettingsService instance = null;
    private const string CONF = "osFotoFix.config";
    private UserSettings userSettings;

    public static UserSettingsService GetInstance {
      get { 
        if (instance == null )
          instance = new UserSettingsService();
        return instance;
      }
    }
    public UserSettings GetUserSettings {
      get { return userSettings; }
    }

    private void ReadUserSettings()
    {
      if( userSettings != null )
        return ;

      userSettings = new UserSettings();
      try
      {
        string filePath = Path.Combine( getUserHomePath(), CONF );
        if( File.Exists( filePath ) ) {
          using ( var reader = XmlReader.Create( filePath ) )
          {
            XmlSerializer serializier = new XmlSerializer( typeof( UserSettings ) );
            userSettings = (UserSettings) serializier.Deserialize( reader );
          }
        }
      }
      catch {}
    }

    public void SaveUserSettings()
    {
      try
      {
        string filePath = Path.Combine( getUserHomePath(), CONF );
        var writerSettings = new XmlWriterSettings() {
          Indent = true,
          NewLineOnAttributes = true
        };
        using ( var writer = XmlWriter.Create( filePath, writerSettings ) )
        {
          XmlSerializer serializier = new XmlSerializer( typeof( UserSettings ) );
          serializier.Serialize(writer, userSettings);
        }
      }
      catch ( Exception e )
      {
        Console.WriteLine( e.Message );
      }
    }

    public static string getUserHomePath()
    {
      return Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData);
    }

    public static string getUserPicturePath()
    {
      return Environment.GetFolderPath(
                Environment.SpecialFolder.MyPictures);
    }
  }
}