using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace osFotoFix.Views;

using ViewModels;

public partial class FotoInfoListView : UserControl
{
    ///// private DataGrid FotoList;

    public FotoInfoListView()
    {
        InitializeComponent();
        ///// FotoList = this.FindControl<DataGrid>("FotoList");
        ///// FotoList.CurrentCellChanged += OnCellChanged;
    }

    private void OnCellChanged( object sender, EventArgs e )
    {
      /*****
      try {
        var vm = (FotoInfoListViewModel) this.DataContext;
        FotoList.ScrollIntoView( vm.FotoSelected, null );
      }
      catch {
      }
      *****/
    }
}