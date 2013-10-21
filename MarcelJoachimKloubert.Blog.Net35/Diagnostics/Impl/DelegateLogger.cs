// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Ein Logger, die auf Methoden und Funktionen basiert.
    /// </summary>
    public sealed class DelegateLogger : LoggerFacadeBase
    {
        #region Fields (1)

        private readonly List<Action<ILogMessage>> _ACTIONS = new List<Action<ILogMessage>>();

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="DelegateLogger" />.
        /// </summary>
        public DelegateLogger()
            : base(true)
        {

        }

        #endregion Constructors

        #region Methods (5)

        // Public Methods (4) 

        /// <summary>
        /// Fügt Logik der internen Liste hinzu.
        /// </summary>
        /// <param name="action">Die Logik, die hinzugefügt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> ist <see langword="null" />.
        /// </exception>
        public DelegateLogger Add(Action<ILogMessage> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            lock (this._SYNC)
            {
                this._ACTIONS
                    .Add(action);
            }

            return this;
        }

        /// <summary>
        /// Entfernt alle Logiken.
        /// </summary>
        /// <returns>Diese Instanz.</returns>
        public DelegateLogger Clear()
        {
            lock (this._SYNC)
            {
                this._ACTIONS
                    .Clear();
            }

            return this;
        }

        /// <summary>
        /// Gibt eine neue Liste aller Logger-Actions zurück, die Teil dieses Loggers sind.
        /// </summary>
        /// <returns>Die Liste aller Logger-Action dieser Instanz.</returns>
        public List<Action<ILogMessage>> GetActions()
        {
            lock (this._SYNC)
            {
                return new List<Action<ILogMessage>>(this._ACTIONS);
            }
        }

        /// <summary>
        /// Entfernt Logik aus der internen Liste.
        /// </summary>
        /// <param name="action">Die Logik, die entfernt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> ist <see langword="null" />.
        /// </exception>
        public DelegateLogger Remove(Action<ILogMessage> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            lock (this._SYNC)
            {
                this._ACTIONS
                    .Remove(action);
            }

            return this;
        }
        // Protected Methods (1) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="LoggerFacadeBase.OnLog(ILogMessage)" />
        protected override void OnLog(ILogMessage msg)
        {
            foreach (var action in this._ACTIONS)
            {
                try
                {
                    action(msg);
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
