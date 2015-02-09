using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;

namespace DataVisualizationDemos
{
    public partial class TextToChart : UserControl
    {
        private readonly char[] SplitTokens = " \t;".ToCharArray();

        public TextToChart()
        {
            InitializeComponent();
            InputText.Text = "1 3\n2 4\n4 5\n";
            InputText.TextChanged += new TextChangedEventHandler(InputText_TextChanged);
            SeriesType.ItemsSource = new TypeStringTuple[]
            {
                new TypeStringTuple(typeof(StackedBarSeries), "Stacked Bar Series"),
                new TypeStringTuple(typeof(StackedColumnSeries), "Stacked Column Series"),
                new TypeStringTuple(typeof(StackedLineSeries), "Stacked Line Series"),
                new TypeStringTuple(typeof(StackedAreaSeries), "Stacked Area Series"),
                new TypeStringTuple(typeof(Stacked100BarSeries), "100% Stacked Bar Series"),
                new TypeStringTuple(typeof(Stacked100ColumnSeries), "100% Stacked Column Series"),
                new TypeStringTuple(typeof(Stacked100LineSeries), "100% Stacked Line Series"),
                new TypeStringTuple(typeof(Stacked100AreaSeries), "100% Stacked Area Series"),
            };
            SeriesType.SelectedIndex = 1;
            SeriesType.SelectionChanged += new SelectionChangedEventHandler(SeriesType_SelectionChanged);
            ConvertTextToChart();
        }

        void SeriesType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConvertTextToChart();
        }

        private void InputText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConvertTextToChart();
        }

        private void ConvertTextToChart()
        {
            OutputChart.Series.Clear();
            var palette = OutputChart.Palette;
            OutputChart.Palette = null;
            OutputChart.Palette = palette;
            InputText.ClearValue(TextBox.ForegroundProperty);

            var dataValues = new List<List<SimpleDataValue>>();
            try
            {
                using (var reader = new StringReader(InputText.Text))
                {
                    int independentValue = 1;
                    string line;
                    while (null != (line = reader.ReadLine()))
                    {
                        var parts = line.Trim().Split(SplitTokens);
                        if (0 == dataValues.Count)
                        {
                            for (int i = 0; i < parts.Length; i++)
                            {
                                dataValues.Add(new List<SimpleDataValue>());
                            }
                        }
                        else if (parts.Length != dataValues.Count)
                        {
                            throw new Exception("Missing expected data value.");
                        }
                        for (int i = 0; i < parts.Length; i++)
                        {
                            var dependentValue = double.Parse(parts[i]);
                            dataValues[i].Add(new SimpleDataValue { DependentValue = dependentValue, IndependentValue = independentValue });
                        }
                        independentValue++;
                    }
                }
            }
            catch
            {
                dataValues.Clear();
                InputText.Foreground = new SolidColorBrush(Colors.Red);
            }

            var stackedSeries = Activator.CreateInstance(((TypeStringTuple)SeriesType.SelectedItem).Item1) as DefinitionSeries;
            foreach (var values in dataValues)
            {
                var definition = new SeriesDefinition();
                definition.DependentValuePath = "DependentValue";
                definition.IndependentValuePath = "IndependentValue";
                definition.ItemsSource = values;
                stackedSeries.SeriesDefinitions.Add(definition);
            }
            OutputChart.Series.Add(stackedSeries);
        }
    }

    public class SimpleDataValue
    {
        public double DependentValue { get; set; }
        public double IndependentValue { get; set; }
    }

    public class TypeStringTuple
    {
        public Type Item1 { get; set; }
        public string Item2 { get; set; }
        public TypeStringTuple(Type item1, string item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
}
