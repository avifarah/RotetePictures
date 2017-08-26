using System.Reflection;
using System.Windows;
using RotatePictures.ViewModel;


namespace RotatePictures.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public MainWindow()
		{
			log4net.Config.XmlConfigurator.Configure();

			InitializeComponent();
			DataContext = new MainWindowViewModel();

			//Log.Info(string.Empty);
			//Log.Info(string.Empty);
		}

		private void BackImageMove(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) => (DataContext as MainWindowViewModel)?.BackImageMove(e);

		private void NextImageMove(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) => (DataContext as MainWindowViewModel)?.NextImageMove(e);
	}
}
