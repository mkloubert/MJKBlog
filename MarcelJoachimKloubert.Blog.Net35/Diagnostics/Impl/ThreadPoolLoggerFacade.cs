// s. http://blog.marcel-kloubert.de


using System;
using System.Threading;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Eine <see cref="ILoggerFacade" />, die ein Wrapper für einen anderen
    /// Logger ist und dessen Logik asynchron über einen <see cref="ThreadPool" />
    /// ausführt.
    /// </summary>
    public sealed class ThreadPoolLoggerFacade : LoggerFacadeWrapperBase
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ThreadPoolLoggerFacade" />.
        /// </summary>
        /// <param name="innerLogger">Der zugrundeliegende Logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerLogger" /> ist <see langword="null" />.
        /// </exception>
        public ThreadPoolLoggerFacade(ILoggerFacade innerLogger)
            : base(innerLogger)
        {

        }

        #endregion Constructors

        #region Methods (2)

        // Protected Methods (1) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="LoggerFacadeBase.OnLog(ILogMessage)" />
        protected override void OnLog(ILogMessage msg)
        {
            ThreadPool.QueueUserWorkItem(this.OnLogTaskAction,
                                         msg);
        }
        // Private Methods (1) 

        private void OnLogTaskAction(object state)
        {
            try
            {
                var msg = state as ILogMessage;
                if (msg == null)
                {
                    return;
                }

                this.InnerLogger
                    .Log(msg);
            }
            catch
            {
                // Fehler ignorieren
            }
        }

        #endregion Methods
    }
}
