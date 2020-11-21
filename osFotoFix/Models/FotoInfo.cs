using System;
using System.IO;

namespace osFotoFix.Models
{
  using ViewModels;
  public class FotoInfo {

    public enum ETypeOfCreationDate
    {
      Exif,
      Filename,
      Filesystem 
    }

    public FotoInfo( FileInfo file, DateTime created, ETypeOfCreationDate typeOfCreationDate) {
      File = file;
      Created = created;
      TypeOfCreationDate = typeOfCreationDate;
    }

    public FileInfo File {get;set;}

    public DateTime Created {get;set;}

    public ETypeOfCreationDate TypeOfCreationDate {get;set;}

  }

}