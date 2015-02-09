using System.ComponentModel;
using System.Windows.Media;

namespace DataVisualizationDemos
{
    // Standard data object representing a Student
    public class Student : INotifyPropertyChanged
    {
        // Student's name
        public string Name { get; private set; }

        // Student's favorite color
        public Brush FavoriteColor { get; private set; }

        // Student's grade
        public double Grade
        {
            get { return _grade; }
            set
            {
                _grade = value;
                Helpers.InvokePropertyChanged(PropertyChanged, this, "Grade");
            }
        }
        private double _grade;

        // Student constructor
        public Student(string name, Brush favoriteColor)
        {
            Name = name;
            FavoriteColor = favoriteColor;
        }

        // INotifyPropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
