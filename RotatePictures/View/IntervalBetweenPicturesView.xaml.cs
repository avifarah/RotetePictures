using System.Windows;
using System.Windows.Input;
using RotatePictures.Utilities;
using RotatePictures.ViewModel;


namespace RotatePictures.View
{
	/// <summary>
	/// Interaction logic for IntervalBetweenPicturesView.xaml
	/// </summary>
	public partial class IntervalBetweenPicturesView : Window
	{
		public IntervalBetweenPicturesView()
		{
			InitializeComponent();

			WindowStartupLocation = WindowStartupLocation.CenterOwner;
			Owner = Application.Current.MainWindow;

			DataContext = new IntervalBetweenPicturesViewModel();
		}

		private void Interval_OnPreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = !e.Text.IsFloat();
	}
}
