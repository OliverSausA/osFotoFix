using System;
using System.IO;
using System.Collections.Generic;
using ExifLib;


namespace osFotoFix.Services
{
  using Models;

  public class FotoInfoService
  {
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
      foreach( var d in dir.GetDirectories() )
        ReadFotoInfos(infos, d);

      foreach( var f in dir.GetFiles() ) {
        infos.Add( CreateFotoInfo(f) );
      }
      
    }

    private FotoInfo CreateFotoInfo( FileInfo file )
    {
      using( ExifReader reader = new ExifReader( file.FullName ) )
      {
        DateTime d;
        if( reader.GetTagValue<DateTime>(ExifTags.DateTimeDigitized, out d))
        {
          var fotoInfo = new FotoInfo( file, d, true );
          fotoInfo.FileLocationOld = file.DirectoryName;
          fotoInfo.FileLocationNew = Path.Combine( d.ToString("yyyy"), d.ToString("yyyy_MM") );
          fotoInfo.FileNameOld = file.Name;
          fotoInfo.FileNameNew = string.Format("{0}_{1}{2}",
            d.ToString("yyyyMMdd_hhmmss"), "TEXT", file.Extension );
          return fotoInfo;
        }
      }
      return new FotoInfo( file, file.CreationTime, false );
    }

  }
}