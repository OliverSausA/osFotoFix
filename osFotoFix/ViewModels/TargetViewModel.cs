using System;
using System.Collections.Generic;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace osFotoFix.ViewModels;

using Models;

public class TargetViewModel : ObservableObject
{
  public TargetViewModel(Target target)
  {
    Target = target;
    ActionList = new List<ActionItem>() {
      new ActionItem() { Value = EAction.copy, Title = "Copy"},
      new ActionItem() { Value = EAction.move, Title = "Move"},
      new ActionItem() { Value = EAction.delete, Title = "Delete"},
    };
    action = ActionList.Find(x => x.Value == target.Action);

    SelectPathCommand = new AsyncRelayCommand( async () => {
      var directoryPicker = new Services.DirectoryPicker();
      string path = await directoryPicker.GetFolderName( Target.Path );
      if( path != string.Empty ) {
        Path = path;
      }
    });

    RemoveCommand = new RelayCommand( () => {
      System.Diagnostics.Debug.WriteLine("Remove this TargetViewModel");
      if( RemoveThisItemEvent != null )
        RemoveThisItemEvent(this);
    });
  }

  public Target Target {get;set;}
  public string IconName
  {
    get { return Target.IconName; }
    set { 
      Target.IconName = value; 
      OnPropertyChanged();
    }
  }

  public string Path
  {
    get { return Target.Path; }
    set {
      Target.Path = value;
      OnPropertyChanged();
    }
  }

  public HsvColor IconColor
  {
    get {
      if( Target.IconColor == string.Empty )
        return new HsvColor(0,0,0,0);
      return HsvColor.Parse(Target.IconColor);
    }
    set {
      Target.IconColor = value.ToString();
      OnPropertyChanged(nameof(IconName));
      OnPropertyChanged();
    }
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

  public AsyncRelayCommand SelectPathCommand { get; }

  public RelayCommand RemoveCommand { get; }
  public delegate void RemoveThisItem(TargetViewModel target);
  public RemoveThisItem? RemoveThisItemEvent;
}

public struct ActionItem
{
  public EAction Value {get; set;}
  public string  Title {get; set;}
}