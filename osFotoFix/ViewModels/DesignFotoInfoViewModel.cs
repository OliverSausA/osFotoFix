using System.IO;
using System.Linq.Expressions;
using osFotoFix.Models;

namespace osFotoFix.ViewModels
{
  public class DesignFotoInfoViewModel : FotoInfoViewModel
  {
    // private static string testFotoPath = "../../../../Help/Pic/PXL_20211022_120552313.jpg";
    private static string testFotoPath = "/daten/workspace/osFotoFix/Help/Pic/PXL_20211022_120552313.jpg";

    public DesignFotoInfoViewModel() : base(new FotoInfo( 
      new FileInfo(testFotoPath), System.DateTime.Now, ETypeOfCreationDate.Filename), 300)
    {
      // Title = "Title of the FotoInfo item";
      // Description = "Description of the FotoInfo item";
      Comment = "This is a comment for the foto info item.";
      Target = new Target()
      {
        Title = "Target Title",
        Description = "Target Description",
        Path = "/path/to/target/folder",
        IconColor = "#FF5733",
        IconName = "Folder",
        Action = EAction.copy
      };
    }
  }
}