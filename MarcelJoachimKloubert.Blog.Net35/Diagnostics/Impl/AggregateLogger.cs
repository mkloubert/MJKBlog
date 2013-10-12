// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Führt mehere Logger nacheinander bzw. gleichzeitig aus.
    /// </summary>
    public sealed class AggregateLogger : LoggerFacadeBase
    {
        #region Fields (1)

        private readonly List<ILoggerFacade> _LOGGERS = new List<ILoggerFacade>();

        #endregion Fields

        #region Methods (4)

        // Public Methods (3) 

        /// <summary>
        /// Fügt Logik der internen Liste hinzu.
        /// </summary>
        /// <param name="logger">Die Logik, die hinzugefügt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="logger" /> ist diese Instanz.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> ist <see langword="null" />.
        /// </exception>
        public AggregateLogger Add(ILoggerFacade logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (object.ReferenceEquals(this, logger))
            {
                throw new ArgumentException("logger");
            }

            this._LOGGERS.Add(logger);
            return this;
        }

        /// <summary>
        /// Entfernt alle Logiken.
        /// </summary>
        /// <returns>Diese Instanz.</returns>
        public AggregateLogger Clear()
        {
            this._LOGGERS.Clear();
            return this;
        }

        /// <summary>
        /// Entfernt Logik aus der internen Liste.
        /// </summary>
        /// <param name="logger">Die Logik, die entfernt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> ist <see langword="null" />.
        /// </exception>
        public AggregateLogger Remove(ILoggerFacade logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this._LOGGERS.Remove(logger);
            return this;
        }
        // Protected Methods (1) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="LoggerFacadeBase.OnLog(ILogMessage)" />
        protected override void OnLog(ILogMessage msg)
        {
            foreach (var logger in this._LOGGERS)
            {
                try
                {
                    logger.Log(msg);
                }
                catch
                {
                    // Fehler ignorieren
                }
            }
        }

        #endregion Methods
    }
}
