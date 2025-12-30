using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace osFotoFix.Models;
public enum EFilterState
{
  eDisable,
  eOn,
  eOff,
}

[XmlRootAttribute("UserSettings")]
public class UserSettings
{
  public UserSettings() {
    FilterDatumExif = EFilterState.eDisable;
    FilterDatumFilename = EFilterState.eDisable;
    FilterDatumFilechanged = EFilterState.eDisable;
    FilterFilenameTrashed = EFilterState.eDisable;
    FilterDoubles = EFilterState.eDisable;
    TrashCmdActive = true;
    DelCmdActive = true;
    MoveCmdActive = true;
    CopyCmdActive = true;
    CultureId = "DE-de";
    GeneralFontSize = 12;
    Targets = new List<Target>();
  }

  [XmlAttribute]
  public string Quelle {get;set;}
  [XmlAttribute]
  public string Ziel {get;set;}
  [XmlAttribute]
  public string Papierkorb {get;set;}
  [XmlArray]
  public List<Target> Targets {get; set;}

  [XmlAttribute]
  public EFilterState FilterDatumExif {get;set;}
  [XmlAttribute]
  public EFilterState FilterDatumFilename {get;set;}
  [XmlAttribute]
  public EFilterState FilterDatumFilechanged {get;set;}
  [XmlAttribute]
  public EFilterState FilterFilenameTrashed {get;set;}
  [XmlAttribute]
  public EFilterState FilterDoubles {get;set;}

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