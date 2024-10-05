using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace osFotoFix.Views
{
    public class FotoPreviewView : UserControl
    {
        public FotoPreviewView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
