using RotatePictures.Utilities;


namespace RotatePictures.InnerVmCommunication
{
	public class SelectedStretchModeMessage
	{
		public SelectedStretchMode Mode { get; }

		public SelectedStretchModeMessage(SelectedStretchMode mode) => Mode = mode;
	}
}
