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
                
                ViewForZoomingInScrollView = v => v.Subviews[0];
            }
        }

        public override bool TouchesShouldCancelInContentView(UIView view)
        {
            //NOTE: We need zooming to be allowed when you happen to tap a UIButton. Overriding this method, fixes our case.
            return true;
        }
    }
}