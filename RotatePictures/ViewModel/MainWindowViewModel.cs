using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Windows.Input;
using System.IO;
using System.Threading.Tasks;
using RotatePictures.Model;
using RotatePictures.Utilities;
using RotatePictures.Services;
using RotatePictures.InnerVmCommunication;


namespace RotatePictures.ViewModel
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly System.Timers.Timer _tmr;
		private readonly PictureModel _model;

		private readonly StretchDialogService _strechSvc;
		private readonly IntervalBetweenPicturesDialogService _intervalBetweenPicturesService;
		private readonly FileTypeToRotateService _pictureMetadataService;

		public MainWindowViewModel()
		{
			_model = new PictureModel();

			_strechSvc = new StretchDialogService();
			_intervalBetweenPicturesService = new IntervalBetweenPicturesDialogService();
			_pictureMetadataService = new FileTypeToRotateService();

			_pic = ConfigValue.Inst.FirstPictureToDisplay();
			_imgStretch = ConfigValue.Inst.ImageStretch();
			_rotationRunning = ConfigValue.Inst.RotatingPicturesInit();

			_tmr = new System.Timers.Timer { Interval = IntervalBetweenPictures, Enabled = RotationRunning };
			_tmr.Elapsed += ChangePic;

			SelectionTracker.Inst.Append(_pic);

			if (_pic == null && !File.Exists(_pic))
			{
				bool succeeded = false;
				for (var i = 0; i < 100 && !succeeded; ++i)
				{
					var t = Task.Delay(10);
					t.Wait();
					try
					{
						RetrieveNextPicture();
						succeeded = true;
					}
					catch { succeeded = false; }
				}
			}

			LoadCommands();
			RegisterMessages();
		}

		private string _pic;

		public string CurrentPicture
		{
			get => _pic;
			set
			{
				_pic = value;
				//Log.Info($"New picture path: {_pic}");
				OnPropertyChanged();
			}
		}

		private string _imgStretch;

		public string ImageStretch
		{
			get => _imgStretch;
			set
			{
				_imgStretch = value;
				OnPropertyChanged();
			}
		}

		private bool _rotationRunning;

		public bool RotationRunning
		{
			get => _rotationRunning;
			set
			{
				_rotationRunning = value;
				_tmr.Enabled = _rotationRunning;
				OnPropertyChanged();
			}
		}

		private int _intervalBetweenPictures = ConfigValue.Inst.IntervalBetweenPictures();

		public int IntervalBetweenPictures
		{
			get => _intervalBetweenPictures;
			set
			{
				_intervalBetweenPictures = value;
				_tmr.Interval = _intervalBetweenPictures;
				OnPropertyChanged();
			}
		}

		private void ChangePic(object sender, System.Timers.ElapsedEventArgs e) => RetrieveNextPicture();

		private void RetrieveNextPicture()
		{
			var pic = _model.GetNextPicture();
			if (pic == null) return;
			CurrentPicture = pic;
			SelectionTracker.Inst.Append(_pic);
		}

		#region Inner VM Communication Message Handling

		private void RegisterMessages()
		{
			Messenger.DefaultMessenger.Register<CloseStretchModeMessage>(this, OnCloseStretchMode);
			Messenger.DefaultMessenger.Register<SetStretchModeMessage>(this, OnSetStretchMode, 1);
			Messenger.DefaultMessenger.Register<SetIntervalMessage>(this, OnSetIntervalBetweenPictures, 2);
			Messenger.DefaultMessenger.Register<CloseIntervalBetweenPictureMessage>(this, OnCloseIntervalBetweenPictures, 3);
			Messenger.DefaultMessenger.Register<CancelFileTypesMessage>(this, OnCancelFileTypes, 4);
			Messenger.DefaultMessenger.Register<SelectedMetadataMessage>(this, OnSetMetadataAction, 5);
		}

		private void OnCloseIntervalBetweenPictures(CloseIntervalBetweenPictureMessage obj) => _intervalBetweenPicturesService.CloseDetailDialog();

		private void OnSetIntervalBetweenPictures(SetIntervalMessage intervalMsg)
		{
			var interval = intervalMsg.SetInterval;
			IntervalBetweenPictures = interval;

			const string key = "Timespan between pictures [Seconds]";
			UpdateConfigFile.Inst.UpdateConfig(key, (IntervalBetweenPictures / 1000.0F).ToString(CultureInfo.CurrentCulture));
		}

		private void OnCloseStretchMode(CloseStretchModeMessage obj) => _strechSvc.CloseDetailDialog();

		private void OnSetStretchMode(SetStretchModeMessage stretchMode)
		{
			ImageStretch = stretchMode.SelectedStretch.ToString();

			const string key = "Image stretch";
			UpdateConfigFile.Inst.UpdateConfig(key, ImageStretch);
		}

		private void OnCancelFileTypes(CancelFileTypesMessage obj) => _pictureMetadataService.CloseDetailDialog();

		private void OnSetMetadataAction(SelectedMetadataMessage metadata)
		{
			const string pictureFolderKey = "Initial Folders";
			var pictureFolder = metadata.PictureFlder;
			if (!string.IsNullOrWhiteSpace(pictureFolder))
			{
				UpdateConfigFile.Inst.UpdateConfig(pictureFolderKey, pictureFolder);
				ConfigValue.Inst.SetInitialPictureDirectories(pictureFolder);
			}

			const string firstPictureKey = "First picture to display";
			var firstPicture = metadata.FirstPictureToDisplay;
			if (!string.IsNullOrWhiteSpace(firstPicture))
				UpdateConfigFile.Inst.UpdateConfig(firstPictureKey, firstPicture);

			const string stillExtKey = "Still pictures";
			var stillExt = metadata.StillPictureExtensions;
			if (!string.IsNullOrWhiteSpace(stillExt))
				UpdateConfigFile.Inst.UpdateConfig(stillExtKey, stillExt);

			const string motionExtKey = "Motion pictures";
			var motionExt = metadata.MotionPictureExtensions;
			if (!string.IsNullOrWhiteSpace(motionExt))
				UpdateConfigFile.Inst.UpdateConfig(motionExtKey, motionExt);

			// Restart the system
			ResetPictures();
		}

		private void ResetPictures()
		{
			_tmr.Stop();
			_model.Restart();
			_tmr.Start();
		}

		#endregion

		#region Command

		private void LoadCommands()
		{
			StopStartCommand = new CustomCommand(StopStartRotation);
			BackImageCommand = new CustomCommand(BackImageMove);
			NextImageCommand = new CustomCommand(NextImageMove);
			SetTimeBetweenPicturesCommand = new CustomCommand(SetTimeBetweenPictures);
			SetSelectedStrechModeCommand = new CustomCommand(SetSelectedStrechMode);
			SetPicturesMetaDataCommand = new CustomCommand(SetPicturesMetaData);
		}

		public ICommand StopStartCommand { get; set; }

		public ICommand BackImageCommand { get; set;  }

		public ICommand NextImageCommand { get; set;  }

		public ICommand SetTimeBetweenPicturesCommand { get; set; }

		public ICommand SetSelectedStrechModeCommand { get; set; } 

		public ICommand SetPicturesMetaDataCommand { get; set; }

		private void StopStartRotation(object obj) => RotationRunning = !RotationRunning;

		public void BackImageMove(object obj)
		{
			CurrentPicture = SelectionTracker.Inst.Prev();

			if (!RotationRunning) return;

			_tmr.Stop();
			_tmr.Start();
		}

		public void NextImageMove(object obj)
		{
			if (SelectionTracker.Inst.AtTail)
				RetrieveNextPicture();
			else
				CurrentPicture = SelectionTracker.Inst.Next();

			if (!RotationRunning) return;

			_tmr.Stop();
			_tmr.Start();
		}

		private void SetTimeBetweenPictures(object obj) => _intervalBetweenPicturesService.ShowDetailDialog(IntervalBetweenPictures);

		private void SetSelectedStrechMode(object obj)
		{
			var mode = ImageStretch.TextToMode();
			_strechSvc.ShowDetailDialog(mode);
		}

		private void SetPicturesMetaData(object obj)
		{
			var picFolder = string.Join(";", ConfigValue.Inst.InitialPictureDirectories());
			var firstPicture = ConfigValue.Inst.FirstPictureToDisplay();
			var stillExtentions = string.Join(";", ConfigValue.Inst.StillPictureExtensions());
			var motionExtentions = string.Join(";", ConfigValue.Inst.MotionPictures());
			var _metaData = new PictureMetaDataTransmission {
				PictureFolder = picFolder,
				FirstPictureToDisplay = firstPicture,
				StillPictureExtensions = stillExtentions,
				MotionPictureExtensions = motionExtentions
			};
			_pictureMetadataService.ShowDetailDialog(_metaData);
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

		#endregion
	}
}

