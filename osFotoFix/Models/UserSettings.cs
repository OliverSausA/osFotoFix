using System;
using System.Xml.Serialization;

namespace osFotoFix.Models
{
  [XmlRootAttribute("UserSettings")]
  public class UserSettings
  {
    public UserSettings() {
      FilterDatumExif = true;
      FilterDatumFilename = true;
      FilterDatumFilechanged = true;
      TrashCmdActive = true;
      DelCmdActive = true;
      MoveCmdActive = true;
      CopyCmdActive = true;
      CultureId = "DE-de";
      GeneralFontSize = 12;
    }

    [XmlAttribute]
    public string Quelle {get;set;}
    [XmlAttribute]
    public string Ziel {get;set;}
    [XmlAttribute]
    public string Papierkorb {get;set;}

    [XmlAttribute]
    public bool FilterDatumExif {get;set;}
    [XmlAttribute]
    public bool FilterDatumFilename {get;set;}
    [XmlAttribute]
    public bool FilterDatumFilechanged {get;set;}
    [XmlAttribute]
    public bool FilterFilenameTrashed {get;set;}

    [XmlAttribute]
    public bool TrashCmdActive {get;set;}
    [XmlAttribute]
    public bool DelCmdActive {get;set;}
    [XmlAttribute]
    public bool MoveCmdActive {get;set;}
    [XmlAttribute]
    public bool CopyCmdActive {get;set;}

    [XmlAttribute]
    public string CultureId {get;set;}
    [XmlAttribute]
    public int GeneralFontSize {get;set;}
    [XmlAttribute]
    public bool ShowFotoInfoDetail {get;set;}

    [XmlAttribute]
    public string Title {get;set;}
    [XmlAttribute]
    public string Description {get;set;}
  }
}