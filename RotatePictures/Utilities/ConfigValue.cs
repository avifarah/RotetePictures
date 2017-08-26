using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
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

		public string[] InitialPictureDirectories()
		{
			const string key = "Initial Folders";
			string[] defaultFolder = { @"G:\Pictures" };
			var rawConfig = ReadConfigValue(key);
			if (rawConfig == null)
			{
				Log.Error($"Configuration value for \"{key}\" is missing.  Returning default value of: \"{defaultFolder}\"");
				return defaultFolder;
			}
			return rawConfig.Split(';');
		}

		public int MaxPictureTrackerDepth()
		{
			const string key = "Max picture tracker depth";
			const int defaultDepth = 1000;
			var raw = ReadConfigValue(key);
			if (raw == null) return defaultDepth;
			var rc = int.TryParse(raw, out int depth);
			return rc ? depth : defaultDepth;
		}

		public List<string> FileExtensionsToConsider()
		{
			const string key1 = "Still pictures";
			var defaultExtensions = new List<string> { ".jpg", ".bmp", ".gif", ".png", ".psd", ".tif", ".3gp", ".avi", ".mov", ".mpg", ".mod", ".mp4" };
			var raw1 = ReadConfigValue(key1);

			const string key2 = "Motion pictures";
			var raw2 = ReadConfigValue(key2);

			List<string> extensions = null;
			if (raw1 != null) extensions = raw1.Split(';').ToList();
			if (raw2 != null)
			{
				if (extensions == null) extensions = raw2.Split(';').ToList();
				else extensions.AddRange(raw2.Split(';').ToList());
			}

			return extensions ?? defaultExtensions;
		}

		public List<string> StillPictureExtensions()
		{
			const string key = "Still pictures";
			var defExt = new List<string> { ".jpg", ".bmp", ".gif", ".png", ".psd", ".tif" };
			var raw = ReadConfigValue(key);
			if (raw == null) return defExt;
			return raw.Split(';').ToList();
		}

		public List<string> MotionPictures()
		{
			const string key = "Motion pictures";
			var defExt = new List<string> { ".avi", ".mov", ".mpg", ".mod", ".mp4", ".3gp" };
			var raw = ReadConfigValue(key);
			if (raw == null) return defExt;
			var motionPics = raw.Split(';').ToList();
			return motionPics.Except(StillPictureExtensions(), (x, y) => string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0, x => x.ToLower().GetHashCode()).ToList();
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

		/// <summary>
		/// Output in milliseconds
		/// </summary>
		/// <returns></returns>
		public int IntervalBetweenPictures()
		{
			const string key = "Timespan between pictures [Seconds]";
			const int defInterval = 10_000;
			var raw = ReadConfigValue(key);
			if (raw == null) return defInterval;
			var rc = double.TryParse(raw, out double dblIntervalSec);
			return rc ? (int)(dblIntervalSec * 1000) : defInterval;
		}

		public string FirstPictureToDisplay()
		{
			const string key = "First picture to display";
			var raw = ReadConfigValue(key);
			return raw;
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
