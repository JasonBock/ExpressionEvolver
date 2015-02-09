using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

namespace DataVisualizationDemos
{
    public partial class StackedSeries : UserControl
    {
        private Chart[] _charts;
        private DefinitionSeries[] _stackedSeries;
        private DefinitionSeries[] _stacked100Series;
        private SeriesDefinition[][] _stackedDefinitions;
        private SeriesDefinition[][] _stacked100Definitions;
        private MultiDataValue[] _data = new MultiDataValue[5];
        private Random _rand = new Random();
        private enum DependentSeriesIdentifier { A = 0, B = 1, C = 2 };

        public StackedSeries()
        {
            InitializeComponent();

            _charts = new Chart[] { StackedColumnChart, StackedBarChart, StackedLineChart, StackedAreaChart };
            _stackedSeries = _charts.Select(c => c.Series.First()).Cast<DefinitionSeries>().ToArray();
            _stacked100Series = _charts.Select(c => c.Series.Last()).Cast<DefinitionSeries>().ToArray();
            _stackedDefinitions = _stackedSeries.Select(ds => ds.SeriesDefinitions.ToArray()).ToArray();
            _stacked100Definitions = _stacked100Series.Select(ds => ds.SeriesDefinitions.ToArray()).ToArray();

            for (var i = 0; i < _data.Length; i++)
            {
                _data[i] = new MultiDataValue();
            }
            DataContext = _data;

            IndependentValues.ItemsSource = new Choice[]
            {
                new Choice { Description = "Strings", Action = () => UpdateChartAllowingAxisChanges(SetIndependentValuesToStrings) },
                new Choice { Description = "Numbers (Consecutive)", Action = () => UpdateChartAllowingAxisChanges(SetIndependentValuesToConsecutiveNumbers) },
                new Choice { Description = "Numbers (Random)", Action = () => UpdateChartAllowingAxisChanges(SetIndependentValuesToRandomNumbers) },
                new Choice { Description = "Dates", Action = () => UpdateChartAllowingAxisChanges(SetIndependentValuesToDates) },
            };
            IndependentValues.SelectionChanged += new SelectionChangedEventHandler(ComboBoxSelectionChanged);
            IndependentValues.SelectedIndex = 0;

            DependentValuesA.ItemsSource = new Choice[]
            {
                new Choice { Description = "Small Positive Integers", Action = () => SetDependentValuesToSmallPositiveIntegers(DependentSeriesIdentifier.A) },
                new Choice { Description = "Large Positive Doubles", Action = () => SetDependentValuesToLargePositiveDoubles(DependentSeriesIdentifier.A) },
                new Choice { Description = "Small Mixed Integers", Action = () => SetDependentValuesToSmallMixedIntegers(DependentSeriesIdentifier.A) },
                new Choice { Description = "Large Mixed Doubles", Action = () => SetDependentValuesToLargeMixedDoubles(DependentSeriesIdentifier.A) },
                new Choice { Description = "Small Negative Integers", Action = () => SetDependentValuesToSmallNegativeIntegers(DependentSeriesIdentifier.A) },
                new Choice { Description = "Large Negative Doubles", Action = () => SetDependentValuesToLargeNegativeDoubles(DependentSeriesIdentifier.A) },
            };
            DependentValuesA.SelectionChanged += new SelectionChangedEventHandler(ComboBoxSelectionChanged);
            DependentValuesA.SelectedIndex = 0;

            DependentValuesB.ItemsSource = new Choice[]
            {
                new Choice { Description = "[Nothing]", Action = () => EnableSeriesDefinition(DependentSeriesIdentifier.B, false) },
                new Choice { Description = "Small Positive Integers", Action = () => SetDependentValuesToSmallPositiveIntegers(DependentSeriesIdentifier.B) },
                new Choice { Description = "Large Positive Doubles", Action = () => SetDependentValuesToLargePositiveDoubles(DependentSeriesIdentifier.B) },
                new Choice { Description = "Small Mixed Integers", Action = () => SetDependentValuesToSmallMixedIntegers(DependentSeriesIdentifier.B) },
                new Choice { Description = "Large Mixed Doubles", Action = () => SetDependentValuesToLargeMixedDoubles(DependentSeriesIdentifier.B) },
                new Choice { Description = "Small Negative Integers", Action = () => SetDependentValuesToSmallNegativeIntegers(DependentSeriesIdentifier.B) },
                new Choice { Description = "Large Negative Doubles", Action = () => SetDependentValuesToLargeNegativeDoubles(DependentSeriesIdentifier.B) },
            };
            DependentValuesB.SelectionChanged += new SelectionChangedEventHandler(ComboBoxSelectionChanged);
            DependentValuesB.SelectedIndex = 1;

            DependentValuesC.ItemsSource = new Choice[]
            {
                new Choice { Description = "[Nothing]", Action = () => EnableSeriesDefinition(DependentSeriesIdentifier.C, false) },
                new Choice { Description = "Small Positive Integers", Action = () => SetDependentValuesToSmallPositiveIntegers(DependentSeriesIdentifier.C) },
                new Choice { Description = "Large Positive Doubles", Action = () => SetDependentValuesToLargePositiveDoubles(DependentSeriesIdentifier.C) },
                new Choice { Description = "Small Mixed Integers", Action = () => SetDependentValuesToSmallMixedIntegers(DependentSeriesIdentifier.C) },
                new Choice { Description = "Large Mixed Doubles", Action = () => SetDependentValuesToLargeMixedDoubles(DependentSeriesIdentifier.C) },
                new Choice { Description = "Small Negative Integers", Action = () => SetDependentValuesToSmallNegativeIntegers(DependentSeriesIdentifier.C) },
                new Choice { Description = "Large Negative Doubles", Action = () => SetDependentValuesToLargeNegativeDoubles(DependentSeriesIdentifier.C) },
            };
            DependentValuesC.SelectionChanged += new SelectionChangedEventHandler(ComboBoxSelectionChanged);
            DependentValuesC.SelectedIndex = 0;

            Stacked100.Checked += new RoutedEventHandler(Stacked100IsCheckedChanged);
            Stacked100.Unchecked += new RoutedEventHandler(Stacked100IsCheckedChanged);
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var choice in e.AddedItems.Cast<Choice>())
            {
                choice.Action();
            }
        }

