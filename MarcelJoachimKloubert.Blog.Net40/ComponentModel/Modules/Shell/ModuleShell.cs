// s. http://blog.marcel-kloubert.de


using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using MarcelJoachimKloubert.Blog.Diagnostics;
using MarcelJoachimKloubert.Blog.MEF;
using MarcelJoachimKloubert.Blog.Values;

namespace MarcelJoachimKloubert.Blog.ComponentModel.Modules.Shell
{
    /// <summary>
    /// Eine Shell, die Module verwaltet.
    /// </summary>
    public sealed class ModuleShell : SyncDisposableBase
    {
        #region Properties (6)

        /// <summary>
        /// Gibt den Logger zurück, der Teil von <see cref="ModuleShell.Logger" /> ist
        /// und lediglich dazu dient vereinfacht Delegates als Logger hinzuzufügen.
        /// </summary>
        public DelegateLogger InnerFunctionLogger
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt zurück, ob diese Shell initialisiert wurde oder nicht.
        /// </summary>
        public bool IsInitialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt den Logger dieser Shell zurück.
        /// </summary>
        public AggregateLogger Logger
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt den Katalog zurück, der zum Erstellen von <see cref="IModule" />n verwendet wird.
        /// </summary>
        public AggregateCatalog ModuleCompositionCatalog
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt die aktuelle Liste der Module zurück.
        /// </summary>
        public IModule[] Modules
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt den State dieser Shell zurück.
        /// </summary>
        public IValueRouter<TrafficLightState> State
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods (8)

        // Public Methods (4) 

        /// <summary>
        /// Fügt einen Logger zur asynchronen Ausführung der Instanz in <see cref="ModuleShell.Logger" /> hinzu.
        /// </summary>
        /// <param name="logger">Der Logger, der hinzugefügt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger" /> ist <see langword="null" />.
        /// </exception>
        public ModuleShell AddAsyncLogger(ILoggerFacade logger)
        {
            logger.ThrowIfParamIsNull(() => logger);

            lock (this._SYNC_ROOT)
            {
                this.ThrowIfDisposed();
                this.ThrowIfNotInitialized();

                this.Logger
                    .Add(new TaskLogger(innerLogger: logger,
                                        isThreadSafe: false));
            }

            return this;
        }

        /// <summary>
        /// Fügt eine Logger-Action zur asynchronen Ausführung der Instanz in <see cref="ModuleShell.Logger" /> hinzu.
        /// </summary>
        /// <param name="action">Die Action, die hinzugefügt werden soll.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> ist <see langword="null" />.
        /// </exception>
        public ModuleShell AddAsyncLoggerAction(Action<ILogMessage> action)
        {
            var delegateLogger = new DelegateLogger();
            delegateLogger.Add(action);

            return this.AddAsyncLogger(delegateLogger);
        }

        /// <summary>
        /// Initialisiert diese Shell.
        /// </summary>
        /// <param name="context">Der Kontext der die Daten um Initialisieren enthält.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> ist <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Diese Shell wurde bereits initialisiert.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Diese Shell wurde bereits verworfen.
        /// </exception>
        public void Inititialize(IModuleShellInitializeContext context)
        {
            context.ThrowIfParamIsNull(() => context);

            lock (this._SYNC_ROOT)
            {
                this.ThrowIfDisposed();
                if (this.IsInitialized)
                {
                    throw new InvalidOperationException();
                }

                this.InnerFunctionLogger = new DelegateLogger();

                this.Logger = new AggregateLogger();
                this.Logger.Add(this.InnerFunctionLogger);

                this.State = new EnumValueRouter<TrafficLightState>();
                this.ModuleCompositionCatalog = new AggregateCatalog();

                this.ReloadModulesInner();

                this.IsInitialized = true;
            }
        }

        /// <summary>
        /// Lädt die Module dieser Shell erneut.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Diese Shell wurde noch nicht initialisiert.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Diese Shell wurde bereits verworfen.
        /// </exception>
        public void ReloadModules()
        {
            lock (this._SYNC_ROOT)
            {
                this.ThrowIfDisposed();
                this.ThrowIfNotInitialized();

                this.ReloadModulesInner();
            }
        }
        // Protected Methods (1) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="SyncDisposableBase.OnDispose(DisposeContext)" />
        protected override void OnDispose(DisposeContext context)
        {
            base.OnDispose(context);

            // dies zu allerletzt machen
            {
                var funcLogger = this.InnerFunctionLogger;
                if (funcLogger != null)
                {
                    funcLogger.Clear();
                }

                var aggLogger = this.Logger;
                if (aggLogger != null)
                {
                    aggLogger.Clear();
                }
            }
        }
        // Private Methods (3) 

        private void DisposeOldModules()
        {
            const string LOG_TAG = "DisposeOldModules";

            var oldModules = this.Modules;
            if (oldModules == null)
            {
                return;
            }

            var ex = oldModules.ForAllAsync((m, state) =>
                {
                    //TODO
                }, actionState: new
                {
                    Shell = this,
                }, throwExceptions: false);

            if (ex != null)
            {
                this.Logger
                    .Log(msg: ex,
                         categories: LoggerFacadeCategories.Errors,
                         tag: LOG_TAG);
            }

            this.Modules = oldModules.Where(m => m.IsInitialized &&
                                                 !m.IsDisposed)
                                     .ToArray();
        }

        private void ReloadModulesInner()
        {
            const string LOG_TAG = "ReloadModules";

            this.DisposeOldModules();

            var container = new CompositionContainer(this.ModuleCompositionCatalog,
                                                     isThreadSafe: false);

            var composer = new MultiInstanceComposer<IModule>(container);
            composer.RefreshIfNeeded();

            var ex = composer.Instances
                             .ForAllAsync((m, state) =>
                                  {
                                      if (m.IsDisposed)
                                      {
                                          return;
                                      }

                                      if (!m.IsInitialized)
                                      {
                                          var ctx = new SimpleModuleInitializeContext()
                                              {

                                              };

                                          m.Initialize(ctx);
                                      }
                                  }, actionState: new
                                         {
                                             Shell = this,
                                         }, throwExceptions: false);

            if (ex != null)
            {
                this.Logger
                    .Log(msg: ex,
                         categories: LoggerFacadeCategories.Errors,
                         tag: LOG_TAG);
            }

            this.Modules = composer.Instances
                                   .Where(m => m.IsInitialized &&
                                               !m.IsDisposed)
                                   .ToArray();
        }

        private void ThrowIfNotInitialized()
        {
            if (!this.IsInitialized)
            {
                throw new InvalidOperationException();
            }
        }

        #endregion Methods
    }
}
