using System.IO;
using System.Linq.Expressions;
using osFotoFix.Models;

namespace osFotoFix.ViewModels
{
  public class DesignFotoInfoViewModel : FotoInfoViewModel
  {
    // private static string testFotoPath = "../../../../Help/Pic/PXL_20211022_120552313.jpg";
    private static string testFotoPath = "/daten/workspace/osFotoFix/Help/Pic/PXL_20211022_120552313.jpg";

    public DesignFotoInfoViewModel() : base(new FotoInfo( new FileInfo(testFotoPath), System.DateTime.Now, ETypeOfCreationDate.Filename))
    {
      Title = "Title of the FotoInfo item";
    }
  }
}