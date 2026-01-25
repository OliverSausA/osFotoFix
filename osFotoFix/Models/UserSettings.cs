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
    CultureId = "DE-de";
    GeneralFontSize = 12;
    Targets = new List<Target>();
  }

  [XmlAttribute]
  public string Source {get;set;} = string.Empty;
  [XmlArray]
  public List<Target> Targets {get; set;} = new();

  [XmlAttribute]
  public int PreviewSize {get;set;} = 300;

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
  public string CultureId {get;set;} = string.Empty;
  [XmlAttribute]
  public int GeneralFontSize {get;set;}
  [XmlAttribute]
  public bool ShowFotoInfoDetail {get;set;}

  [XmlAttribute]
  public string Title {get;set;} = string.Empty;
  [XmlAttribute]
  public string Description {get;set;} = string.Empty;
}