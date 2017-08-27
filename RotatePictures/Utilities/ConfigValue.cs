using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;


namespace RotatePictures.Utilities
{
	public class ConfigValue
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly Lazy<ConfigValue> _inst = new Lazy<ConfigValue>(() => new ConfigValue());
		public static readonly ConfigValue Inst = _inst.Value;

		private readonly Dictionary<string, string> _configValues;

		private ConfigValue() => _configValues = ConfigurationManager.AppSettings.AllKeys.ToDictionary(key => key, key => ConfigurationManager.AppSettings[key]);

		private string[] _initialPictureDirectories;

		public void SetInitialPictureDirectories(string dirs) => _initialPictureDirectories = string.IsNullOrWhiteSpace(dirs) ? InitialPictureDirectories() : dirs.Split(new[] { ';' });

		public string[] InitialPictureDirectories()
		{
			if (_initialPictureDirectories != null) return _initialPictureDirectories;

			const string key = "Initial Folders";
			string[] defaultFolder = { @"G:\Pictures" };
			var rawConfig = ReadConfigValue(key);
			if (rawConfig == null)
			{
				Log.Error($"Configuration value for \"{key}\" is missing.  Returning default value of: \"{defaultFolder}\"");
				return defaultFolder;
			}

			_initialPictureDirectories = rawConfig.Split(';');
			return _initialPictureDirectories;
		}

		private const int DefPictureBufferDepth = 1000;

		private int _maxTrackingDepth = -1;

		public int SetMaxTrackingDepth(int depth) => _maxTrackingDepth = depth <= 0 ? DefPictureBufferDepth : depth;

		public int MaxPictureTrackerDepth()
		{
			if (_maxTrackingDepth > 0) return _maxTrackingDepth;

			const string key = "Max picture tracker depth";
			var raw = ReadConfigValue(key);
			if (raw == null) return DefPictureBufferDepth;
			var rc = int.TryParse(raw, out int depth);
			_maxTrackingDepth = rc ? depth : DefPictureBufferDepth;
			return _maxTrackingDepth;
		}

		public List<string> FileExtensionsToConsider()
		{
			List<string> MotionFromConfig(List<string> e)
			{
				if (_motionExt != null)
				{
					if (e == null)
						return _motionExt;

					var ext = e.Union(_motionExt, (x, y) => string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0,
						x => x.ToLower().GetHashCode()).ToList();
					return ext;
				}

				const string key2 = "Motion pictures";
				var raw2 = ReadConfigValue(key2);
				if (raw2 != null)
					e = e.Union(raw2.Split(';').ToList(), (x, y) => string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0,
						x => x.ToLower().GetHashCode()).ToList();
				return e;
			}

			var defaultExtensions = new List<string> { ".jpg", ".bmp", ".gif", ".png", ".psd", ".tif", ".3gp", ".avi", ".mov", ".mpg", ".mod", ".mp4" };

			List<string> extensions = null;
			if (_stillExt != null)
				extensions = _stillExt;
			else
			{
				const string key1 = "Still pictures";
				var raw1 = ReadConfigValue(key1);
				if (raw1 != null) extensions = raw1.Split(';').ToList();
			}

			extensions = extensions == null ? MotionPictures() : MotionFromConfig(extensions);				
			return extensions ?? defaultExtensions;
		}

		private readonly List<string> _defStillExt = new List<string> { ".jpg", ".bmp", ".gif", ".png", ".psd", ".tif" };

		public string RestoreStillExtensions => string.Join(";", _defStillExt.ToArray());

		private List<string> _stillExt;

		public void SetStillExtension(string stillExt) => _stillExt = stillExt?.Split(new[] { ';' })?.ToList() ?? _defStillExt;

		public List<string> StillPictureExtensions()
		{
			if (_stillExt != null) return _stillExt;

			const string key = "Still pictures";
			var raw = ReadConfigValue(key);
			if (raw == null) return _defStillExt;
			_stillExt = raw.Split(';').ToList();
			return _stillExt;
		}

		private readonly List<string> _defMotionExt = new List<string> { ".mov", ".avi", ".mpg", ".mod", ".mp4", ".wmv", ".3gp" };

		public string RestoreMotionExtensions => string.Join(";", _defMotionExt.ToArray());

		private List<string> _motionExt;

		public void SetMotionExtension(string motionExt)
		{
			_motionExt = motionExt?.Split(new[] { ';' }).ToList() ?? _defMotionExt;
			_motionExt = _motionExt.Except(StillPictureExtensions(),
								(x, y) => string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0,
								x => x.ToLower().GetHashCode()).ToList();
		}

		public List<string> MotionPictures()
		{
			if (_motionExt != null) return _motionExt;

			const string key = "Motion pictures";
			var raw = ReadConfigValue(key);
			if (raw == null) return _defMotionExt;
			var motionPics = raw.Split(';').ToList();
			_motionExt = motionPics.Except(StillPictureExtensions(),
				(x, y) => string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0,
				x => x.ToLower().GetHashCode()).ToList();
			return _motionExt;
		}

		public string ImageStretch()
		{
			const string key = "Image stretch";
			const string defaultStretch = "Uniform";
			var allowdStrechedValues = new List<string> { "Fill", "None", "Uniform", "UniformToFill" };
			var raw = ReadConfigValue(key);
			if (raw == null) return defaultStretch;
			var stretch = allowdStrechedValues.FirstOrDefault(s => string.Compare(s, raw, StringComparison.CurrentCultureIgnoreCase) == 0);
			return stretch ?? defaultStretch;
		}

		private int _intervalBetweenPics = -1;

		public void SetIntervalBetweenPics(int interval) => _intervalBetweenPics = interval > 0 ? interval : IntervalBetweenPictures();

		/// <summary>
		/// Output in milliseconds
		/// </summary>
		/// <returns></returns>
		public int IntervalBetweenPictures()
		{
			if (_intervalBetweenPics > 0) return _intervalBetweenPics;

			const string key = "Timespan between pictures [Seconds]";
			const int defInterval = 10_000;
			var raw = ReadConfigValue(key);
			if (raw == null) return defInterval;
			var rc = double.TryParse(raw, out double dblIntervalSec);
			_intervalBetweenPics = rc ? (int)(dblIntervalSec * 1000) : defInterval;
			return _intervalBetweenPics;
		}

		private string _firstPic;

		public void SetFirstPic(string firstPic) => _firstPic = string.IsNullOrWhiteSpace(firstPic) ? FirstPictureToDisplay() : firstPic;

		public string FirstPictureToDisplay()
		{
			if (!string.IsNullOrWhiteSpace(_firstPic)) return _firstPic;

			const string key = "First picture to display";
			var raw = ReadConfigValue(key);
			_firstPic = raw;
			return _firstPic;
		}

		public bool RotatingPicturesInit()
		{
			const string key = "On start image rotating";
			const bool defRotateInit = true;
			var raw = ReadConfigValue(key);
			if (raw == null) return defRotateInit;
			if (raw.IsTrue()) return true;
			if (raw.IsFalse()) return false;
			Log.Error($"Configuration value for \"{key}\": \"{raw}\", is neither true nor false.  The default value of \"{defRotateInit}\" will be used");
			return defRotateInit;
		}

		private string ReadConfigValue(string key)
		{
			if (!_configValues.ContainsKey(key)) return null;

			return _configValues[key];
		}
	}
}
