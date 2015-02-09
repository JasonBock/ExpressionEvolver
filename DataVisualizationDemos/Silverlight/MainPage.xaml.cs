using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
#if SILVERLIGHT
using System.Windows.Browser;
#else
using System.Diagnostics;
#endif

namespace DataVisualizationDemos
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            // Create distinct brushes for the different sample categories
            var chartingBrush = new SolidColorBrush(Color.FromArgb(255, 255, 220, 220));
            var treeMapBrush = new SolidColorBrush(Color.FromArgb(255, 220, 255, 220));

            // Populate the list of samples
            DataContext = new List<DemoItem>
            {
                new DemoItem { Title = "Charting Introduction", Control = new ChartingIntroduction(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2008/10/28/announcing-a-free-open-source-charting-solution-for-silverlight-silverlight-toolkit-released-today-at-pdc.aspx"),
                        new Uri("http://blogs.msdn.com/delay/archive/2008/12/09/silverlight-charting-gets-a-host-of-improvements-silverlight-toolkit-december-08-release-now-available.aspx"),
                        new Uri("http://blogs.msdn.com/delay/archive/2009/03/19/silverlight-charting-is-faster-and-better-than-ever-silverlight-toolkit-march-09-release-now-available.aspx"),
                        new Uri("http://blogs.msdn.com/delay/archive/2009/07/10/silverlight-charting-gets-an-update-and-a-treemap-silverlight-toolkit-july-2009-release-now-available.aspx"),
                        new Uri("http://blogs.msdn.com/delay/archive/2009/10/19/silverlight-and-wpf-data-visualization-classes-unsealed-silverlight-toolkit-october-2009-release-now-available.aspx"),
                        new Uri("http://blogs.msdn.com/delay/archive/2009/11/18/silverlight-4-beta-is-out-and-the-toolkit-has-it-covered-silverlight-toolkit-november-2009-release-now-available-for-silverlight-3-and-4.aspx"),
                        new Uri("http://blogs.msdn.com/delay/archive/2010/04/15/alive-and-kickin-new-silverlight-4-toolkit-released-with-today-s-silverlight-4-rtw.aspx"),
                   } },
                new DemoItem { Title = "Chart Styling", Control =  new ChartStyling(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2009/05/19/chart-tweaking-made-easy-how-to-make-four-simple-color-tooltip-changes-with-silverlight-wpf-charting.aspx"),
                    } },
                new DemoItem { Title = "Columns With Color", Control = new ColumnsWithColor(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2009/02/04/columns-of-a-different-color-customizing-the-appearance-of-silverlight-charts-with-re-templating-and-mvvm.aspx"),
                    } },
                new DemoItem { Title = "Dynamic Pie Gradients", Control = new DynamicPieGradients(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2008/12/30/yummier-pies-a-technique-for-more-flexible-gradient-styling-of-silverlight-toolkit-pie-charts.aspx"),
                    } },
#if !NO_EASING_FUNCTIONS
                new DemoItem { Title = "Jelly Charting", Control = new JellyCharting(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2009/06/15/peanut-butter-jelly-time-how-to-create-a-pleasing-visual-effect-with-silverlight-wpf-charting.aspx"),
                    } },
#endif
                new DemoItem { Title = "Inverted Axis", Control = new InvertedAxis(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2009/05/12/pineapple-upside-down-chart-how-to-invert-the-axis-of-a-chart-for-smaller-is-better-scenarios.aspx"),
                    } },
                new DemoItem { Title = "Letter Frequency", Control = new LetterFrequency(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2009/06/25/wpf-charting-it-s-official-june-2009-release-of-the-wpf-toolkit-is-now-available.aspx"),
                    } },
                new DemoItem { Title = "Column Annotations", Control = new ColumnAnnotations(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2009/07/27/simple-column-labels-you-can-create-at-home-re-templating-the-silverlight-wpf-data-visualization-columndatapoint-to-add-annotations.aspx"),
                    } },
                new DemoItem { Title = "Performance Tweaks", Control = new PerformanceTweaks(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2010/01/13/i-feel-the-need-the-need-for-speed-seven-simple-performance-boosting-tweaks-for-common-silverlight-wpf-charting-scenarios.aspx"),
                        new Uri("http://blogs.msdn.com/delay/archive/2010/04/16/the-one-with-all-the-goofy-heading-names-detailed-information-about-the-silverlight-toolkit-s-new-stacked-series-support.aspx"),
                        new Uri("http://blogs.msdn.com/delay/archive/2010/04/22/nobody-likes-a-show-off-today-s-datavisualizationdemos-release-includes-new-demos-showing-off-stacked-series-behavior.aspx"),
                    } },
                new DemoItem { Title = "Stacked Series", Control = new StackedSeries(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2010/04/16/the-one-with-all-the-goofy-heading-names-detailed-information-about-the-silverlight-toolkit-s-new-stacked-series-support.aspx"),
                        new Uri("http://blogs.msdn.com/delay/archive/2010/04/22/nobody-likes-a-show-off-today-s-datavisualizationdemos-release-includes-new-demos-showing-off-stacked-series-behavior.aspx"),
                    } },
                new DemoItem { Title = "Text-To-Chart", Control = new TextToChart(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2010/04/22/nobody-likes-a-show-off-today-s-datavisualizationdemos-release-includes-new-demos-showing-off-stacked-series-behavior.aspx"),
                    } },
                new DemoItem { Title = "Text Labels", Control = new TextLabels(), Background = chartingBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/b/delay/archive/2010/06/02/please-rate-your-dining-experience-how-to-show-text-labels-on-a-numeric-axis-with-silverlight-wpf-toolkit-charting.aspx"),
                    } },
                new DemoItem { Title = "TreeMap Introduction", Control = new TreeMapIntroduction(), Background = treeMapBrush,
                    Posts = new Uri[]
                    {
                        new Uri("http://blogs.msdn.com/delay/archive/2009/07/10/silverlight-charting-gets-an-update-and-a-treemap-silverlight-toolkit-july-2009-release-now-available.aspx"),
                    } },
            };
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Referenced by XAML.")]
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Swap in a new sample
            SampleContainer.Children.Clear();
            PostList.ItemsSource = null;
            if ((null != e.AddedItems) && (0 < e.AddedItems.Count))
            {
                var demoItem = (DemoItem)e.AddedItems[0];
                SampleContainer.Children.Add(demoItem.Control);
                PostList.ItemsSource = demoItem.Posts;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Referenced by XAML.")]
        private void UriMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Navigate to the specified web page
            var textBlock = (TextBlock)sender;
            var uri = textBlock.Tag as Uri ?? new Uri(textBlock.Tag as string ?? textBlock.Text);
#if SILVERLIGHT
            HtmlPage.Window.Navigate(uri, "_blank");
#else
            Process.Start(uri.ToString());
#endif
        }
    }
}
