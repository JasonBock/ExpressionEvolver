using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace DataVisualizationDemos
{
    public partial class JellyCharting : UserControl
    {
        private Random _rand = new Random();
        private Storyboard _showColumnChart;
        private Storyboard _showLineChart;

        public JellyCharting()
        {
            // Initialize
            InitializeComponent();
            _showColumnChart = (Storyboard)(LayoutRoot.Resources["ShowColumnChart"]);
            _showLineChart = (Storyboard)(LayoutRoot.Resources["ShowLineChart"]);
            GenerateNewData();
            _showColumnChart.Completed += new EventHandler(SwapCharts);
            _showLineChart.Completed += new EventHandler(SwapCharts);

            // Skip to starting state
            _showLineChart.Begin();
            _showLineChart.SkipToFill();
        }

        void SwapCharts(object sender, EventArgs e)
        {
            // Bring new thumbnail to top
            var newThumbnail = (Chart)(ChartContainer.Children[0]);
            ChartContainer.Children.RemoveAt(0);
            ChartContainer.Children.Add(newThumbnail);

            // Tweak border for visibility when shrunk
            newThumbnail.BorderThickness = new Thickness(5);
        }

        private void GenerateNewData()
        {
            // Create new data starting in the middle
            var count = 25;
            var items = new Collection<Point>();
            var last = 5.0;
            for (var i = 0; i < count; i++)
            {
                // Each new point is near the previous point
                var min = Math.Max(1, last - 1);
                var max = Math.Min(9, last + 1);

                // And within the chart area
                var rand = last + ((_rand.NextDouble() * (max - min)) - (last - min));

                // Add it and move on
                items.Add(new Point((double)i / (count - 1), rand));
                last = rand;
            }

            // Display collection
            DataContext = items;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Referenced by XAML.")]
        private void MoreJelly(object sender, RoutedEventArgs e)
        {
            GenerateNewData();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Referenced by XAML.")]
        private void ShowColumnChartClick(object sender, MouseButtonEventArgs e)
        {
            // If column chart is thumbnailed...
            if (ColumnChart != ChartContainer.Children[0])
            {
                // Animate it forward
                ColumnChart.BorderThickness = new Thickness(1);
                _showColumnChart.Begin();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Referenced by XAML.")]
        private void ShowLineChartClick(object sender, MouseButtonEventArgs e)
        {
            // If line chart is thumbnailed...
            if (LineChart != ChartContainer.Children[0])
            {
                // Animate it forward
                LineChart.BorderThickness = new Thickness(1);
                _showLineChart.Begin();
            }
        }
    }

    // IValueConverter implementation that creates a "jelly" effect for showing chart data
    public class JellyConverter : IValueConverter
    {
        // Converts an ICollection of Points to an ICollection of animated JellyPoints
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Nothing to do for null input
            if (null == value)
            {
                return null;
            }

            // Type-check input
            var originalPoints = value as ICollection<Point>;
            if (null == originalPoints)
            {
                throw new NotImplementedException("JellyConverter only supports value type ICollection<T>.");
            }

            // Fixed paramaters (could be set via properties or parameter)
            var duration = TimeSpan.FromSeconds(0.5);
            var delay = TimeSpan.FromSeconds(0.5);
            var ease = new ElasticEase { Oscillations = 1 };

            // Prepare Storyboard
            var count = originalPoints.Count;
            var jellyPoints = new List<JellyPoint>(count);
            var storyboard = new Storyboard();
            var propertyPath = new PropertyPath("Y");
            var i = 0;

            // For each Point...
            foreach (var originalItem in originalPoints)
            {
                // Add a corresponding JellyPoint
                var jellyPoint = new JellyPoint { X = originalItem.X, Y = 0 };
                jellyPoints.Add(jellyPoint);

                // Create an animation
                var animation = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(animation, jellyPoint);
                Storyboard.SetTargetProperty(animation, propertyPath);

                // Configure the initial delay and "jelly" behavior
                var thisDelay = TimeSpan.FromSeconds(delay.TotalSeconds * ((i + 1.0) / count));
                animation.KeyFrames.Add(new LinearDoubleKeyFrame
                {
                    KeyTime = thisDelay,
                    Value = 0
                });
                animation.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = thisDelay + duration,
                    Value = originalItem.Y,
                    EasingFunction = ease
                });

                // Add animation to Storyboard
                animation.Duration = thisDelay + duration;
                storyboard.Children.Add(animation);
                i++;
            }

            // Play the Storyboard
            storyboard.Begin();

            return jellyPoints;
        }

        // Custom Point-like class allows easy animation of Y property
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Intentionally nested to help associate with its parent class.")]
        public class JellyPoint : DependencyObject, INotifyPropertyChanged
        {
            // Static X value
            [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X", Justification = "Mirroring properties of Point class.")]
            public double X { get; set; }

            // Dynamic Y value
            public static readonly DependencyProperty YProperty = DependencyProperty.Register(
                "Y", typeof(double), typeof(JellyPoint), new PropertyMetadata(YPropertyChanged));
            [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Y", Justification = "Mirroring properties of Point class.")]
            public double Y
            {
                get { return (double)GetValue(YProperty); }
                set { SetValue(YProperty, value); }
            }
            private static void YPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
            {
                var jellyPoint = (JellyPoint)o;
                var handler = jellyPoint.PropertyChanged;
                if (null != handler)
                {
                    handler.Invoke(jellyPoint, _yPropertyChangedEventArgs);
                }
            }
            private static PropertyChangedEventArgs _yPropertyChangedEventArgs =
                new PropertyChangedEventArgs("Y");

            // INotifyPropertyChanged event
            public event PropertyChangedEventHandler PropertyChanged;
        }

        // Unimplemented/unnecessary method
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
