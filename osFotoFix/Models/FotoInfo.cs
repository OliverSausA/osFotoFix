using System;
using System.IO;

namespace osFotoFix.Models
{
  using ViewModels;
  public class FotoInfo {

    public FotoInfo( FileInfo file, DateTime created, bool isExifValid ) {
      File = file;
      Created = created;
      IsExifValid = isExifValid;
      FileNameNew = "DAS IST TEST";
    }

    public FileInfo File {get;set;}

    public DateTime Created {get;set;}

    public bool IsExifValid {get;set;}
    public string FileLocationOld {get;set;}
    public string FileNameOld {get;set;}
    public string FileLocationNew {get;set;}
    public string FileNameNew {get;set;}


    public override string ToString()
    {
      return string.Format( "{0} | {1} | {2} | {3} | {4} | {5} | {6} | {7}",
        IsExifValid, 
        Created.ToString("yyyy_MM"), 
        Created.ToString("yyyyMMdd_hhmmss"), 
        File.Name,
        FileLocationOld,
        FileLocationNew,
        FileNameOld,
        FileNameNew
        );
    }
  }

}