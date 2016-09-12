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
            scrollView.ContentSize = imageView.Frame.Size;
            scrollView.DidZoom += (sender, e) =>
            {
                CenterContent();
            };
        }

        private void CenterContent()
        {
            nfloat top = 0, left = 0;

            if (scrollView.ContentSize.Width < scrollView.Bounds.Size.Width)
            {
                left = (scrollView.Bounds.Size.Width - scrollView.ContentSize.Width) * 0.5f;
            }
            if (scrollView.ContentSize.Height < scrollView.Bounds.Size.Height)
            {
                top = (scrollView.Bounds.Size.Height - scrollView.ContentSize.Height) * 0.5f;
            }
            scrollView.ContentInset = new UIEdgeInsets(top, left, top, left);
        }
    }
}

