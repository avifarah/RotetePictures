using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RotatePictures.Utilities;

namespace RotatePictures.Model
{
	public class PictureModel
	{
		private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly PictureCollection _picCollection = new PictureCollection();
		private readonly List<string> _extions;
		private readonly Random _rand = new Random();

		/// <summary>
		/// .ctor
		/// Retrieve pictures asynchrounously
		/// </summary>
		public PictureModel()
		{
			_extions = ConfigValue.Inst.FileExtensionsToConsider();
			Task.Run(() => RetrievePictures());
		}

		/// <summary>
		/// Retrieve next picture randomly
		/// </summary>
		/// <returns></returns>
		public string GetNextPicture()
		{
			var cnt = _picCollection.Count;
			if (cnt == 0)
			{
				var pic1 = ConfigValue.Inst.FirstPictureToDisplay();
				if (string.IsNullOrWhiteSpace(pic1)) return null;
				return pic1;
			}

			var index = _rand.Next(cnt);
			return _picCollection[index];
		}

		/// <summary>
		/// Retrieve all pictures from all directories
		/// </summary>
		private void RetrievePictures()
		{
			var dirs = ConfigValue.Inst.InitialPictureDirectories();
			foreach (var dir in dirs)
				RetrievePictures(dir);
		}

		/// <summary>
		/// Retrieve pictures in a specific directory
		/// </summary>
		/// <param name="dir"></param>
		private void RetrievePictures(string dir)
		{
			var files = Directory.GetFiles(dir);
			var rightFiles = files.Where(fl => _extions.Any(e => fl.EndsWith(e, StringComparison.CurrentCultureIgnoreCase)));

			foreach (var f in rightFiles)
				_picCollection.Add(f);

			var dirs = Directory.GetDirectories(dir);
			foreach (var d in dirs)
				RetrievePictures(d);
		}
	}
}
