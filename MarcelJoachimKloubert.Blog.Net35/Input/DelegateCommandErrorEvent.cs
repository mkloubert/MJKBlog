using System;
using System.IO;

namespace MarcelJoachimKloubert.Blog.Input
{
    #region DELEGATE: DelegateCommandErrorEventHandler<TParam>

    /// <summary>
    /// Ein Handler für ein Ereignis, das einen Fehler in einem
    /// <see cref="DelegateCommand{TParam}" /> meldet.
    /// </summary>
    /// <typeparam name="TParam">Typ des Parameters des <see cref="DelegateCommand{TParam}" />s.</typeparam>
    /// <param name="sender">Das sendende Objekt.</param>
    /// <param name="e">Die Argumente für das Ereignis.</param>
    public delegate void DelegateCommandErrorEventHandler<TParam>(object sender, DelegateCommandErrorEventArgs<TParam> e);

    #endregion

    #region class DelegateCommandErrorEventArgs<TParam>

    /// <summary>
    /// Argumente für ein Ereignis, das einen Fehler in einem
    /// <see cref="DelegateCommand{TParam}" /> meldet.
    /// </summary>
    /// <typeparam name="TParam">Typ des Parameters des <see cref="DelegateCommand{TParam}" />s.</typeparam>
    public sealed class DelegateCommandErrorEventArgs<TParam> : ErrorEventArgs
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="DelegateCommandErrorEventArgs{TParam}" /> Klasse.
        /// </summary>
        /// <param name="ex">Die zugrundeliegende Exception.</param>
        /// <param name="param">Der zugrundeliegende Parameter.</param>
        public DelegateCommandErrorEventArgs(Exception ex, TParam param)
            : base(ex)
        {
            this.Parameter = param;
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gibt den zugrundeliegenden Parameter zurück.
        /// </summary>
        public TParam Parameter
        {
            get;
            private set;
        }

        #endregion Properties
    }

    #endregion

}
