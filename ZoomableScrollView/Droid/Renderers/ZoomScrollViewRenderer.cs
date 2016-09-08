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
        private bool _isCentered = false;
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
                    //TODO: need to rewrite this stuff to match what iOS is doing

                    _prevScale = Math.Max((float)scrollView.MinZoom, Math.Min(_prevScale * scale, (float)scrollView.MaxZoom));
                    content.ScaleX = content.ScaleY = _prevScale;
                    System.Diagnostics.Debug.WriteLine($"Delta: {scale}  Final: {content.ScaleX}");
                }, () =>
                {
                    System.Diagnostics.Debug.WriteLine("Finished");
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
                //Prevent letting any touch events from moving the scroll view until all fingers are up from zooming...This prevents the jumping and skipping around after user zooms.
                if(e.Action == MotionEventActions.Up)
                    _isScaleProcess = false;
                return false;
            }
            else
                return base.OnTouchEvent(e);
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);
            if (!_isCentered)
            {
                _isCentered = true;
                var horScrollView = GetChildAt(0) as global::Android.Widget.HorizontalScrollView;
                if (horScrollView != null)
                {
                    ScrollTo(0, (horScrollView.Height - Height) / 2);
                    var content = horScrollView.GetChildAt(0);
                    if (content != null)
                    {
                        horScrollView.ScrollTo((content.Width - horScrollView.Width) / 2, 0);
                        content.ScaleX = content.ScaleY = 1;
                    }
                }
            }
        }
    }

    public class ClearScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener
    {
        private Action<float> _onScale;
        private Action _onScaleEnd;
        private bool _skip = false;

        public ClearScaleListener(Action<float> onScale, Action onScaleEnd = null)
        {
            _onScale = onScale;
            _onScaleEnd = onScaleEnd;
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

        public override void OnScaleEnd(ScaleGestureDetector detector)
        {
            _onScaleEnd?.Invoke();
        }

        public override bool OnScaleBegin(ScaleGestureDetector detector)
        {
            System.Diagnostics.Debug.WriteLine($"Begin {detector.ScaleFactor}");
            _skip = true;
            return base.OnScaleBegin(detector);
        }
    }
}