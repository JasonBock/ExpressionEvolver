using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

namespace DataVisualizationDemos
{
    public partial class ChartingIntroductionSL : UserControl
    {
        private ObservableCollection<double> _randomData = new ObservableCollection<double> { 1, 2, 3, 4 };
        private Random _rand = new Random();

        public ChartingIntroductionSL()
        {
            InitializeComponent();

            ((ColumnSeries)EasingDemonstration.Series[0]).ItemsSource = _randomData;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Referenced by XAML.")]
        private void RandomizeData_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _randomData.Count; i++)
            {
                _randomData[i] = (_rand.NextDouble() * 8) + 1;
            }
        }
    }
}
