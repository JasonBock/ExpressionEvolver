using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DataVisualizationDemos
{
    public partial class ChartingIntroduction : UserControl
    {
        private CodeElementCollection _codeElementCollection;

        public ChartingIntroduction()
        {
            InitializeComponent();

            _codeElementCollection = LayoutRoot.Resources["CodeElementCollection"] as CodeElementCollection;

#if !NO_EASING_FUNCTIONS
            var chartingIntroductionSL = new ChartingIntroductionSL();
            foreach (var child in chartingIntroductionSL.Container.Children.OfType<UIElement>().ToList())
            {
                chartingIntroductionSL.Container.Children.Remove(child);
                Container.Children.Add(child);
            }
#endif
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Referenced by XAML.")]
        private void AddXamlStatistics_Click(object sender, RoutedEventArgs e)
        {
            _codeElementCollection.Add(new CodeElement { Name = "XAML", Lines = 100 });
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Referenced by XAML.")]
        private void ChangeCommentStatistics_Click(object sender, RoutedEventArgs e)
        {
            CodeElement commentCodeElement = _codeElementCollection.Where(element => element.Name == "Comments").First();
            commentCodeElement.Lines += 50;
        }
    }

    public class WrapPanel : System.Windows.Controls.WrapPanel
    {
    }

    public class ObjectCollection : Collection<object>
    {
    }
}
