﻿// s. http://blog.marcel-kloubert.de


using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Eine <see cref="ILoggerFacade" />, die ein Wrapper für einen anderen
    /// Logger ist und dessen Logik asynchron in einem <see cref="Task" />
    /// ausführt.
    /// </summary>
    public sealed class TaskLoggerFacade : LoggerFacadeWrapperBase
    {
        #region Fields (3)

        private readonly TaskCreationOptions _OPTIONS;
        private readonly TaskScheduler _SCHEDULER;
        private readonly CancellationToken _TOKEN;

        #endregion Fields

        #region Constructors (8)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="TaskLoggerFacade" />.
        /// </summary>
        /// <param name="innerLogger">Der zugrundeliegende Logger.</param>
        /// <param name="token">Das <see cref="CancellationToken" />, das den Abbruch des Tasks steuert.</param>
        /// <param name="options">Die Optionen zum erstellen des Tasks</param>
        /// <param name="scheduler">Der zugrundliegende Scheduler.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerLogger" /> und/oder <paramref name="scheduler" />
        /// sind <see langword="null" />.
        /// </exception>
        public TaskLoggerFacade(ILoggerFacade innerLogger,
                                CancellationToken token,
                                TaskCreationOptions options,
                                TaskScheduler scheduler)
            : base(innerLogger: innerLogger)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException("scheduler");
            }

            this._TOKEN = token;
            this._OPTIONS = options;
            this._SCHEDULER = scheduler;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="TaskLoggerFacade" />.
        /// </summary>
        /// <param name="innerLogger">Der zugrundeliegende Logger.</param>
        /// <param name="token">Das <see cref="CancellationToken" />, das den Abbruch des Tasks steuert.</param>
        /// <param name="options">Die Optionen zum erstellen des Tasks</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerLogger" /> ist <see langword="null" />.
        /// </exception>
        public TaskLoggerFacade(ILoggerFacade innerLogger,
                                CancellationToken token,
                                TaskCreationOptions options)
            : this(innerLogger: innerLogger,
                   token: token,
                   options: options,
                   scheduler: Task.Factory.Scheduler)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="TaskLoggerFacade" />.
        /// </summary>
        /// <param name="innerLogger">Der zugrundeliegende Logger.</param>
        /// <param name="token">Das <see cref="CancellationToken" />, das den Abbruch des Tasks steuert.</param>
        /// <param name="scheduler">Der zugrundliegende Scheduler.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerLogger" /> und/oder <paramref name="scheduler" />
        /// sind <see langword="null" />.
        /// </exception>
        public TaskLoggerFacade(ILoggerFacade innerLogger,
                                CancellationToken token,
                                TaskScheduler scheduler)
            : this(innerLogger: innerLogger,
                   token: token,
                   options: Task.Factory.CreationOptions,
                   scheduler: scheduler)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="TaskLoggerFacade" />.
        /// </summary>
        /// <param name="innerLogger">Der zugrundeliegende Logger.</param>
        /// <param name="options">Die Optionen zum erstellen des Tasks</param>
        /// <param name="scheduler">Der zugrundliegende Scheduler.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerLogger" /> und/oder <paramref name="scheduler" />
        /// sind <see langword="null" />.
        /// </exception>
        public TaskLoggerFacade(ILoggerFacade innerLogger,
                                TaskCreationOptions options,
                                TaskScheduler scheduler)
            : this(innerLogger: innerLogger,
                   token: Task.Factory.CancellationToken,
                   options: options,
                   scheduler: scheduler)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="TaskLoggerFacade" />.
        /// </summary>
        /// <param name="innerLogger">Der zugrundeliegende Logger.</param>
        /// <param name="token">Das <see cref="CancellationToken" />, das den Abbruch des Tasks steuert.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerLogger" /> ist <see langword="null" />.
        /// </exception>
        public TaskLoggerFacade(ILoggerFacade innerLogger,
                                CancellationToken token)
            : this(innerLogger: innerLogger,
                   token: token,
                   options: Task.Factory.CreationOptions,
                   scheduler: Task.Factory.Scheduler)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="TaskLoggerFacade" />.
        /// </summary>
        /// <param name="innerLogger">Der zugrundeliegende Logger.</param>
        /// <param name="options">Die Optionen zum erstellen des Tasks</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerLogger" /> ist <see langword="null" />.
        /// </exception>
        public TaskLoggerFacade(ILoggerFacade innerLogger,
                                TaskCreationOptions options)
            : this(innerLogger: innerLogger,
                   token: Task.Factory.CancellationToken,
                   options: options,
                   scheduler: Task.Factory.Scheduler)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="TaskLoggerFacade" />.
        /// </summary>
        /// <param name="innerLogger">Der zugrundeliegende Logger.</param>
        /// <param name="scheduler">Der zugrundliegende Scheduler.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerLogger" /> und/oder <paramref name="scheduler" />
        /// sind <see langword="null" />.
        /// </exception>
        public TaskLoggerFacade(ILoggerFacade innerLogger,
                                TaskScheduler scheduler)
            : this(innerLogger: innerLogger,
                   token: Task.Factory.CancellationToken,
                   options: Task.Factory.CreationOptions,
                   scheduler: scheduler)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="TaskLoggerFacade" />.
        /// </summary>
        /// <param name="innerLogger">Der zugrundeliegende Logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="innerLogger" /> ist <see langword="null" />.
        /// </exception>
        public TaskLoggerFacade(ILoggerFacade innerLogger)
            : this(innerLogger: innerLogger,
                   token: Task.Factory.CancellationToken,
                   options: Task.Factory.CreationOptions,
                   scheduler: Task.Factory.Scheduler)
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
            Task.Factory
                .StartNew(action: this.OnLogTaskAction,
                          state: msg,
                          cancellationToken: this._TOKEN,
                          creationOptions: this._OPTIONS,
                          scheduler: this._SCHEDULER);
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
