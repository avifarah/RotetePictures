using RotatePictures.Utilities;


namespace RotatePictures.InnerVmCommunication
{
	public class SetStretchModeMessage
	{
		public SelectedStretchMode SelectedStretch { get; }

		public SetStretchModeMessage(SelectedStretchMode selectedStretch) => SelectedStretch = selectedStretch;
	}
}
