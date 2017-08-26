using System;
using System.Windows.Input;

namespace RotatePictures.Utilities
{
	public class CustomCommand : ICommand
	{
		private readonly Action<object> _execute;
		private readonly Predicate<object> _canExecute;

		public CustomCommand(Action<object> execute, Predicate<object> canExecute = null)
		{
			bool CanExecuteDefault(object _) => true;
			_execute = execute;
			_canExecute = canExecute ?? CanExecuteDefault;
		}

		public bool CanExecute(object parameter) => _canExecute(parameter);

		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public void Execute(object parameter) => _execute(parameter);
	}
}
