using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace ZoomableScrollView
{
    public class PanContainer : ContentView
    {
        private double _x, _y, _currentScale = 1;
        
        public EventHandler PanCompleted;

        public PanContainer()
        {
            GestureRecognizers.Add(GetPan());
            GestureRecognizers.Add(GetPinch());
        }

        private PanGestureRecognizer GetPan()
        {
            var pan = new PanGestureRecognizer();
            pan.PanUpdated += (s, e) =>
            {
                switch (e.StatusType)
                {
                    case GestureStatus.Started:
                        Content.TranslationX = _x;
                        Content.TranslationY = _y;
                        break;
                    case GestureStatus.Running:
                        //needs to not let you pan outside the bounds of the container.
                        Content.TranslationX = _x + e.TotalX;
                        Content.TranslationY = _y + e.TotalY;
                        break;
                    case GestureStatus.Completed:
                        _x = Content.TranslationX;
                        _y = Content.TranslationY;
                        PanCompleted?.Invoke(s, EventArgs.Empty);
                        break;
                }
            };
            return pan;
        }

        private PinchGestureRecognizer GetPinch()
        {
            var pinch = new PinchGestureRecognizer();

            double xOffset = 0, yOffset = 0, startScale = 1;

            pinch.PinchUpdated += (s, e) =>
            {
                switch (e.Status)
                {
                    case GestureStatus.Started:
                        {
                            startScale = Content.Scale;
                            Content.AnchorX = e.ScaleOrigin.X;
                            Content.AnchorY = e.ScaleOrigin.Y;
                        }
                        break;
                    case GestureStatus.Running:
                        {
                            _currentScale += (e.Scale - 1) * startScale;
                            _currentScale = Math.Max(1, _currentScale);

                            var renderedX = Content.X + xOffset;
                            var deltaX = renderedX / Width;
                            var deltaWidth = Width / (Content.Width * startScale);
                            var originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                            var renderedY = Content.Y + yOffset;
                            var deltaY = renderedY / Height;
                            var deltaHeight = Height / (Content.Height * startScale);
                            var originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                            double targetX = xOffset - (originX * Content.Width) * (_currentScale - startScale);
                            double targetY = yOffset - (originY * Content.Height) * (_currentScale - startScale);

                            //Content.TranslationX = Clamp(targetX, -Content.Width * (_currentScale - 1), 0);
                            //Content.TranslationY = Clamp(targetY, -Content.Height * (_currentScale - 1), 0);

                            Content.Scale = _currentScale;
                        }
                        break;
                    case GestureStatus.Completed:
                        {
                            xOffset = Content.TranslationX;
                            yOffset = Content.TranslationY;
                        }
                        break;
                }
            };

            return pinch;
        }

        private double Clamp(double self, double min, double max)
        {
            return Math.Min(max, Math.Max(self, min));
        }
    }
}
