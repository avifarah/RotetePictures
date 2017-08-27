using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RotatePictures.Annotations;
using RotatePictures.InnerVmCommunication;
using RotatePictures.Services;
using RotatePictures.Utilities;


namespace RotatePictures.ViewModel
{
	public class FileTypesToRotateViewModel : INotifyPropertyChanged
	{
		public FileTypesToRotateViewModel() => Messenger.DefaultMessenger.Register<SelectedMetadataMessage>(this, OnMetaDataProcess);

		private void OnMetaDataProcess(SelectedMetadataMessage metadata)
		{
			if (metadata == null)
			{
				PictureFolders = string.Join(";", ConfigValue.Inst.InitialPictureDirectories());
				FirstPictureToDisplay = ConfigValue.Inst.FirstPictureToDisplay();
				StillPictureExtensions = string.Join(";", ConfigValue.Inst.StillPictureExtensions().ToArray());
				MotionPictureExtensions = string.Join(";", ConfigValue.Inst.MotionPictures().ToArray());
				return;
			}

			PictureFolders = metadata.PictureFlder;
			FirstPictureToDisplay = metadata.FirstPictureToDisplay;
			StillPictureExtensions = metadata.StillPictureExtensions;
			MotionPictureExtensions = metadata.MotionPictureExtensions;

			LoadCommands();
		}

		private string _pictureFolders;

		public string PictureFolders
		{
			get => _pictureFolders;
			set
			{
				_pictureFolders = value;
				OnPropertyChanged();
			}
		}

		private string _firstPictureToDisplayToDisplay;

		public string FirstPictureToDisplay
		{
			get => _firstPictureToDisplayToDisplay;
			set
			{
				_firstPictureToDisplayToDisplay = value;
				OnPropertyChanged();
			}
		}

		private string _stillPictureExtensions;

		public string StillPictureExtensions
		{
			get => _stillPictureExtensions;
			set
			{
				_stillPictureExtensions = value;
				OnPropertyChanged();
			}
		}

		private string _motionPictureExtensions;

		public string MotionPictureExtensions
		{
			get => _motionPictureExtensions;
			set
			{
				_motionPictureExtensions = value;
				OnPropertyChanged();
			}
		}

		#region Command

		private void LoadCommands()
		{
			CancelCommand = new CustomCommand(CancelAction);
			OkCommand = new CustomCommand(OkAction);
			RestoreStillExtCommand = new CustomCommand(RestoreStillExts);
			RestoreMotionExtCommand = new CustomCommand(RestoreMotionExt);
		}

		public ICommand CancelCommand { get; set; }

		public ICommand OkCommand { get; set; }

		public ICommand RestoreStillExtCommand { get; set; }

		public ICommand RestoreMotionExtCommand { get; set; }

		private void CancelAction(object obj) => Messenger.DefaultMessenger.Send(new CancelFileTypesMessage(), 4);

		private void RestoreStillExts(object obj) => StillPictureExtensions = ConfigValue.Inst.RestoreStillExtensions;

		private void RestoreMotionExt(object obj) => MotionPictureExtensions = ConfigValue.Inst.RestoreMotionExtensions;

		private void OkAction(object obj)
		{
			var metadata = new PictureMetaDataTransmission {
				PictureFolder = PictureFolders,
				FirstPictureToDisplay = FirstPictureToDisplay,
				StillPictureExtensions = StillPictureExtensions,
				MotionPictureExtensions = MotionPictureExtensions
			};
			Messenger.DefaultMessenger.Send(new SelectedMetadataMessage(metadata), 5);
			CancelAction(null);
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
