using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MenuMontaditosApp.Helpers
{
    public class RelayCommand : ICommand
    {
        // Guardamos el método que se ejecutará cuando pulsen el botón
        private readonly Action<object> _execute;

        // Guardamos el método que decide si el botón está activo o no
        // puede ser null si el botón siempre está activo
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // WPF llama a este método para saber si el botón debe estar activo
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute(parameter);
        }

        // WPF llama a este método cuando el usuario pulsa el botón
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        // Este evento le dice a WPF "oye, vuelve a comprobar si el botón está activo"
        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
