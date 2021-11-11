using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Extensions.Controls
{
    public class CircularProgress : Shape
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(CircularProgress), valueMetadata);

        private static readonly FrameworkPropertyMetadata valueMetadata = new(0.0, null, new CoerceValueCallback(CoerceValue));

        static CircularProgress()
        {
            Brush brush = new SolidColorBrush(Color.FromArgb(255, 6, 176, 37));
            brush.Freeze();

            StrokeProperty.OverrideMetadata(typeof(CircularProgress), new FrameworkPropertyMetadata(brush));
            FillProperty.OverrideMetadata(typeof(CircularProgress), new FrameworkPropertyMetadata(brush));
        }

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                const double startAngle = 90.0;
                double endAngle = 90.0 - (Value / 100.0 * 360.0);

                double maxWidth = Math.Max(0.0, RenderSize.Width - StrokeThickness);
                double maxHeight = Math.Max(0.0, RenderSize.Height - StrokeThickness);

                double xStart = maxWidth / 2.0 * Math.Cos(startAngle * Math.PI / 180.0);
                double yStart = maxHeight / 2.0 * Math.Sin(startAngle * Math.PI / 180.0);

                double xEnd = maxWidth / 2.0 * Math.Cos(endAngle * Math.PI / 180.0);
                double yEnd = maxHeight / 2.0 * Math.Sin(endAngle * Math.PI / 180.0);

                StreamGeometry geom = new();
                using (StreamGeometryContext ctx = geom.Open())
                {
                    ctx.BeginFigure(new Point((RenderSize.Width / 2.0) + xStart, (RenderSize.Height / 2.0) - yStart), true, true);
                    ctx.ArcTo(new Point((RenderSize.Width / 2.0) + xEnd, (RenderSize.Height / 2.0) - yEnd), new Size(maxWidth / 2.0, maxHeight / 2), 0.0, (startAngle - endAngle) > 180, SweepDirection.Clockwise, true, true);
                    ctx.LineTo(new Point(RenderSize.Width / 2.0, RenderSize.Height / 2.0), true, false);
                }

                return geom;
            }
        }

        private static object CoerceValue(DependencyObject depObj, object baseVal)
        {
            double val = (double)baseVal;
            val = Math.Min(val, 99.999);
            val = Math.Max(val, 0.0);
            return val;
        }
    }
}