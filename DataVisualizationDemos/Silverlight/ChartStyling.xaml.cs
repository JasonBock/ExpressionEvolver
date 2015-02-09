using System.Windows.Controls;

namespace DataVisualizationDemos
{
    public partial class ChartStyling : UserControl
    {
        public ChartStyling()
        {
            InitializeComponent();

            DataContext = new[] { "this", "is", "sample", "data" };
        }
    }
}
