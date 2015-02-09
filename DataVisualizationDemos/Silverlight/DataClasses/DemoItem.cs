using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataVisualizationDemos
{
    public class DemoItem
    {
        public string Title { get; set; }
        public UserControl Control { get; set; }
        public Brush Background { get; set; }
        public IEnumerable<Uri> Posts { get; set; }
    }
}
