using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace osFotoFix.Views
{
    public class FotoInfoDetailView : UserControl
    {
        public FotoInfoDetailView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}