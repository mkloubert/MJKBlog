using System;
using System.Windows.Input;

namespace MarcelJoachimKloubert.Blog.Input
{
    #region CLASS: DelegateCommand<TParam>

    /// <summary>
    /// Ein <see cref="ICommand" />, der auf Delegates basiert und typisierte Parameter
    /// nutzt.
    /// </summary>
    /// <typeparam name="TParam"> Typ der Parameter.</typeparam>
    public class DelegateCommand<TParam> : ICommand
    {
        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="execute">
        /// Der Wert für die <see cref="DelegateCommand{TParam}.CanExecutePredicate" /> Eigenschaft.
        /// </param>
        /// <param name="canExecute">
        /// Der Wert für die <see cref="DelegateCommand{TParam}.ExecuteHandler" /> Eigenschaft.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="execute" /> und/oder<paramref name="canExecute" /> sind
        /// <see langword="null" /> Referenzen.
        /// </exception>
        public DelegateCommand(Action<TParam> execute,
                               Func<TParam, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.ExecuteHandler = execute;
            this.CanExecutePredicate = canExecute;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="execute">
        /// Der Wert für die <see cref="DelegateCommand{TParam}.CanExecutePredicate" /> Eigenschaft.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="execute" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public DelegateCommand(Action<TParam> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.ExecuteHandler = execute;
            this.CanExecutePredicate = DefaultCanExecute;
        }

        #endregion Constructors

        #region Properties (2)

        /// <summary>
        /// Gibt die Logik der <see cref="DelegateCommand{TParam}.CanExecute(TParam)" />
        /// methode zurück.
        /// </summary>
        public Func<TParam, bool> CanExecutePredicate
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt die Logik der <see cref="DelegateCommand{TParam}.Execute(TParam)" />
        /// methode zurück.
        /// </summary>
        public Action<TParam> ExecuteHandler
        {
            get;
            private set;
        }

        #endregion Properties

        #region Delegates and Events (2)

        // Events (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ICommand.CanExecuteChanged" />
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Wird aufgerufen, wenn die <see cref="DelegateCommand{TParam}.Execute(TParam)" />
        /// Methode eine <see cref="Exception" /> wirft.
        /// </summary>
        public event DelegateCommandErrorEventHandler<TParam> ExecutionError;

        #endregion Delegates and Events

        #region Methods (6)

        // Public Methods (3) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ICommand.CanExecute(object)" />
        public bool CanExecute(TParam parameter)
        {
            return this.CanExecutePredicate(parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ICommand.Execute(object)" />
        public void Execute(TParam parameter)
        {
            try
            {
                this.ExecuteHandler(parameter);
            }
            catch (Exception ex)
            {
                var handler = this.ExecutionError;
                if (handler != null)
                {
                    handler(this, new DelegateCommandErrorEventArgs<TParam>(ex, parameter));
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Führt, wenn möglich, das <see cref="DelegateCommand{TParam}.CanExecuteChanged" />
        /// Ereignis aus.
        /// </summary>
        /// <returns></returns>
        public bool OnCanExecuteChanged()
        {
            var handler = this.CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
                return true;
            }

            return false;
        }
        // Private Methods (3) 

        private static bool DefaultCanExecute(TParam parameter)
        {
            return true;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute(parameter != null ? (TParam)parameter : default(TParam));
        }

        void ICommand.Execute(object parameter)
        {
            this.Execute(parameter != null ? (TParam)parameter : default(TParam));
        }

        #endregion Methods
    }

    #endregion

    #region CLASS: DelegateCommand

    /// <summary>
    /// Vereinfachte Form von <see cref="DelegateCommand{TParam}" />.
    /// </summary>
    public sealed class DelegateCommand : DelegateCommand<object>
    {
        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="execute">
        /// Der Wert für die <see cref="DelegateCommand{TParam}.CanExecutePredicate" /> Eigenschaft.
        /// </param>
        /// <param name="canExecute">
        /// Der Wert für die <see cref="DelegateCommand{TParam}.ExecuteHandler" /> Eigenschaft.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="execute" /> und/oder<paramref name="canExecute" /> sind
        /// <see langword="null" /> Referenzen.
        /// </exception>
        public DelegateCommand(Action<object> execute,
                               Func<object, bool> canExecute)
            : base(execute, canExecute)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="execute">
        /// Der Wert für die <see cref="DelegateCommand{TParam}.CanExecutePredicate" /> Eigenschaft.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="execute" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public DelegateCommand(Action<object> execute)
            : base(execute)
        {

        }

        #endregion Constructors

    }

    #endregion
}
