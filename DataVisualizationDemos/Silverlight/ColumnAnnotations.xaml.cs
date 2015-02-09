using System.Collections.Generic;
using System.Windows.Controls;

namespace DataVisualizationDemos
{
    public partial class ColumnAnnotations : UserControl
    {
        public ColumnAnnotations()
        {
            InitializeComponent();

            var items = new List<KeyValuePair<string, double>>();
            items.Add(new KeyValuePair<string, double>("Apples", 0));
            items.Add(new KeyValuePair<string, double>("Oranges", 0.1));
            items.Add(new KeyValuePair<string, double>("Pears", 1));
            DataContext = items;
        }
    }
}
