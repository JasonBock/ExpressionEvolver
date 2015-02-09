using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace DataVisualizationDemos
{
    public partial class TextLabels : UserControl
    {
        public TextLabels()
        {
            InitializeComponent();

            // Create sample feedback data
            DataContext = new Feedback[]
            {
                new Feedback { Topic = "Food", Rating = 3.2 },
                new Feedback { Topic = "Atmosphere", Rating = 0.9 },
                new Feedback { Topic = "Service", Rating = 1.8 },
                new Feedback { Topic = "Cleanliness", Rating = 2.7 },
            };
        }
    }

    /// <summary>
    /// Implements IValueConverter to convert from double rating values to friendly string names.
    /// </summary>
    public class RatingToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Validate parameters
            if (!(value is double))
            {
                throw new NotSupportedException("Unsupported value type in RatingToStringConverter.");
            }
            // Convert number to string
            double doubleValue = Math.Floor((double)value);
            if (0.0 == doubleValue)
            {
                return "Awful";
            }
            else if (1.0 == doubleValue)
            {
                return "Poor";
            }
            else if (2.0 == doubleValue)
            {
                return "Fair";
            }
            else if (3.0 == doubleValue)
            {
                return "Good";
            }
            else if (4.0 == doubleValue)
            {
                return "Great";
            }
            else
            {
                throw new ArgumentException("Unsupported value in RatingToStringConverter.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Implements a simple model class for feedback.
    /// </summary>
    public class Feedback
    {
        public string Topic { get; set; }
        public double Rating { get; set; }
    }
}
