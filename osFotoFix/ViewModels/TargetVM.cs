using System;
using System.IO;
using Avalonia.Media.Imaging;
using ReactiveUI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace osFotoFix.ViewModels
{
  using System.Collections.Generic;
  using Models;
    using osFotoFix.Views;

    public class TargetVM : ViewModelBase
  {
    public TargetVM( Target target )
    {
      Target = target;
      /*
      ActionList = new List<(EAction Action, string Title)>
      {
        (EAction.copy,   "Copy"),
        (EAction.move,   "Move"),
        (EAction.delete, "Delete"),
      };
      */
      ActionList = new List<ActionItem>() {
        new ActionItem() { Value = EAction.copy, Title = "Copy"},
        new ActionItem() { Value = EAction.move, Title = "Move"},
        new ActionItem() { Value = EAction.delete, Title = "Delete"},
      };
    }

    public Target Target {get;set;}

    // public List<(EAction Action,string Title)> ActionList {get;}
    public List<ActionItem> ActionList {get;}
    public ActionItem Action {
      get { return new ActionItem(); } 
      set { Target.Action = value.Value; }
    }
  }

  public struct ActionItem
  {
    public EAction Value {get; set;}
    public string  Title {get; set;}
  }
}