using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace osFotoFix.Services
{
  using Models;
  public class UserSettingsService /* : IUserSettingsService*/
  {
    public UserSettingsService() {
      ReadUserSettings();
    }
    private const string CONF = "osFotoFix.config";
    protected UserSettings userSettings = new();

    public UserSettings GetUserSettings {
      get { return userSettings; }
    }

    protected virtual void ReadUserSettings()
    {
      try
      {
        string filePath = Path.Combine( getUserHomePath(), CONF );
        if( File.Exists( filePath ) ) {
          using ( var reader = XmlReader.Create( filePath ) )
          {
            XmlSerializer serializier = new XmlSerializer( typeof( UserSettings ) );
            var data = serializier.Deserialize( reader );
            if (data is UserSettings)
              userSettings = (UserSettings)data;
          }
        }
      }
      catch {}
    }

    public virtual void SaveUserSettings()
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

    public virtual string getUserHomePath()
    {
      return Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData);
    }

    public virtual string getUserPicturePath()
    {
      return Environment.GetFolderPath(
                Environment.SpecialFolder.MyPictures);
    }
  }
}