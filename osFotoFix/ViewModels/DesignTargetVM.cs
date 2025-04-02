using System.Linq.Expressions;
using osFotoFix.Models;

namespace osFotoFix.ViewModels
{
  public class DesignTargetVM : TargetVM
  {
    public DesignTargetVM() : base(new Target(){
      Title = "Title of the target item",
      Path = "/home/user/pictures/somewhere",
      Action = EAction.copy,
      Enabled = true,
    })
    {
    }
  }
}