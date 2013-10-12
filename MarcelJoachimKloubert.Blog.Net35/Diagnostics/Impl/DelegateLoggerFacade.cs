// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Ein Logger, die auf Methoden und Funktionen basiert.
    /// </summary>
    public sealed class DelegateLoggerFacade : LoggerFacadeBase
    {
        #region Fields (1)

        private readonly List<Action<ILogMessage>> _ACTIONS = new List<Action<ILogMessage>>();

        #endregion Fields

        #region Methods (4)

        // Public Methods (3) 

        /// <summary>
        /// Fügt Logik der internen Liste hinzu.
        /// </summary>
        /// <param name="action">Die Logik, die hinzugefügt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> ist <see langword="null" />.
        /// </exception>
        public DelegateLoggerFacade Add(Action<ILogMessage> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this._ACTIONS.Add(action);
            return this;
        }

        /// <summary>
        /// Entfernt alle Logiken.
        /// </summary>
        /// <returns>Diese Instanz.</returns>
        public DelegateLoggerFacade Clear()
        {
            this._ACTIONS.Clear();
            return this;
        }

        /// <summary>
        /// Entfernt Logik aus der internen Liste.
        /// </summary>
        /// <param name="action">Die Logik, die entfernt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> ist <see langword="null" />.
        /// </exception>
        public DelegateLoggerFacade Remove(Action<ILogMessage> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this._ACTIONS.Remove(action);
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
