using System;
using System.Xml.Serialization;

namespace osFotoFix.Models
{
  [XmlRootAttribute("UserSettings")]
  public class UserSettings
  {
    public UserSettings() {
      TrashCmdActive = true;
      DelCmdActive = true;
      MoveCmdActive = true;
      CopyCmdActive = true;
    }

    [XmlAttribute]
    public string Quelle {get;set;}
    [XmlAttribute]
    public string Ziel {get;set;}
    [XmlAttribute]
    public string Papierkorb {get;set;}

    [XmlAttribute]
    public bool TrashCmdActive {get;set;}
    [XmlAttribute]
    public bool DelCmdActive {get;set;}
    [XmlAttribute]
    public bool MoveCmdActive {get;set;}
    [XmlAttribute]
    public bool CopyCmdActive {get;set;}
  }
}