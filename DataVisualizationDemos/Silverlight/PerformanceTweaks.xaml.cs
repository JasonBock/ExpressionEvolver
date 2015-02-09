using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Input;

namespace DataVisualizationDemos
{
    public partial class PerformanceTweaks : UserControl
    {
        private Random _rand = new Random();

        public PerformanceTweaks()
        {
            InitializeComponent();
            UpdateEnabledStates();
            Loaded += delegate
            {
                UIElement applicationRoot =
#if SILVERLIGHT
                    Application.Current.RootVisual;
#else
                    Application.Current.MainWindow;
#endif
                applicationRoot.AddHandler(KeyDownEvent, new KeyEventHandler(OnKeyDown), true);
                applicationRoot.AddHandler(KeyUpEvent, new KeyEventHandler(OnKeyUp), true);
            };
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
#if SILVERLIGHT
            if (Key.Ctrl == e.Key)
#else
            if ((Key.LeftCtrl == e.Key) || (Key.RightCtrl == e.Key))
#endif
            {
                AddSeries.Visibility = Visibility.Collapsed;
                AddCompatibleSeries.Visibility = Visibility.Visible;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
#if SILVERLIGHT
            if (Key.Ctrl == e.Key)
#else
            if ((Key.LeftCtrl == e.Key) || (Key.RightCtrl == e.Key))
#endif
            {
                AddSeries.Visibility = Visibility.Visible;
                AddCompatibleSeries.Visibility = Visibility.Collapsed;
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            // Reset the application state (go a little overboard just to be thorough)
            foreach (var series in Chart.Series.OfType<DataPointSeries>())
            {
                series.ItemsSource = null;
            }
            Chart.Series.Clear();
            Container.Content = null;
            GC.Collect();
            GC.Collect();
            UpdateEnabledStates();
        }

        private void CreateChart_Click(object sender, RoutedEventArgs e)
        {
            // Create an empty Chart
            Container.Content = new Chart { Title = "Test Chart" };
            UpdateEnabledStates();
        }

        private void AddSeries_Click(object sender, RoutedEventArgs e)
        {
            // Configure a new Series and add it to the Chart
            Control series;
            DependencyProperty itemsSourceProperty;
            DependencyProperty dataPointStyleProperty;
            DependencyProperty transitionDurationProperty;
            DependencyProperty independentAxisProperty;
            DependencyProperty dependentRangeAxisProperty;
            if (sender == AddSeries)
            {
                series = new ScatterSeries { Title = "Standard", IndependentValuePath = "X", DependentValuePath = "Y" };
                itemsSourceProperty = ScatterSeries.ItemsSourceProperty;
                dataPointStyleProperty = ScatterSeries.DataPointStyleProperty;
                transitionDurationProperty = ScatterSeries.TransitionDurationProperty;
                independentAxisProperty = ScatterSeries.IndependentAxisProperty;
                dependentRangeAxisProperty = ScatterSeries.DependentRangeAxisProperty;
            }
            else
            {
                series = new System.Windows.Controls.DataVisualization.Charting.Compatible.ScatterSeries { Title = "Compatible", IndependentValuePath = "X", DependentValuePath = "Y" };
                itemsSourceProperty = System.Windows.Controls.DataVisualization.Charting.Compatible.ScatterSeries.ItemsSourceProperty;
                dataPointStyleProperty = System.Windows.Controls.DataVisualization.Charting.Compatible.ScatterSeries.DataPointStyleProperty;
                transitionDurationProperty = System.Windows.Controls.DataVisualization.Charting.Compatible.ScatterSeries.TransitionDurationProperty;
                independentAxisProperty = System.Windows.Controls.DataVisualization.Charting.Compatible.ScatterSeries.IndependentAxisProperty;
                dependentRangeAxisProperty = System.Windows.Controls.DataVisualization.Charting.Compatible.ScatterSeries.DependentRangeAxisProperty;
            }
            series.SetValue(itemsSourceProperty, EfficientCollection.IsChecked.GetValueOrDefault() ? new AddRangeObservableCollection<DataValue>() : new ObservableCollection<DataValue>());
            if (NoVsm.IsChecked.GetValueOrDefault())
            {
                series.SetValue(dataPointStyleProperty, (Style)Resources["NoVsmStyle"]);
            }
            if (SimplifiedTemplate.IsChecked.GetValueOrDefault())
            {
                series.SetValue(dataPointStyleProperty, (Style)Resources["SimplifiedTemplate"]);
            }
            if (TransitionDuration.IsChecked.GetValueOrDefault())
            {
                series.SetValue(transitionDurationProperty, TimeSpan.Zero);
            }
            if (AxisRanges.IsChecked.GetValueOrDefault())
            {
                series.SetValue(independentAxisProperty, new LinearAxis { Orientation = AxisOrientation.X, Minimum = 0, Maximum = 100 });
                series.SetValue(dependentRangeAxisProperty, new LinearAxis { Orientation = AxisOrientation.Y, Minimum = 0, Maximum = 100, ShowGridLines = true });
            }
            Chart.Series.Add((ISeries)series);
            UpdateEnabledStates();
        }

        private void Populate_Click(object sender, RoutedEventArgs e)
        {
            // Create a set of random values
            var items = new DataValue[(int)PointCount.SelectedItem];
            for (var i = 0; i < items.Length; i++)
            {
                items[i] = new DataValue { X = _rand.Next(100), Y = _rand.Next(100) };
            }
            var addRangeCollection = Collection as AddRangeObservableCollection<DataValue>;
            if (null != addRangeCollection)
            {
                // Use optimized method to add all data at once
                addRangeCollection.AddRange(items);
            }
            else
            {
                // Add values one-by-one
                foreach (var item in items)
                {
                    Collection.Add(item);
                }
            }
            UpdateEnabledStates();
        }

        private void ChangeValues_Click(object sender, RoutedEventArgs e)
        {
            // Randomize every value
            foreach (var dataValue in Collection)
            {
                dataValue.X = _rand.Next(100);
                dataValue.Y = _rand.Next(100);
            }
        }

        private void UpdateEnabledStates()
        {
            // Update the state of all relevant UI elements
            var chartPresent = Container.Content != null;
            var seriesPresent = chartPresent && Chart.Series.Any();
            var seriesPopulated = seriesPresent && Collection.Any();
            Reset.IsEnabled = chartPresent;
            CreateChart.IsEnabled = !chartPresent;
            AddSeries.IsEnabled = chartPresent && !seriesPresent;
            AddCompatibleSeries.IsEnabled = AddSeries.IsEnabled;
            Populate.IsEnabled = chartPresent && seriesPresent && !seriesPopulated;
            ChangeValues.IsEnabled = chartPresent && seriesPresent && seriesPopulated;
            PointCount.IsEnabled = chartPresent && seriesPresent && !seriesPopulated;
            NoVsm.IsEnabled = !seriesPresent;
            SimplifiedTemplate.IsEnabled = !seriesPresent;
            TransitionDuration.IsEnabled = !seriesPresent;
            AxisRanges.IsEnabled = !seriesPresent;
            EfficientCollection.IsEnabled = !seriesPresent;
        }

        private void TemplateChange_Checked(object sender, RoutedEventArgs e)
        {
            // SimplifiedTemplate and NoVsm are mutually exclusive because they both set DataPointStyle
            if (sender == NoVsm)
            {
                SimplifiedTemplate.IsChecked = false;
            }
            else if (sender == SimplifiedTemplate)
            {
                NoVsm.IsChecked = false;
            }
        }

        private Chart Chart
        {
            get { return (Chart)Container.Content; }
        }

        private ISeries Series
        {
            get { return Chart.Series[0]; }
        }

        private ObservableCollection<DataValue> Collection
        {
            get
            {
                var scatterSeries = Series as System.Windows.Controls.DataVisualization.Charting.ScatterSeries;
                if (null != scatterSeries)
                {
                    return (ObservableCollection<DataValue>)(scatterSeries.ItemsSource);
                }
                var compatibleScatterSeries = Series as System.Windows.Controls.DataVisualization.Charting.Compatible.ScatterSeries;
                if (null != compatibleScatterSeries)
                {
                    return (ObservableCollection<DataValue>)(compatibleScatterSeries.ItemsSource);
                }
                throw new NotSupportedException();
            }
        }
    }

    // Custom class for storing data values with change notifications
    public class DataValue : INotifyPropertyChanged
    {
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X")]
        public int X
        {
            get { return _x; }
            set
            {
                _x = value;
                InvokePropertyChanged("X");
            }
        }
        private int _x;

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Y")]
        public int Y
        {
            get { return _y; }
            set
            {
                _y = value;
                InvokePropertyChanged("Y");
            }
        }
        private int _y;

        private void InvokePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    // Custom class adds an efficient AddRange method for adding many items at once
    // without causing a CollectionChanged event for every item
    public class AddRangeObservableCollection<T> : ObservableCollection<T>
    {
        private bool _suppressOnCollectionChanged;

        public void AddRange(IEnumerable<T> items)
        {
            if (null == items)
            {
                throw new ArgumentNullException("items");
            }
            if (items.Any())
            {
                try
                {
                    _suppressOnCollectionChanged = true;
                    foreach (var item in items)
                    {
                        Add(item);
                    }
                }
                finally
                {
                    _suppressOnCollectionChanged = false;
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressOnCollectionChanged)
            {
                base.OnCollectionChanged(e);
            }
        }
    }
}
