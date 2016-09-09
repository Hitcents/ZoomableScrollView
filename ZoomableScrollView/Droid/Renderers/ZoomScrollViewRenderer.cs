using System;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZoomableScrollView;
using ZoomableScrollView.Droid;

[assembly: ExportRenderer(typeof(ZoomScrollView), typeof(ZoomScrollViewRenderer))]

namespace ZoomableScrollView.Droid
{
    public class ZoomScrollViewRenderer : ScrollViewRenderer
    {
        private ScaleGestureDetector _scaleDetector;
        private bool _isScaleProcess = false;
        private float _prevScale = 1f;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                _scaleDetector = new ScaleGestureDetector(Context, new ClearScaleListener(scale =>
                {
                    _isScaleProcess = true;
                    var scrollView = Element as ZoomScrollView;
                    var horScrollView = GetChildAt(0) as global::Android.Widget.HorizontalScrollView;
                    var content = horScrollView.GetChildAt(0);
                    _prevScale = Math.Max((float)scrollView.MinimumZoom, Math.Min(_prevScale * scale, (float)scrollView.MaximumZoom));
                    content.ScaleX = content.ScaleY = _prevScale;
                    System.Diagnostics.Debug.WriteLine($"Delta: {scale}  Final: {content.ScaleX}");
                }));
            }
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            if (e.PointerCount == 2)
            {
                return _scaleDetector.OnTouchEvent(e);
            }
            else if (_isScaleProcess)
            {
                //HACK:
                //Prevent letting any touch events from moving the scroll view until all fingers are up from zooming...This prevents the jumping and skipping around after user zooms.
                if(e.Action == MotionEventActions.Up)
                    _isScaleProcess = false;
                return false;
            }
            else
                return base.OnTouchEvent(e);
        }
    }

    public class ClearScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener
    {
        private Action<float> _onScale;
        private bool _skip = false;

        public ClearScaleListener(Action<float> onScale)
        {
            _onScale = onScale;
        }

        public override bool OnScale(ScaleGestureDetector detector)
        {
            if (_skip)
            {
                _skip = false;
                return true;
            }
            _onScale?.Invoke(detector.ScaleFactor);
            return true;
        }

        public override bool OnScaleBegin(ScaleGestureDetector detector)
        {
            System.Diagnostics.Debug.WriteLine($"Begin {detector.ScaleFactor}");
            _skip = true;
            return base.OnScaleBegin(detector);
        }
    }
}