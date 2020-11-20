using System;
using System.Collections.Generic;
using System.Text;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace osFotoFix.ViewModels
{
  public class ViewModelBase : ReactiveObject
  {
    protected Window GetMainWindow()
    {
      var alt = Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
      return alt?.MainWindow;
    }
  }
}
