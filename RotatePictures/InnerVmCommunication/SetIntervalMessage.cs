

namespace RotatePictures.InnerVmCommunication
{
	public sealed class SetIntervalMessage
	{
		public int SetInterval { get; }

		public SetIntervalMessage(float interval) => SetInterval = (int)(interval * 1000.0F);
	}
}
