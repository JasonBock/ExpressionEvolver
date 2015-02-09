using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.DataVisualization.Charting;

namespace DataVisualizationDemos
{
    public partial class DynamicPieGradients : UserControl
    {
        /// <summary>
        /// Initializes an instance of the Page class.
        /// </summary>
        public DynamicPieGradients()
        {
            InitializeComponent();

            // Hook up to Example1's PieSeries
            var pieSeries1 = Example1.Series[0] as PieSeries;
            PieDataPointMappingModeUpdater.UpdatePieSeries(pieSeries1, PieSeries1Updater, true);

            // Hook up to Example2's PieSeries
            var pieSeries2 = Example2.Series[0] as PieSeries;
            PieDataPointMappingModeUpdater.UpdatePieSeries(pieSeries2, PieSeries2Updater, true);
        }

        /// <summary>
        /// Updates the gradients for Example1's PieSeries.
        /// </summary>
        private void PieSeries1Updater(PieDataPoint pieDataPoint, Rect pieBounds)
        {
            var brush = pieDataPoint.Background as LinearGradientBrush;
            if (null != brush)
            {
#if !SILVERLIGHT
                brush = brush.Clone();
#endif
                brush.StartPoint = new Point(pieBounds.Left, pieBounds.Top);
                brush.EndPoint = new Point(pieBounds.Right, pieBounds.Bottom);
#if !SILVERLIGHT
                pieDataPoint.Background = brush;
#endif
            }
        }

        /// <summary>
        /// Updates the gradients for Example2's PieSeries.
        /// </summary>
        private void PieSeries2Updater(PieDataPoint pieDataPoint, Rect pieBounds)
        {
            var brush = pieDataPoint.Background as RadialGradientBrush;
            if (null != brush)
            {
#if !SILVERLIGHT
                brush = brush.Clone();
#endif
                var center = new Point(
                    pieBounds.Left + ((pieBounds.Right - pieBounds.Left) / 2),
                    pieBounds.Top + ((pieBounds.Bottom - pieBounds.Top) / 2));
                brush.Center = center;
                brush.GradientOrigin = center;
                var radius = (pieBounds.Right - pieBounds.Left) / 2;
                brush.RadiusX = radius;
                brush.RadiusY = radius;
#if !SILVERLIGHT
                pieDataPoint.Background = brush;
#endif
            }
        }
    }

    /// <summary>
    /// Class demonstrating one way to apply gradients to an entire pie (vs. each slice separately)
    /// </summary>
    public static class PieDataPointMappingModeUpdater
    {
        /// <summary>
        /// Updates the PieDataPoints of a PieSeries by applying the specified action to each.
        /// </summary>
        /// <param name="pieSeries">PieSeries instance to update.</param>
        /// <param name="updater">Action to run for each PieDataPoint.</param>
        /// <param name="keepUpdated">true to attach to the SizeChanged event of the PieSeries's PlotArea.</param>
        public static void UpdatePieSeries(PieSeries pieSeries, Action<PieDataPoint, Rect> updater, bool keepUpdated)
        {
            // Apply template to ensure visual tree containing PlotArea is created
            pieSeries.ApplyTemplate();
            // Find PieSeries's PlotArea element
            var children = Traverse<FrameworkElement>(
                pieSeries,
                e => VisualTreeChildren(e).OfType<FrameworkElement>(),
                element => null == element as Chart);
            var plotArea = children.OfType<Panel>().Where(e => "PlotArea" == e.Name).FirstOrDefault();
            // If able to find the PlotArea...
            if (null != plotArea)
            {
                // Calculate the diameter of the pie (0.95 multiplier is from PieSeries implementation)
                var diameter = Math.Min(plotArea.ActualWidth, plotArea.ActualHeight) * 0.95;
                // Calculate the bounding rectangle of the pie
                var leftTop = new Point((plotArea.ActualWidth - diameter) / 2, (plotArea.ActualHeight - diameter) / 2);
                var rightBottom = new Point(leftTop.X + diameter, leftTop.Y + diameter);
                var pieBounds = new Rect(leftTop, rightBottom);
                // Call the provided updater action for each PieDataPoint
                foreach (var pieDataPoint in plotArea.Children.OfType<PieDataPoint>())
                {
                    updater(pieDataPoint, pieBounds);
                }
                // If asked to keep the gradients updated, hook up to PlotArea.SizeChanged as well
                if (keepUpdated)
                {
                    plotArea.SizeChanged += delegate
                    {
                        UpdatePieSeries(pieSeries, updater, false);
                    };
                }
            }
        }

        /// <summary>
        /// Traverses a tree by accepting an initial value and a function that retrieves the child nodes of a node.
        /// </summary>
        /// <typeparam name="T">The type of the stream.</typeparam>
        /// <param name="initialNode">The initial node.</param>
        /// <param name="getChildNodes">A function that retrieves the child nodes of a node.</param>
        /// <param name="traversePredicate">A predicate that evaluates a node and returns a value indicating whether that node and it's children should be traversed.</param>
        /// <returns>A stream of nodes.</returns>
        /// <remarks>Source: http://themechanicalbride.blogspot.com/2008/10/implicitstylemanager-under-hood.html</remarks>
        private static IEnumerable<T> Traverse<T>(T initialNode, Func<T, IEnumerable<T>> getChildNodes, Func<T, bool> traversePredicate)
        {
            Stack<T> stack = new Stack<T>();
            stack.Push(initialNode);
            while (stack.Count > 0)
            {
                T node = stack.Pop();
                if (traversePredicate(node))
                {
                    yield return node;
                    IEnumerable<T> childNodes = getChildNodes(node);
                    foreach (T childNode in childNodes)
                    {
                        stack.Push(childNode);
                    }
                }
            }
        }

        /// <summary>
        /// Implementation of getChildNodes parameter to Traverse based on the visual tree.
        /// </summary>
        /// <param name="reference">Object in the visual tree.</param>
        /// <returns>Stream of visual children of the object.</returns>
        private static IEnumerable<DependencyObject> VisualTreeChildren(DependencyObject reference)
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(reference);
            for (var i = 0; i < childrenCount; i++)
            {
                yield return VisualTreeHelper.GetChild(reference, i);
            }
        }
    }
}