        void Stacked100IsCheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdateChartAllowingAxisChanges(null);
        }

        private void UpdateChartAllowingAxisChanges(Action action)
        {
            foreach (var chart in _charts)
            {
                chart.Series.Clear();
            }
            if (null != action)
            {
                action();
            }
            for (var i = 0; i < _charts.Length; i++)
            {
                _charts[i].Series.Add(Stacked100.IsChecked.GetValueOrDefault() ? _stacked100Series[i] : _stackedSeries[i]);
            }
        }

        private void SetIndependentValuesToStrings()
        {
            string[] strings = "Apples Bananas Cherries Grapes Oranges".Split();
            for (var i = 0; i < Math.Max(_data.Count(), strings.Length); i++)
            {
                _data[i].IndependentValue = strings[i];
            }
        }

        private void SetIndependentValuesToConsecutiveNumbers()
        {
            for (var i = 0; i < _data.Count(); i++)
            {
                _data[i].IndependentValue = i;
            }
        }

        private void SetIndependentValuesToRandomNumbers()
        {
            for (var i = 0; i < _data.Count(); i++)
            {
                _data[i].IndependentValue = Math.Round(_rand.NextDouble() * 100, 2);
            }
        }

        private void SetIndependentValuesToDates()
        {
            DateTime date = DateTime.Now.Date;
            for (var i = 0; i < _data.Count(); i++)
            {
                _data[i].IndependentValue = date;
                date = date.AddMonths(1);
            }
        }

        private void EnableSeriesDefinition(DependentSeriesIdentifier identifier, bool enable)
        {
            for (var i = 0; i < Math.Max(_stackedDefinitions.Length, _stacked100Definitions.Length); i++)
            {
                var stackedDefinition = _stackedDefinitions[i][(int)identifier];
                if (enable && !_stackedSeries[i].SeriesDefinitions.Contains(stackedDefinition))
                {
                    _stackedSeries[i].SeriesDefinitions.Add(stackedDefinition);
                }
                else if (!enable && _stackedSeries[i].SeriesDefinitions.Contains(stackedDefinition))
                {
                    _stackedSeries[i].SeriesDefinitions.Remove(stackedDefinition);
                }
                var stacked100Definition = _stacked100Definitions[i][(int)identifier];
                if (enable && !_stacked100Series[i].SeriesDefinitions.Contains(stacked100Definition))
                {
                    _stacked100Series[i].SeriesDefinitions.Add(stacked100Definition);
                }
                else if (!enable && _stacked100Series[i].SeriesDefinitions.Contains(stacked100Definition))
                {
                    _stacked100Series[i].SeriesDefinitions.Remove(stacked100Definition);
                }
            }
        }

        private void SetDependentValuesToSmallPositiveIntegers(DependentSeriesIdentifier identifier)
        {
            SetDependentValuesToFunctionResult(identifier, () => _rand.Next(1, 10));
        }

        private void SetDependentValuesToSmallMixedIntegers(DependentSeriesIdentifier identifier)
        {
            SetDependentValuesToFunctionResult(identifier, () => _rand.Next(-10, 10));
        }

        private void SetDependentValuesToSmallNegativeIntegers(DependentSeriesIdentifier identifier)
        {
            SetDependentValuesToFunctionResult(identifier, () => 0 - _rand.Next(1, 10));
        }

        private void SetDependentValuesToLargePositiveDoubles(DependentSeriesIdentifier identifier)
        {
            SetDependentValuesToFunctionResult(identifier, () => _rand.NextDouble() * 40);
        }

        private void SetDependentValuesToLargeMixedDoubles(DependentSeriesIdentifier identifier)
        {
            SetDependentValuesToFunctionResult(identifier, () => 40 - (_rand.NextDouble() * 80));
        }

        private void SetDependentValuesToLargeNegativeDoubles(DependentSeriesIdentifier identifier)
        {
            SetDependentValuesToFunctionResult(identifier, () => 0 - (_rand.NextDouble() * 40));
        }

        private void SetDependentValuesToFunctionResult(DependentSeriesIdentifier identifier, Func<object> getValue)
        {
            for (var i = 0; i < _data.Count(); i++)
            {
                var value = getValue();
                switch (identifier)
                {
                    case DependentSeriesIdentifier.A:
                        _data[i].DependentValueA = value;
                        break;
                    case DependentSeriesIdentifier.B:
                        _data[i].DependentValueB = value;
                        break;
                    case DependentSeriesIdentifier.C:
                        _data[i].DependentValueC = value;
                        break;
                }
            }
            EnableSeriesDefinition(identifier, true);
        }
    }

    // Custom class for storing multiple data values with change notifications
    public class MultiDataValue : INotifyPropertyChanged
    {
        public object IndependentValue
        {
            get { return _independentValue; }
            set
            {
                _independentValue = value;
                InvokePropertyChanged("IndependentValue");
            }
        }
        private object _independentValue;

        public object DependentValueA
        {
            get { return _dependentValueA; }
            set
            {
                _dependentValueA = value;
                InvokePropertyChanged("DependentValueA");
            }
        }
        private object _dependentValueA;

        public object DependentValueB
        {
            get { return _dependentValueB; }
            set
            {
                _dependentValueB = value;
                InvokePropertyChanged("DependentValueB");
            }
        }
        private object _dependentValueB;

        public object DependentValueC
        {
            get { return _dependentValueC; }
            set
            {
                _dependentValueC = value;
                InvokePropertyChanged("DependentValueC");
            }
        }
        private object _dependentValueC;

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

    public class Choice
    {
        public string Description { get; set; }
        public Action Action { get; set; }
    }
}
