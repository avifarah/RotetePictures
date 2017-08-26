using System.Windows.Forms;
using RotatePictures.InnerVmCommunication;
using RotatePictures.View;
using RotatePictures.Utilities;


namespace RotatePictures.Services
{
	class StretchDialogService : DialogService
	{
		public StretchDialogService() : base(() => new StretchModeView()) { }

		public override void ShowDetailDialog(object param)
		{
			WinDialog = WinCreate();

			var mode = (SelectedStretchMode)param;
			Messenger.DefaultMessenger.Send(new SelectedStretchModeMessage(mode));

			WinDialog?.ShowDialog();
		}
	}
}
