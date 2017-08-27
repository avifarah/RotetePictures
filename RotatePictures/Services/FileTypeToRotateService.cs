using RotatePictures.InnerVmCommunication;
using RotatePictures.View;


namespace RotatePictures.Services
{
	public class FileTypeToRotateService : DialogService
	{
		public FileTypeToRotateService() : base(() => new FileTypesToRotate()) { }

		#region Overrides of DialogService

		public override void ShowDetailDialog(object param)
		{
			WinDialog = WinCreate();

			var metadata = param as PictureMetaDataTransmission;
			Messenger.DefaultMessenger.Send(new SelectedMetadataMessage(metadata));

			WinDialog?.ShowDialog();
		}

		#endregion
	}
}
