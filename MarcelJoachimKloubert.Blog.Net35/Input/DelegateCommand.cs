// s. http://blog.marcel-kloubert.de


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
        #region Constructors (3)

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

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        protected DelegateCommand()
        {

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
            protected set;
        }

        /// <summary>
        /// Gibt die Logik der <see cref="DelegateCommand{TParam}.Execute(TParam)" />
        /// methode zurück.
        /// </summary>
        public Action<TParam> ExecuteHandler
        {
            get;
            protected set;
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
        /// <returns>
        /// Ereignis wurde ausgeführt (<see langword="true" />)
        /// oder nicht (<see langword="false" />).
        /// </returns>
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
        // Protected Methods (1) 

        /// <summary>
        /// Die Standardlogik für die <see cref="DelegateCommand{TParam}.CanExecute(TParam)" />
        /// Methode.
        /// </summary>
        /// <param name="parameter">Der Parameter.</param>
        /// <returns>Liefert immer <see langword="true" />.</returns>
        protected static bool DefaultCanExecute(TParam parameter)
        {
            return true;
        }
        // Private Methods (2) 

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
        #region Constructors (4)

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

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="execute">
        /// Die Logik für die <see cref="DelegateCommand{TParam}.CanExecutePredicate" /> Eigenschaft.
        /// </param>
        /// <param name="canExecute">
        /// Die Logik für die <see cref="DelegateCommand{TParam}.ExecuteHandler" /> Eigenschaft.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="execute" /> und/oder<paramref name="canExecute" /> sind
        /// <see langword="null" /> Referenzen.
        /// </exception>
        public DelegateCommand(Action execute,
                               Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.ExecuteHandler = (p) => execute();
            this.CanExecutePredicate = (p) => canExecute();
        }

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="execute">
        /// Die Logik für die <see cref="DelegateCommand{TParam}.CanExecutePredicate" /> Eigenschaft.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="execute" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public DelegateCommand(Action execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.ExecuteHandler = (p) => execute();
            this.CanExecutePredicate = DefaultCanExecute;
        }

        #endregion Constructors
    }

    #endregion
}
