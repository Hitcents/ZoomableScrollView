using System;
using System.ComponentModel;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ZoomableScrollView;

[assembly: ExportRenderer(typeof(ZoomScrollView), typeof(ZoomableScrollView.iOS.ZoomScrollViewRenderer))]

namespace ZoomableScrollView.iOS
{
    public class ZoomScrollViewRenderer : ScrollViewRenderer
    {
        private bool _ignoreZeroContentOffset;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            var element = e.NewElement as ZoomScrollView;
            if (element != null)
            {
                MaximumZoomScale = (nfloat)element.MaxZoom;
                MinimumZoomScale = (nfloat)element.MinZoom;
                MultipleTouchEnabled = true;
                ShowsVerticalScrollIndicator =
                    ShowsHorizontalScrollIndicator = false;
                if (element.ScrollX != 0 || element.ScrollY != 0 || element.CurrentZoom != 1)
                {
                    //UpdateMinimumZoom(element.IgnoreMinimumZoom);
                    //HACK: it did not work if I did not BeginInvoke here
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //Save the values before we change anything
                        double scrollX = element.ScrollX, scrollY = element.ScrollY;
                        ZoomScale = (float)element.CurrentZoom;
                        ContentOffset = new CGPoint(scrollX, scrollY);
                    });
                }
                ViewForZoomingInScrollView = v => v.Subviews[0];
                DidZoom += (sender, args) =>
                {
                    element.CurrentZoom = ZoomScale;
                    Controller.SetScrolledPosition(ContentOffset.X, ContentOffset.Y);
                };
                element.PropertyChanged += OnPropertyChanged;
            }
            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnPropertyChanged;
            }
        }
        public override bool TouchesShouldCancelInContentView(UIView view)
        {
            //NOTE: We need zooming to be allowed when you happen to tap a UIButton. Overriding this method, fixes our case.
            return true;
        }
        public override CGPoint ContentOffset
        {
            get { return base.ContentOffset; }
            set
            {
                if (_ignoreZeroContentOffset)
                {
                    if (value.IsEmpty)
                    {
                        return;
                    }
                }
                base.ContentOffset = value;
            }
        }
        private void UpdateMinimumZoom(bool ignore = false)
        {
            if (ignore)
                return;
            var contentSize = ContentSize;
            var size = new CGSize(Element.Width, Element.Height);
            nfloat widthRatio = size.Width / contentSize.Width;
            nfloat heightRatio = size.Height / contentSize.Height;
            MinimumZoomScale = NMath.Min(1, NMath.Min(widthRatio, heightRatio));

        }
        private void UpdateContentSize(double zoom)
        {
            var contentSize = ContentSize;
            ContentOffset = new CGPoint((contentSize.Width) / 2, (contentSize.Height) / 2);
            ZoomScale = (float)zoom;
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var element = Element as ZoomScrollView;
            if (element == null)
                return;
            if (e.PropertyName == ZoomScrollView.ContentSizeProperty.PropertyName)
            {
                //UpdateMinimumZoom(element.IgnoreMinimumZoom);
                UpdateContentSize(element.CurrentZoom);
                //HACK: something keeps setting ContentOffset back to zero, this was the only way I could stop it
                _ignoreZeroContentOffset = true;
                BeginInvokeOnMainThread(() => _ignoreZeroContentOffset = false);
            }
        }
    }
}