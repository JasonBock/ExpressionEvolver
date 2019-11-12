using System.Windows;

namespace ExpressionEvolver.Client.Windows
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
			this.DataContext = new MainWindowViewModel();
		}

		private async void OnEvolveClick(object sender, RoutedEventArgs e)
		{
			await (this.DataContext as MainWindowViewModel).EvolveAsync();
		}
	}
}
