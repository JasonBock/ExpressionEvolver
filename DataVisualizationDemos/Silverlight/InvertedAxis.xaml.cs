using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace DataVisualizationDemos
{
    public partial class InvertedAxis : UserControl
    {
        public InvertedAxis()
        {
            InitializeComponent();

            var items = new List<DataItem>
            {
                new DataItem(new DateTime(2009, 4, 1), 10),
                new DataItem(new DateTime(2009, 4, 8),  5),
                new DataItem(new DateTime(2009, 4, 15), 2),
                new DataItem(new DateTime(2009, 4, 22), 1),
                new DataItem(new DateTime(2009, 4, 29), 1),
            };

            DataContext = items;
        }
    }

    public class InverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return -(int)value;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DataItem
    {
        public DateTime Date { get; private set; }
        public int Place { get; private set; }
        public DataItem(DateTime date, int place)
        {
            Date = date;
            Place = place;
        }
    }
}
