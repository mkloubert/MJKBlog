// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Blog.Execution.Collections
{
    /// <summary>
    /// Ein Objekt zum Erezugen von Logiken, die eine Liste von Elementen abarbeiten.
    /// </summary>
    /// <typeparam name="T">Typ der zugrundeliegenden Elemente.</typeparam>
    public sealed partial class ItemExecutionBuilder<T>
    {
        #region Fields (2)

        private readonly List<T> _ITEMS = new List<T>();
        private readonly object _SYNC;

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ItemExecutionBuilder{T}" />.
        /// </summary>
        /// <param name="syncRoot">Das Objekt für Thread-sichere Operationen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="syncRoot" /> ist <see langword="null" />.
        /// </exception>
        public ItemExecutionBuilder(object syncRoot)
        {
            if (syncRoot == null)
            {
                throw new ArgumentNullException("syncRoot");
            }

            this._SYNC = syncRoot;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ItemExecutionBuilder{T}" />.
        /// </summary>
        public ItemExecutionBuilder()
            : this(new object())
        {

        }

        #endregion Constructors

        #region Methods (7)

        // Public Methods (7) 

        /// <summary>
        /// Fügt ein neues Element der internen Liste hinzu.
        /// </summary>
        /// <param name="item">Das Element, das hinzugefügt werden soll.</param>
        public void Add(T item)
        {
            lock (this._SYNC)
            {
                this._ITEMS
                    .Add(item);
            }
        }

        /// <summary>
        /// Fügt ein Liste von Elementen der internen Liste hinzu.
        /// </summary>
        /// <param name="items">Die Elemente, die hinzugefügt werden sollen.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="items" /> ist <see langword="null" />.
        /// </exception>
        public void AddRange(IEnumerable<T> items)
        {
            lock (this._SYNC)
            {
                this._ITEMS.AddRange(items);
            }
        }

        /// <summary>
        /// Leert die Liste der internen Elemente.
        /// </summary>
        public void Clear()
        {
            lock (this._SYNC)
            {
                this._ITEMS
                    .Clear();
            }
        }

        /// <summary>
        /// Erzeugt einen neuen Ausführungskontext auf Basis dieser Liste und einer Action,
        /// die für jedes Element ausgeführt wird.
        /// </summary>
        /// <param name="action">Die Logik, die für ein Element ausgeführt werden soll.</param>
        /// <param name="runAsync">Den Kontext synchron ausführen oder nicht.</param>
        /// <param name="autoStart">Kontext automatisch starten oder nicht.</param>
        /// <param name="taskCreationOptions">Die Optionen mit denen der zugrundeliegende Task erstellt werden soll.</param>
        /// <param name="taskScheduler">Der optionale <see cref="TaskScheduler" /> der für die Ausführung verwendet werden soll.</param>
        /// <returns>Der erzeugte Kontext-</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> ist <see langword="null" />.
        /// </exception>
        public IItemListExecutionContext<T> Execute(Action<IItemExecutionContext<T, object>> action,
                                                    bool runAsync = true,
                                                    bool autoStart = false,
                                                    TaskCreationOptions taskCreationOptions = TaskCreationOptions.LongRunning,
                                                    TaskScheduler taskScheduler = null)
        {
            return this.Execute<object>(action: action,
                                        actionState: null,
                                        runAsync: runAsync,
                                        autoStart: autoStart,
                                        taskCreationOptions: taskCreationOptions,
                                        taskScheduler: taskScheduler);
        }

        /// <summary>
        /// Erzeugt einen neuen Ausführungskontext auf Basis dieser Liste und einer Action,
        /// die für jedes Element ausgeführt wird.
        /// </summary>
        /// <typeparam name="S">Der Typ von <paramref name="actionState" />.</typeparam>
        /// <param name="action">Die Logik, die für ein Element ausgeführt werden soll.</param>
        /// <param name="actionState">Das zusätzliche Objekt für <paramref name="action" />.</param>
        /// <param name="runAsync">Den Kontext synchron ausführen oder nicht.</param>
        /// <param name="autoStart">Kontext automatisch starten oder nicht.</param>
        /// <param name="taskCreationOptions">Die Optionen mit denen der zugrundeliegende Task erstellt werden soll.</param>
        /// <param name="taskScheduler">Der optionale <see cref="TaskScheduler" /> der für die Ausführung verwendet werden soll.</param>
        /// <returns>Der erzeugte Kontext-</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> ist <see langword="null" />.
        /// </exception>
        public IItemListExecutionContext<T, S> Execute<S>(Action<IItemExecutionContext<T, S>> action,
                                                          S actionState,
                                                          bool runAsync = true,
                                                          bool autoStart = false,
                                                          TaskCreationOptions taskCreationOptions = TaskCreationOptions.LongRunning,
                                                          TaskScheduler taskScheduler = null)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var list = this.GetItems();

            var result = new SimpleItemListExecutionContext<S>()
                {
                    Action = action,
                    CanceledAt = null,
                    CancellationSource = new CancellationTokenSource(),
                    CreationOptions = taskCreationOptions,
                    Errors = null,
                    ItemCount = list.Count,
                    Items = list,
                    IsFaulted = false,
                    IsRunning = false,
                    LastResult = null,
                    RunAsyn = runAsync,
                    State = actionState,
                    Scheduler = taskScheduler ?? TaskScheduler.Current,
                };

            var task = new Task(action: (state) =>
                {
                    var listCtx = (SimpleItemListExecutionContext<S>)state;

                    try
                    {
                        var exceptions = new List<Exception>();

                        try
                        {
                            long i = -1;
                            AggregateException lastErr = null;
                            foreach (var item in listCtx.Items)
                            {
                                ++i;
                                var itemCtx = new SimpleIItemExecutionContext<S>()
                                    {
                                        Cancel = false,
                                        CancelToken = listCtx.CancellationSource.Token,
                                        Index = i,
                                        Item = item,
                                        ItemCount = listCtx.ItemCount,
                                        LastErrors = lastErr,
                                        Result = null,
                                        State = listCtx.State,
                                    };

                                try
                                {
                                    lastErr = null;
                                    listCtx.Action(itemCtx);

                                    listCtx.LastResult = itemCtx.Result;
                                    if (itemCtx.Cancel)
                                    {
                                        listCtx.CanceledAt = i;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lastErr = ex as AggregateException;
                                    if (lastErr == null)
                                    {
                                        lastErr = new AggregateException(ex);
                                    }

                                    exceptions.Add(new ItemExecutionException<T, S>(index: i,
                                                                                    item: item,
                                                                                    innerException: lastErr,
                                                                                    message: ex.Message,
                                                                                    state: itemCtx.State));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            listCtx.IsFaulted = true;

                            exceptions.Add(ex);
                        }

                        if (exceptions.Count > 0)
                        {
                            listCtx.Errors = new AggregateException(exceptions);
                        }

                        if (listCtx.IsFaulted)
                        {
                            // Ausführung erfolgreich

                            var cb = listCtx.SucceededCallback;
                            if (cb != null)
                            {
                                cb(listCtx);
                            }
                        }
                        else
                        {
                            // Fehler bei Ausführung

                            var cb = listCtx.FaultedCallback;
                            if (cb != null)
                            {
                                cb(listCtx);
                            }
                        }
                    }
                    finally
                    {
                        listCtx.IsRunning = false;

                        // Callback für "Vorgang beendet"
                        var cb = listCtx.CompletedCallback;
                        if (cb != null)
                        {
                            cb(listCtx);
                        }
                    }
                }, state: result
                 , creationOptions: result.CreationOptions);

            result.Task = task;

            if (autoStart)
            {
                result.Start();
            }

            return result;
        }

        /// <summary>
        /// Gibt die aktuelle Liste der Elemente zurück.
        /// </summary>
        /// <returns>Die aktuelle Liste der Elemente.</returns>
        public List<T> GetItems()
        {
            lock (this._SYNC)
            {
                return new List<T>(this._ITEMS);
            }
        }

        /// <summary>
        /// Entfernt ein vorhandenes Element aus der internen Liste.
        /// </summary>
        /// <param name="item">Das Element, das entfernt werden soll.</param>
        /// <returns>Element wurde entfernt oder nicht, da es kein Teil der Liste war.</returns>
        public bool Remove(T item)
        {
            lock (this._SYNC)
            {
                return this._ITEMS
                           .Remove(item);
            }
        }

        #endregion Methods
    }
}
