using RotatePictures.View;
using RotatePictures.InnerVmCommunication;


namespace RotatePictures.Services
{
	public class IntervalBetweenPicturesDialogService : DialogService
	{
		public IntervalBetweenPicturesDialogService() : base(() => new IntervalBetweenPicturesView()) { }

		public override void ShowDetailDialog(object param)
		{
			WinDialog = WinCreate();

			Messenger.DefaultMessenger.Send(new SelectedIntervalMessage((int)param));

			WinDialog?.ShowDialog();
		}
	}
}
