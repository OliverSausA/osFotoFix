using System;
using System.Xml.Serialization;

namespace osFotoFix.Models
{
  
  public class Target
  {
    /*
    public enum EAction {
      ignore = -1,
      copy,
      move,
      delete
    };
    */

    [XmlAttribute]
    public string Title {get; set;}

    [XmlAttribute]
    public int IconId {get; set;}

    [XmlAttribute]
    public string Path {get; set;}

    [XmlAttribute]
    public EAction Action {get; set;}

    [XmlAttribute]
    public bool Enabled {get; set;}

   }
}
