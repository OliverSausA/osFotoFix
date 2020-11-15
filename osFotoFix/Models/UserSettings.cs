using System;
using System.Xml.Serialization;

namespace osFotoFix.Models
{
  [XmlRootAttribute("UserSettings")]
  public class UserSettings
  {
    [XmlAttribute]
    public string Quelle {get;set;}
    [XmlAttribute]
    public string Ziel {get;set;}
    [XmlAttribute]
    public string Papierkorb {get;set;}
  }
}