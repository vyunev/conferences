using System;
using System.Diagnostics;
using System.Windows.Input;

namespace WAAD.POC.ProductCatalog.UWP.Common
{
    public class Command : ICommand
    {
        private readonly Action _mExecute;
        private readonly Func<bool> _mCanExecute;
        public event EventHandler CanExecuteChanged;

        public Command(Action execute)
            : this(execute, () => true)
        { /* empty */ }

        public Command(Action execute, Func<bool> canexecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _mExecute = execute;
            _mCanExecute = canexecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object p)
        {
            try
            {
                return _mCanExecute == null ? true : _mCanExecute();
            }
            catch
            {
                Debugger.Break();
                return false;
            }
        }

        public void Execute(object p)
        {
            if (CanExecute(p))
                try
                {
                    _mExecute();
                }
                catch { Debugger.Break(); }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Command<T> : ICommand
    {
        private readonly Action<T> _mExecute;
        private readonly Func<T, bool> _mCanExecute;
        public event EventHandler CanExecuteChanged;

        public Command(Action<T> execute)
            : this(execute, (x) => true)
        { /* empty */ }

        public Command(Action<T> execute, Func<T, bool> canexecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _mExecute = execute;
            _mCanExecute = canexecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object p)
        {
            try
            {
                var value = (T)Convert.ChangeType(p, typeof(T));
                return _mCanExecute == null ? true : _mCanExecute(value);
            }
            catch
            {
                Debugger.Break();
                return false;
            }
        }

        public void Execute(object p)
        {
            if (CanExecute(p))
                try
                {
                    var value = (T)Convert.ChangeType(p, typeof(T));
                    _mExecute(value);
                }
                catch { Debugger.Break(); }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
