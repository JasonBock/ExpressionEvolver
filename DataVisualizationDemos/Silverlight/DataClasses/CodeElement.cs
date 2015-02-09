using System.ComponentModel;

namespace DataVisualizationDemos
{
    public class CodeElement : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public int Lines
        {
            get { return _lines; }
            set
            {
                _lines = value;
                var handler = PropertyChanged;
                if (null != handler)
                {
                    handler.Invoke(this, new PropertyChangedEventArgs("Lines"));
                }
            }
        }
        private int _lines;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
