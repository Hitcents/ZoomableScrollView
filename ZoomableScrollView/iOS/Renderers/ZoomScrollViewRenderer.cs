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
                MaximumZoomScale = (nfloat)element.MaximumZoom;
                MinimumZoomScale = (nfloat)element.MinimumZoom;
                MultipleTouchEnabled = true;
                ShowsVerticalScrollIndicator =
                    ShowsHorizontalScrollIndicator = false;
                
                ViewForZoomingInScrollView = v => v.Subviews[0];
                DidZoom += (sender, args) =>
                {
                    var offSetX = Math.Max((element.Bounds.Size.Width - element.ContentSize.Width) * .5, 0);
                    var offSetY = Math.Max((element.Bounds.Size.Height - element.ContentSize.Height) * .5, 0);
                    var subView = Subviews[0];
                    var center = subView.Center;

                    center.X = ((nfloat)element.ContentSize.Width * ZoomScale) * .5f + (nfloat)offSetX;
                    center.Y = ((nfloat)element.ContentSize.Height * ZoomScale) * .5f + (nfloat)offSetY;

                    subView.Center = center;
                };
            }
        }

        public override bool TouchesShouldCancelInContentView(UIView view)
        {
            //NOTE: We need zooming to be allowed when you happen to tap a UIButton. Overriding this method, fixes our case.
            return true;
        }
    }
}