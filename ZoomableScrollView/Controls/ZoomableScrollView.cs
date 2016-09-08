using Xamarin.Forms;

namespace ZoomableScrollView
{
    /// <summary>
    /// A custom ScrollView with the following features:
    /// - Min/Max Zoom
    /// - Min Zoom is dynamic on the size of the content, so you can't zoom out past the content - this is what would prevent content going top left
    /// - Min/Max Zoom needs to "bounce" like native iOS
    /// - Content should auto-center
    /// - You want the scrollview to save its settings when hidden
    /// - Inner content should not block zooming
    /// </summary>
    public class ZoomScrollView : ScrollView
    {
        public static readonly BindableProperty MaximumZoomProperty = BindableProperty.Create(nameof(MaxZoom), typeof(double), typeof(ZoomScrollView), 1d);

        public double MaxZoom
        {
            get { return (double)GetValue(MaximumZoomProperty); }
            set { SetValue(MaximumZoomProperty, value); }
        }

        public static readonly BindableProperty MinimumZoomProperty = BindableProperty.Create(nameof(MinZoom), typeof(double), typeof(ZoomScrollView), 1d);

        public double MinZoom
        {
            get { return (double)GetValue(MinimumZoomProperty); }
            set { SetValue(MinimumZoomProperty, value); }
        }
    }
}
