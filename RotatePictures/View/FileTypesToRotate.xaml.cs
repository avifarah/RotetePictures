using System.Windows;
using RotatePictures.ViewModel;


namespace RotatePictures.View
{
	/// <summary>
	/// Interaction logic for FileTypesToRotate.xaml
	/// </summary>
	public partial class FileTypesToRotate : Window
	{
		public FileTypesToRotate()
		{
			InitializeComponent();

			WindowStartupLocation = WindowStartupLocation.CenterOwner;
			Owner = Application.Current.MainWindow;

			DataContext = new FileTypesToRotateViewModel();
		}
	}
}
