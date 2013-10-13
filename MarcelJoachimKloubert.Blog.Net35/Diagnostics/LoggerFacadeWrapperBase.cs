using System;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    #region CLASS: LoggerFacadeWrapperBase<L>

    /// <summary>
    /// Eine <see cref="ILoggerFacade" />, die als Wrapper für einen anderen Logger dient
    /// </summary>
    /// <typeparam name="L">Typ des zugrundeliegenden Loggers.</typeparam>
    public abstract class LoggerFacadeWrapperBase<L> : LoggerFacadeBase
        where L : global::MarcelJoachimKloubert.Blog.Diagnostics.ILoggerFacade
    {
        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="LoggerFacadeBase" />.
        /// </summary>
        /// <param name="innerLogger">
        /// Der Wert für die <see cref="LoggerFacadeWrapperBase{L}.InnerLogger" /> Eigenschaft.
        /// </param>
        /// <param name="isThreadSafe">Log-Vorgänge Thread-sicher ausführen oder nicht.</param>
        protected LoggerFacadeWrapperBase(L innerLogger,
                                          bool isThreadSafe)
            : base(isThreadSafe)
        {
            if (innerLogger == null)
            {
                throw new ArgumentNullException("innerLogger");
            }

            this.InnerLogger = innerLogger;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="LoggerFacadeWrapperBase{L}" />.
        /// </summary>
        /// <param name="innerLogger">
        /// Der Wert für die <see cref="LoggerFacadeWrapperBase{L}.InnerLogger" /> Eigenschaft.
        /// </param>
        /// <remarks>Log-Vorgänge werden Thread-safe ausgeführt.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerLogger" /> ist <see langword="null" />.
        /// </exception>
        protected LoggerFacadeWrapperBase(L innerLogger)
            : this(innerLogger,
                   true)
        {

        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gibt den zugrundeliegenden Logger zurück.
        /// </summary>
        public L InnerLogger
        {
            get;
            private set;
        }

        #endregion Properties
    }

    #endregion

    #region CLASS: LoggerFacadeWrapperBase

    /// <summary>
    /// Einfache <see cref="ILoggerFacade" />, die als Wrapper für einen anderen Logger dient.
    /// </summary>
    public abstract class LoggerFacadeWrapperBase : LoggerFacadeWrapperBase<ILoggerFacade>
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="LoggerFacadeWrapperBase{L}" />.
        /// </summary>
        /// <param name="innerLogger">
        /// Der Wert für die <see cref="LoggerFacadeWrapperBase{L}.InnerLogger" /> Eigenschaft.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerLogger" /> ist <see langword="null" />.
        /// </exception>
        protected LoggerFacadeWrapperBase(ILoggerFacade innerLogger)
            : base(innerLogger)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="LoggerFacadeWrapperBase{L}" />.
        /// </summary>
        /// <param name="innerLogger">
        /// Der Wert für die <see cref="LoggerFacadeWrapperBase{L}.InnerLogger" /> Eigenschaft.
        /// </param>
        /// <param name="isThreadSafe">Log-Vorgänge Thread-sicher ausführen oder nicht.</param>
        protected LoggerFacadeWrapperBase(ILoggerFacade innerLogger,
                                          bool isThreadSafe)
            : base(innerLogger,
                   isThreadSafe)
        {

        }

        #endregion Constructors
    }

    #endregion
}
