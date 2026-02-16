using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using osFotoFix.ViewModels;

namespace osFotoFix.Views;

public partial class MainFotoView : UserControl
{
    FotoPreviewView? fotoPreviewView = null;

    public MainFotoView()
    {
        InitializeComponent();
    }

    private void ListBox_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (DataContext is not MainFotoViewModel vm)
            return;

        // Handle double-click event here
        if (fotoPreviewView == null)
        {
            fotoPreviewView = new FotoPreviewView();
            fotoPreviewView.Closed += (s, args) => fotoPreviewView = null;
            fotoPreviewView.DataContext = vm.FotoPreview;
            fotoPreviewView.Show();
        }
    }
}