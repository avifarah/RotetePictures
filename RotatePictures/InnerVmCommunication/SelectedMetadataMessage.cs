using RotatePictures.Services;


namespace RotatePictures.InnerVmCommunication
{
	public sealed class SelectedMetadataMessage
	{
		private readonly PictureMetaDataTransmission _metadata;

		public SelectedMetadataMessage(PictureMetaDataTransmission metadata) => _metadata = metadata;

		public string PictureFlder => _metadata.PictureFolder;

		public string FirstPictureToDisplay => _metadata.FirstPictureToDisplay;

		public string StillPictureExtensions => _metadata.StillPictureExtensions;

		public string MotionPictureExtensions => _metadata.MotionPictureExtensions;
	}
}
