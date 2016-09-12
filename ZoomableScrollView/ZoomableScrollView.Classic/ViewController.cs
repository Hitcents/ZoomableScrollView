using System;

using UIKit;

namespace ZoomableScrollView.Classic
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            scrollView.ViewForZoomingInScrollView = (scrollView) => { return imageView; };
            scrollView.DidZoom += (sender, e) =>
            {
                var offSetX = Math.Max((scrollView.Bounds.Size.Width - scrollView.ContentSize.Width) * .5, 0);
                var offSetY = Math.Max((scrollView.Bounds.Size.Height - scrollView.ContentSize.Height) * .5, 0);
                var subView = scrollView.Subviews[0];
                var center = subView.Center;

                center.X = ((nfloat)scrollView.ContentSize.Width * scrollView.ZoomScale) * .5f + (nfloat)offSetX;
                center.Y = ((nfloat)scrollView.ContentSize.Height * scrollView.ZoomScale) * .5f + (nfloat)offSetY;

                subView.Center = center;
            };
        }

        public override void ViewDidLayoutSubviews()
        {
            scrollView.ContentSize = imageView.Frame.Size;
        }
    }
}

