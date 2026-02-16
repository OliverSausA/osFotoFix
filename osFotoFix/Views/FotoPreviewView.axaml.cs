using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Input;
using MetadataExtractor.Formats.Raf;
using osFotoFix.ViewModels;

namespace osFotoFix.Views
{
    public partial class FotoPreviewView : Window
    {
        private bool _isDragging;
        private Avalonia.Point _dragStart;
        private double _startOffsetX;
        private double _startOffsetY;

        public FotoPreviewView()
        {
            InitializeComponent();
            var scrollView = this.PART_ScrollViewer;
            scrollView.AddHandler(ScrollViewer.PointerWheelChangedEvent, OnPointerWheelChanged, RoutingStrategies.Tunnel);

            this.SizeChanged += (_,_) => FitToWindow();
            this.PART_Image.Loaded += (_,_) => FitToWindow();
        }

        private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
                return;

            if (DataContext is not FotoPreviewViewModel vm)
                return;

            var image = this.FindControl<Image>("PART_Image");

            if (image == null)
                return;

            var mousePos = e.GetPosition(image);

            double zoomFactor = e.Delta.Y > 0 ? 1.2 : 1 / 1.2;
            double oldZoom = vm.Zoom;
            double newZoom = oldZoom * zoomFactor;

            vm.SetZoom(newZoom);

            double scaleChange = vm.Zoom / oldZoom;

            // Zoom um Mausposition korrigieren
            vm.OffsetX = (vm.OffsetX - mousePos.X) * scaleChange + mousePos.X;
            vm.OffsetY = (vm.OffsetY - mousePos.Y) * scaleChange + mousePos.Y;

            e.Handled = true;
        }
        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
                return;

            if (e.Key == Key.D0)
            {
                e.Handled = true;
                FitToWindow();
            }
        }

        private void FitToWindow()
        {
            if (DataContext is not FotoPreviewViewModel vm)
                return;
            
            var scrollViewer = this.PART_ScrollViewer;
            var image = this.PART_Image;

            if (scrollViewer == null || image == null || image.Source == null )
                return;

            var bitmap = image.Source as Avalonia.Media.Imaging.Bitmap;
            if (bitmap == null)
                return;
            
            double imageWidth = bitmap.PixelSize.Width;
            double imageHeight = bitmap.PixelSize.Height;
            double viewportWidth = scrollViewer.Viewport.Width;
            double viewportHeight = scrollViewer.Viewport.Height;

            if (viewportWidth <= 0 || viewportHeight <= 0)
                return;
            
            double scaleX = viewportWidth / imageWidth;
            double scaleY = viewportHeight / imageHeight;

            double fitZoom = Math.Min(scaleX, scaleY);
            vm.Zoom = fitZoom;
            vm.OffsetX = 0;
            vm.OffsetY = 0;
        }
        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (DataContext is not FotoPreviewViewModel vm)
                return;

            if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                _isDragging = true;
                _dragStart = e.GetPosition(this);

                _startOffsetX = vm.OffsetX;
                _startOffsetY = vm.OffsetY;

                this.Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.SizeAll);

                e.Pointer.Capture((IInputElement)sender!);
            }
        }
        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (!_isDragging)
                return;

            if (DataContext is not FotoPreviewViewModel vm)
                return;

            var currentPos = e.GetPosition(this);

            var delta = currentPos - _dragStart;

            vm.OffsetX = _startOffsetX + delta.X;
            vm.OffsetY = _startOffsetY + delta.Y;
        }
        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (!_isDragging)
                return;

            _isDragging = false;

            this.Cursor = new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Arrow);

            e.Pointer.Capture(null);
        }

    }
}
