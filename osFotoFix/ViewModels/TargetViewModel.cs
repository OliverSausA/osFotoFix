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

  public string IconColor
  {
    get { return Target.IconColor; }
    set {
      Target.IconColor = value;
      OnPropertyChanged(nameof(IconName));
      OnPropertyChanged();
    }
  }

  public string Path
  {
    get { return Target.Path; }
    set {
      Target.Path = value;
      OnPropertyChanged();
      OnPropertyChanged(nameof(IsPathValid));
    }
  }

  public bool IsPathValid
  {
    get { return System.IO.Directory.Exists( Target.Path ); }
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

  public HsvColor HsvColor
  {
    get {
      if (string.IsNullOrEmpty( Target.IconColor ))
        return new HsvColor();
      var color = Color.Parse( Target.IconColor );
      return color.ToHsv();
    }
    set {
      var color = value.ToRgb();
      Target.IconColor = color.ToString();
      OnPropertyChanged(nameof(IconColor));
    }
  }

  public bool Enabled
  {
    get { return Target.Enabled; }
    set {
      Target.Enabled = value;
      OnPropertyChanged();
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