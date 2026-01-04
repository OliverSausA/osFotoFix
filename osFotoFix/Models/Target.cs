using System;
using System.Xml.Serialization;
using Avalonia.Media;

namespace osFotoFix.Models;
  
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
  public string Title {get; set;} = string.Empty;

  [XmlAttribute]
  public string Description {get; set;} = string.Empty;

  [XmlAttribute]
  public string IconName {get; set;} = string.Empty;

  [XmlAttribute]
  public string IconColor {get; set;} = string.Empty;

  [XmlAttribute]
  public string Path {get; set;} = string.Empty;

  [XmlAttribute]
  public EAction Action {get; set;}

  [XmlAttribute]
  public bool Enabled {get; set;}
}
