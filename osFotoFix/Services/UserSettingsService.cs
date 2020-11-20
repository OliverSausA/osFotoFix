using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace osFotoFix.Services
{
  using Models;
  public class UserSettingsService
  {
    private const string CONF = "osFotoFix.config";

    public UserSettingsService()
    {
    }

    public UserSettings GetUserSettings()
    {
      var userSettings = new UserSettings();
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
        else
        {
          userSettings.Quelle = "/home/oliver/Bilder/import";
          userSettings.Ziel = "/home/oliver/Bilder/FotoSammlung";
          userSettings.Papierkorb = "/home/oliver/Bilder/papierkorb";
        }
      }
      catch
      {
      }
      return userSettings;
    }

    public void SaveUserSettings( UserSettings userSettings )
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

    private string getUserHomePath()
    {
      return Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData);
    }

  }
}