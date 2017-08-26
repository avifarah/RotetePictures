

namespace RotatePictures.InnerVmCommunication
{
	public sealed class SelectedIntervalMessage
	{
		public float SelectedInterval { get; }

		public SelectedIntervalMessage(int selectedInterval) => SelectedInterval = (float)selectedInterval / 1000.0F;
	}
}
