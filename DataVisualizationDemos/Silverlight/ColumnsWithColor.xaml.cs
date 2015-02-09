using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;

namespace DataVisualizationDemos
{
    public partial class ColumnsWithColor : UserControl
    {
        // Collection of Student data objects
        private ObservableCollection<Student> _students = new ObservableCollection<Student>();
        // Collection of StudentViewModel view model objects
        private ObservableCollection<StudentViewModel> _studentViewModels = new ObservableCollection<StudentViewModel>();
        // Random number generator
        private Random _random = new Random();

        public ColumnsWithColor()
        {
            InitializeComponent();

            // Initialize Student objects
            _students.Add(new Student("Alice", new SolidColorBrush { Color = Colors.Brown }));
            _students.Add(new Student("Edward", new SolidColorBrush { Color = Colors.Purple }));
            _students.Add(new Student("Emmett", new SolidColorBrush { Color = Colors.Orange }));
            _students.Add(new Student("Jasper", new SolidColorBrush { Color = Colors.Gray }));
            _students.Add(new Student("Rosalie", new SolidColorBrush { Color = Colors.Blue }));

            // Create corresponding StudentViewModel objects
            foreach (var student in _students)
            {
                _studentViewModels.Add(new StudentViewModel(student));
            }

            // Assign random grades
            AssignRandomGrades();

            // Assign collections to Chart Series
            (FavoriteColorColumnChart.Series[0] as ColumnSeries).ItemsSource = _students;
            (FavoriteColorPieChart.Series[0] as PieSeries).ItemsSource = _students;
            (GradeColorColumnChart.Series[0] as ColumnSeries).ItemsSource = _studentViewModels;
        }

        // Assign random grades to each Student object
        private void AssignRandomGrades()
        {
            foreach (var student in _students)
            {
                student.Grade = Math.Round((_random.NextDouble() * 70) + 30, 1);
            }
        }

        // Handle clicks on the "Randomize Grades" button
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Referenced by XAML.")]
        private void RandomizeGradesClick(object sender, RoutedEventArgs e)
        {
            AssignRandomGrades();
        }
    }
}
