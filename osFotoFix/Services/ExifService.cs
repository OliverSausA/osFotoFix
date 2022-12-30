using System;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace osFotoFix.Services
{
  public class ExifService
  {
    static public DateTime? ReadCreationTime( string path )
    {
      DateTime? d = null;
      try
      {
        var exifDirectorie = ImageMetadataReader.ReadMetadata(path);
        var subIfd0Directorie = exifDirectorie.OfType<ExifIfd0Directory>().FirstOrDefault();
        d = subIfd0Directorie?.GetDateTime(ExifDirectoryBase.TagDateTime);
      }
      catch { }
      return d;
    }
  }
}