using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ExifLib;


namespace osFotoFix.Services
{
  using Models;

  public class FotoInfoService
  {
    const string ext = ".jpg.bmp";
    public FotoInfoService()
    {

    }

    public List<FotoInfo> GetFotoInfos( DirectoryInfo baseDir )
    {
      var fotoInfos = new List<FotoInfo>();
      ReadFotoInfos( fotoInfos, baseDir );
      return fotoInfos;
    }

    public string GetNewFileName( FotoInfo foto )
    {
      return Path.Combine(
          foto.Target,
          CreateNewFileLocation(foto),
          CreateNewFileName(foto) );
    }
    
    public string CopyFoto( FotoInfo foto )
    {
      var dir = new DirectoryInfo( Path.Combine(
        foto.Target,
        CreateNewFileLocation( foto )));
      dir.Create();

      foto.NewFileName = GetNewFileName( foto );
      foto.File.CopyTo( foto.NewFileName );
      AdjustTimeStamp(foto.NewFileName, foto.Created);
      return foto.NewFileName;
    }
    public string MoveFoto( FotoInfo foto )
    {
      var dir = new DirectoryInfo( Path.Combine(
        foto.Target,
        CreateNewFileLocation( foto )));
      dir.Create();
      foto.NewFileName = GetNewFileName( foto );
      foto.File.MoveTo( foto.NewFileName );
      AdjustTimeStamp(foto.NewFileName, foto.Created);
      return foto.NewFileName;
    }
    public void DeleteFoto(FotoInfo foto)
    {
      foto.File.Delete();
    }

    private void ReadFotoInfos( List<FotoInfo> infos, DirectoryInfo dir )
    {
      if(!dir.Exists) return;
      if( ( dir.Attributes & FileAttributes.Hidden ) != 0 ) return;
        
      foreach( var d in dir.EnumerateDirectories().OrderBy( d => d.Name ) )
        ReadFotoInfos(infos, d);

      foreach( var f in dir.EnumerateFiles().OrderBy( f => f.Name ) ) {
        if( (!string.IsNullOrEmpty( f.Extension ) ) &&
            (ext.IndexOf(f.Extension, 0, StringComparison.InvariantCultureIgnoreCase ) >= 0 ) )
          infos.Add( CreateFotoInfo(f) );
      }
    }

    private FotoInfo CreateFotoInfo( FileInfo file )
    {
      try
      {
        using( ExifReader reader = new ExifReader( file.FullName ) )
        {
          DateTime d;
          if( reader.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out d))
            return new FotoInfo( file, d, FotoInfo.ETypeOfCreationDate.Exif );
        }
      }
      catch { }

      DateTime dt;
      if( GetDateTimeFromString( file.Name, out dt ) )
        return new FotoInfo( file, dt, FotoInfo.ETypeOfCreationDate.Filename );
      
      return new FotoInfo( file, file.CreationTime, FotoInfo.ETypeOfCreationDate.Filesystem );
    }

    private bool GetDateTimeFromString( string text, out DateTime dt )
    {
      dt = DateTime.MinValue;
      string pattern = @"(\d{2,4})[\._-]?(\d{2})[\._-]?(\d{2})[_-]?(\d{2})[:_-]?(\d{2})[:_-]?(\d{2})";
      var match = Regex.Match( text, pattern );
      if( !match.Success ) return false;

      string dtStr = "";
      for( int i=1; i<match.Groups.Count; i++ )
        dtStr += match.Groups[i].Value;
      
      bool success = DateTime.TryParseExact(
                        dtStr, "yyyyMMddHHmmss", 
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeLocal, out dt);
      if( success ) 
        return true;
      
      return DateTime.TryParseExact(
                        dtStr, "yyMMddHHmmss", 
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeLocal, out dt);

    }

    protected string CreateNewFileName( FotoInfo foto )
    {
      int idx = 0;
      foto.FileExistsOnTarget = false;
      string name;
      FileInfo fileInfo;
      do
      {
        name = CreateNewFileName( foto, idx++ );
        fileInfo = new FileInfo( Path.Combine( foto.Target, CreateNewFileLocation(foto), name ) );
        if( fileInfo.Exists && fileInfo.Length == foto.File.Length )
          foto.FileExistsOnTarget = true;
      }
      while ( fileInfo.Exists );
      return name;
    }

    protected string CreateNewFileName( FotoInfo foto, int idx = 0 )
    {
      string postfix = "";
      if( idx > 0) postfix = string.Format("_{0}", idx);
      return string.Format("{0}_{1}{2}{3}",
               foto.Created.ToString("yyyyMMdd_hhmmss"),
               foto.Description,
               postfix,
               foto.File.Extension );
    }

    protected string CreateNewFileLocation( FotoInfo foto )
    {
      string path = Path.Combine(
              foto.Created.ToString("yyyy"),
              foto.Created.ToString("yyyy_MM") );
      if( !string.IsNullOrEmpty( foto.Title ) )
        path += "-" + foto.Title;
      return path;
    }

    private void AdjustTimeStamp( string file, DateTime timestamp )
    {
      var f = new FileInfo( file );
      if( f.Exists && f.CreationTime != timestamp )
        File.SetCreationTime( file, timestamp );
    }
  }
}
