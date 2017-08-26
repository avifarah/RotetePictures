using System;
using System.Windows;


namespace RotatePictures.Services
{
	public abstract class DialogService
	{
		protected readonly Func<Window> WinCreate;

		protected Window WinDialog;

		public DialogService(Func<Window> windowCreate) => WinCreate = windowCreate ?? throw new ArgumentException(@"Window creation lambda cannot be null", nameof(windowCreate));

		public virtual void ShowDetailDialog()
		{
			WinDialog = WinCreate();
			WinDialog?.ShowDialog();
		}

		public virtual void ShowDetailDialog(object param)
		{
			WinDialog = WinCreate();
			WinDialog?.ShowDialog();
		}

		public virtual void CloseDetailDialog() => WinDialog?.Close();
	}
}
