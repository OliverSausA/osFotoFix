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
    
    private void ReadFotoInfos( List<FotoInfo> infos, DirectoryInfo dir )
    {
      if(!dir.Exists) return;
      if( ( dir.Attributes & FileAttributes.Hidden ) != 0 ) return;
        
      //foreach( var d in dir.GetDirectories() )
      foreach( var d in dir.EnumerateDirectories().OrderBy( d => d.Name ) )
        ReadFotoInfos(infos, d);

      //foreach( var f in dir.GetFiles() ) {
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

  }
}
