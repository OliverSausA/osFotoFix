using System;
using System.IO;
using Avalonia.Media.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace osFotoFix.ViewModels
{
  using System.Collections.Generic;
  using Models;
    using osFotoFix.Views;

    public class TargetVM : ViewModelBase
  {
    public TargetVM(Target target)
    {
      Target = target;
      ActionList = new List<ActionItem>() {
        new ActionItem() { Value = EAction.copy, Title = "Copy"},
        new ActionItem() { Value = EAction.move, Title = "Move"},
        new ActionItem() { Value = EAction.delete, Title = "Delete"},
      };
      action = ActionList.Find(x => x.Value == target.Action);
    }

    public Target Target {get;set;}
    public string IconName
    {
      get { return Target.IconName; }
      set { Target.IconName = value; }
    }

    public List<ActionItem> ActionList {get;}

    private ActionItem action;
    public ActionItem Action {
      get { return action; }
      set {
        action = value;
        Target.Action = value.Value;
      }
    }
  }

  public struct ActionItem
  {
    public EAction Value {get; set;}
    public string  Title {get; set;}
  }
}