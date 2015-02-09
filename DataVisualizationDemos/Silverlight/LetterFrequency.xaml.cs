using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Input;

namespace DataVisualizationDemos
{
    public partial class LetterFrequency : UserControl
    {
        ObservableCollection<LetterData> _letterDatas = new ObservableCollection<LetterData>();

        public LetterFrequency()
        {
            InitializeComponent();

            foreach (var chart in ChartGrid.Children.OfType<Chart>())
            {
                chart.DataContext = _letterDatas;
                chart.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs args)
                {
                    var c = (Chart)sender;
                    c.DataContext = (null == c.DataContext) ? _letterDatas : null;
                };
            }

            TextBox_TextChanged(Text, null);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var index = 0;
            var groupings = textBox.Text.ToCharArray().Where(c => char.IsLetter(c)).Select(c => char.ToLower(c, CultureInfo.InvariantCulture)).GroupBy(c => c).OrderBy(g => g.Key);
            for (var c = 'a'; c <= 'z'; c++)
            {
                var grouping = groupings.Where(g => c == g.Key).FirstOrDefault();
                if (null == grouping)
                {
                    var letterData = _letterDatas.Where(d => c == d.Character).FirstOrDefault();
                    if (null != letterData)
                    {
                        _letterDatas.Remove(letterData);
                    }
                }
                else
                {
                    var letterData = _letterDatas.Where(d => c == d.Character).FirstOrDefault();
                    if (null == letterData)
                    {
                        letterData = new LetterData { Character = c };
                        _letterDatas.Insert(index, letterData);
                    }
                    letterData.Count = grouping.Count();
                    index++;
                }
            }
        }
    }

    public class LetterData : INotifyPropertyChanged
    {
        public char Character { get; set; }
        public int Ordinal { get { return Character; } }
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                var handler = PropertyChanged;
                if (null != handler)
                {
                    handler.Invoke(this, new PropertyChangedEventArgs("Count"));
                }
            }
        }
        private int _count;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
