using System.Windows;
using RotatePictures.ViewModel;


namespace RotatePictures.View
{
	/// <summary>
	/// Interaction logic for StretchModeView.xaml
	/// </summary>
	public partial class StretchModeView : Window
	{
		public StretchModeView()
		{
			InitializeComponent();

			WindowStartupLocation = WindowStartupLocation.CenterOwner;
			Owner = Application.Current.MainWindow;

			DataContext = new StretchModeViewModel();
		}
	}
}
