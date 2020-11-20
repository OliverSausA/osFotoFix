using System;
using System.IO;
using System.Linq;
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
        if( ext.IndexOf(f.Extension, 0, StringComparison.InvariantCultureIgnoreCase ) >= 0 )
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
            return new FotoInfo( file, d, true );
        }
      }
      catch { }
      return new FotoInfo( file, file.CreationTime, false );
    }

  }
}