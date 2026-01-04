using System.Linq.Expressions;
using osFotoFix.Models;

namespace osFotoFix.ViewModels
{
  public class DesignTargetViewModel : TargetViewModel
  {
    public DesignTargetViewModel() : base(new Target(){
      Title = "Title of the target item",
      Path = "/home/user/pictures/somewhere",
      IconName = "Target",
      Action = EAction.copy,
      Enabled = true,
    })
    {
    }
  }
}