using System.IO;
using System.Linq.Expressions;
using osFotoFix.Models;

namespace osFotoFix.ViewModels
{
  public class DesignFotoInfoViewModel : FotoInfoViewModel
  {
    private static string testFotoPath = "../../../../Help/Pic/osFotoFix.png";

    public DesignFotoInfoViewModel() : base(new FotoInfo( new FileInfo(testFotoPath), System.DateTime.Now, ETypeOfCreationDate.Filename))
    {
      Title = "Title of the FotoInfo item";
    }
  }
}