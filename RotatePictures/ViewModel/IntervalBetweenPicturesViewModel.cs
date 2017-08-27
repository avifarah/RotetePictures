using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RotatePictures.Annotations;
using RotatePictures.InnerVmCommunication;
using RotatePictures.Utilities;


namespace RotatePictures.ViewModel
{
	public sealed class IntervalBetweenPicturesViewModel : INotifyPropertyChanged
	{
		public IntervalBetweenPicturesViewModel()
		{
			LoadCommands();
			RegisterMessages();
		}

		private float _setIntervalBeweenPictures;

		public float SetIntervalBetweenPictures
		{
			get => _setIntervalBeweenPictures;
			set
			{
				_setIntervalBeweenPictures = value;
				OnPropertyChanged();
			}
		}

		#region RegisterMessages

		private void RegisterMessages() => Messenger.DefaultMessenger.Register<SelectedIntervalMessage>(this, OnSelectedIntervalBetweenPictures);

		private void OnSelectedIntervalBetweenPictures(SelectedIntervalMessage selectedInterval) => SetIntervalBetweenPictures = selectedInterval.SelectedInterval;

		#endregion

		#region Command

		private void LoadCommands()
		{
			CancelCommand = new CustomCommand(CancelAct);
			OkCommand = new CustomCommand(OkAct);
		}

		public ICommand CancelCommand { get; set; }

		public ICommand OkCommand { get; set; }

		private void CancelAct(object obj) => Messenger.DefaultMessenger.Send(new CloseIntervalBetweenPictureMessage(), 3);

		private void OkAct(object obj)
		{
			Messenger.DefaultMessenger.Send(new SetIntervalMessage(SetIntervalBetweenPictures), 2);
			CancelAct(null);
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		#endregion
	}
}
