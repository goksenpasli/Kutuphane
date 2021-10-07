﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Extensions
{
    public class RelayAsyncCommand<T> : RelayCommand<T>
    {
        private bool isExecuting;

        public RelayAsyncCommand(Action<T> execute, Predicate<T> canExecute)
            : base(execute, canExecute)
        {
        }

        public RelayAsyncCommand(Action<T> execute)
            : base(execute)
        {
        }

        public event EventHandler Ended;

        public event EventHandler Started;

        public bool IsExecuting
        {
            get { return isExecuting; }
        }

        public override bool CanExecute(object parameter) => base.CanExecute(parameter) && (!isExecuting);

        public override void Execute(object parameter)
        {
            try
            {
                isExecuting = true;
                Started?.Invoke(this, EventArgs.Empty);

                var task = Task.Factory.StartNew(() => _execute((T)parameter));
                _ = task.ContinueWith(_ => OnRunWorkerCompleted(EventArgs.Empty), TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, ex, true));
            }
        }

        private void OnRunWorkerCompleted(EventArgs e)
        {
            isExecuting = false;
            Ended?.Invoke(this, e);
        }
    }

    public class RelayCommand<T> : ICommand
    {
        #region Fields

        protected readonly Predicate<T> _canExecute;

        protected readonly Action<T> _execute;

        #endregion Fields

        #region Constructors

        public RelayCommand(Action<T> execute)
        : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion Constructors

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        [DebuggerStepThrough]
        public virtual bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);

        public virtual void Execute(object parameter) => _execute((T)parameter);

        #endregion ICommand Members
    }

    public class RelayCommand : ICommand
    {
        protected readonly Func<bool> canExecute;

        protected readonly Action execute;

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public virtual bool CanExecute(object parameter) => canExecute == null || canExecute();

        public virtual void Execute(object parameter) => execute();
    }
}