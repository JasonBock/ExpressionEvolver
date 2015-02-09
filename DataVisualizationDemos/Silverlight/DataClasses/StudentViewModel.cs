using System.ComponentModel;
using System.Windows.Media;

namespace DataVisualizationDemos
{
    // Custom data object to wrap a Student object for the view model
    public class StudentViewModel : INotifyPropertyChanged
    {
        // Student object
        public Student Student { get; private set; }

        // Color representing Student's Grade
        public Brush GradeColor { get; private set; }

        // StudentViewModel constructor
        public StudentViewModel(Student student)
        {
            Student = student;
            student.PropertyChanged += new PropertyChangedEventHandler(HandleStudentPropertyChanged);
        }

        // Detect changes to the Student's grade and update GradeColor
        void HandleStudentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("Grade" == e.PropertyName)
            {
                if (Student.Grade < 50)
                {
                    GradeColor = new SolidColorBrush { Color = Colors.Red };
                }
                else if (Student.Grade < 80)
                {
                    GradeColor = new SolidColorBrush { Color = Colors.Yellow };
                }
                else
                {
                    GradeColor = new SolidColorBrush { Color = Colors.Green };
                }
                Helpers.InvokePropertyChanged(PropertyChanged, this, "GradeColor");
            }
        }

        // INotifyPropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
