using System;
using System.IO;

namespace osFotoFix.Models
{
  public enum EAction {
    ignore = -1,
    copy,
    move,
    delete,
    trash,
    done,
    failed
  };

  public class FotoInfo {

    private static int id;

    public enum ETypeOfCreationDate
    {
      Exif,
      Filename,
      Filesystem 
    }

    public FotoInfo( FileInfo file, DateTime created, ETypeOfCreationDate typeOfCreationDate) {
      ID = ++id;
      Action = EAction.ignore;
      File = file;
      Created = created;
      TypeOfCreationDate = typeOfCreationDate;
    }

    public FileInfo File {get;set;}

    public DateTime Created {get;set;}

    public ETypeOfCreationDate TypeOfCreationDate {get;set;}

    public int ID { get; private set; }
    public EAction Action {get;set;}
    public string Target {get;set;}
    public string Title {get;set;}
    public string Description {get;set;}

    public string NewFileName {get;set;}
    public bool FileExistsOnTarget {get;set;}

    public string Comment {get;set;}
    public bool ActionRequiered {
      get { return ( Action == EAction.copy ||
                     Action == EAction.move ||
                     Action == EAction.trash ||
                     Action == EAction.delete ); }
    }
    public string Data {get;set;}
  }

}