﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RotatePictures.Annotations;
using RotatePictures.InnerVmCommunication;
using RotatePictures.Utilities;


namespace RotatePictures.ViewModel
{
	public sealed class StretchModeViewModel : INotifyPropertyChanged
	{
		private SelectedStretchMode _stretchMode;

		private readonly bool[] _mode;        // 0 - FillRb, 1 - NoneRb, 2 - UniformRb, 3 - UniformToFillRb

		public StretchModeViewModel()
		{
			var modeCount = Enum.GetValues(typeof(SelectedStretchMode)).Length;
			_mode = new bool[modeCount];
			LoadCommands();
			RegisterMessages();
		}

		public bool FillRb
		{
			get => _mode[(int)SelectedStretchMode.Fill];
			set
			{
				_mode[(int)SelectedStretchMode.Fill] = value;
				if (_mode[(int)SelectedStretchMode.Fill]) _stretchMode = SelectedStretchMode.Fill;
				OnPropertyChanged();
			}
		}

		public bool NoneRb
		{
			get => _mode[(int)SelectedStretchMode.None];
			set
			{
				_mode[(int)SelectedStretchMode.None] = value;
				if (_mode[(int)SelectedStretchMode.None]) _stretchMode = SelectedStretchMode.None;
				OnPropertyChanged();
			}
		}

		public bool UniformRb
		{
			get => _mode[(int)SelectedStretchMode.Uniform];
			set
			{
				_mode[(int)SelectedStretchMode.Uniform] = value;
				if (_mode[(int)SelectedStretchMode.Uniform]) _stretchMode = SelectedStretchMode.Uniform;
				OnPropertyChanged();
			}
		}

		public bool UniformToFillRb
		{
			get => _mode[(int)SelectedStretchMode.UniformToFill];
			set
			{
				_mode[(int)SelectedStretchMode.UniformToFill] = value;
				if (_mode[(int)SelectedStretchMode.UniformToFill]) _stretchMode = SelectedStretchMode.UniformToFill;
				OnPropertyChanged();
			}
		}

		#region Register Messages

		private void RegisterMessages() => Messenger.DefaultMessenger.Register<SelectedStretchModeMessage>(this, OnStretchMode);

		private void OnStretchMode(SelectedStretchModeMessage mode)
		{
			switch (mode.Mode)
			{
				case SelectedStretchMode.Fill:
					FillRb = true;
					break;
				case SelectedStretchMode.None:
					NoneRb = true;
					break;
				case SelectedStretchMode.Uniform:
					UniformRb = true;
					break;
				case SelectedStretchMode.UniformToFill:
					UniformToFillRb = true;
					break;
			}
		}

		#endregion

		#region Command

		private void LoadCommands()
		{
			CancelCommand = new CustomCommand(CancelDialog);
			SetStretchMode = new CustomCommand(SaveNewStretchMode);
			SetModeFillCommand = new CustomCommand(SetModeFill);
			SetModeNoneCommand = new CustomCommand(SetModeNone);
			SetModeUniformCommand = new CustomCommand(SetModeUniform);
			SetUniformToFillCommand = new CustomCommand(SetUniformToFill);
		}

		public ICommand SetModeFillCommand { get; set; }

		public ICommand SetModeNoneCommand { get; set; }

		public ICommand SetModeUniformCommand { get; set; }

		public ICommand SetUniformToFillCommand { get; set; }

		public ICommand CancelCommand { get; set; }

		public ICommand SetStretchMode { get; set; }

		private void SetModeUniform(object obj) => UniformRb = true;

		private void CancelDialog(object obj) => Messenger.DefaultMessenger.Send(new CloseStretchModeMessage());

		private void SetModeFill(object obj) => FillRb = true;

		private void SetModeNone(object obj) => NoneRb = true;

		private void SetUniformToFill(object obj) => UniformToFillRb = true;

		private void SaveNewStretchMode(object obj)
		{
			Messenger.DefaultMessenger.Send(new SetStretchModeMessage(_stretchMode), 1);
			CancelDialog(obj);
		}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		#endregion
	}
}
