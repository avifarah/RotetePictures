using System.Windows;
using System.Windows.Controls;


namespace RotatePictures
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			const int duration = 60 * 1000;
			ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(duration));
		}
	}
}
