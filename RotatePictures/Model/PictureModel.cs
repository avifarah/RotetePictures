using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using RotatePictures.Utilities;

namespace RotatePictures.Model
{
	public class PictureModel
	{
		private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly PictureCollection _picCollection = new PictureCollection();
		private List<string> _extions;
		private readonly Random _rand = new Random();
		private Task _taskModel;
		private CancellationTokenSource _cts;

		/// <summary>
		/// .ctor
		/// Retrieve pictures asynchrounously
		/// </summary>
		public PictureModel() => Restart();

		public void Restart()
		{
			if (_taskModel != null)
			{
				_cts.Cancel();
				try
				{
					_taskModel.ContinueWith(a => { }).Wait();
					_cts.Dispose();
				}
				catch (AggregateException ae) { Log.Error($"Message: {ae.Flatten()}", ae); }
				catch (Exception e) { Log.Error($"Message: {e.Message}", e); }
			}

			// I have decided not to clear out the SelectionTracker.  The system will still remember old selections
			_picCollection.Clear();
			_extions = ConfigValue.Inst.FileExtensionsToConsider();
			_cts = new CancellationTokenSource();
			_taskModel = Task.Run(() => RetrievePictures(), _cts.Token);
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
				return string.IsNullOrWhiteSpace(pic1) ? null : pic1;
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
