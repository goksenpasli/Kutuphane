using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Extensions
{
    public partial class GraphControl : Control
    {
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register("Series", typeof(ObservableCollection<Chart>), typeof(GraphControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        static GraphControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphControl), new FrameworkPropertyMetadata(typeof(GraphControl)));
        }

        public ObservableCollection<Chart> Series
        {
            get => (ObservableCollection<Chart>)GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()) && Series?.Any() == true)
            {
                double max = Series.Max(z => z.ChartValue);
                Pen pen = null;
                DrawingGroup graph = null;

                for (int i = 1; i <= Series.Count; i++)
                {
                    Chart item = Series[i - 1];
                    pen = new Pen(item.ChartBrush, ActualWidth / Series.Count);
                    pen.Freeze();

                    graph = new();
                    using (DrawingContext dcgraph = graph.Open())
                    {
                        dcgraph.DrawLine(pen, new Point((pen.Thickness * i) - (pen.Thickness / 2), ActualHeight), new Point((pen.Thickness * i) - (pen.Thickness / 2), ActualHeight - (Series[i - 1].ChartValue / max * ActualHeight)));
                        drawingContext.DrawDrawing(graph);
                    }
                    graph.Freeze();
                }
            }
        }
    }
}