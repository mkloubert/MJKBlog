// s. http://blog.marcel-kloubert.de


using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Blog.Execution.Collections
{
    /// <summary>
    /// Ein Objekt zum Erezugen von Logiken, die eine Liste von Elementen abarbeiten.
    /// </summary>
    /// <typeparam name="T">Typ der zugrundeliegenden Elemente.</typeparam>
    public sealed partial class ItemListExecutor<T>
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ItemListExecutor{T}" />.
        /// </summary>
        /// <param name="items">Die zugrundeliegenden Elemente.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="items" /> ist <see langword="null" />.
        /// </exception>
        public ItemListExecutor(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            this.Items = items;
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gibt die Liste der zugrundeliegenden Elemente zurück.
        /// </summary>
        public IEnumerable<T> Items
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods (3)

        // Public Methods (2) 

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

            // versuchen die Länge der Liste zu ermitteln
            long? count = null;
            {
                var array = this.Items as T[];
                if (array != null)
                {
                    count = array.LongLength;
                }
                else
                {
                    var genericColl = this.Items as ICollection<T>;
                    if (genericColl != null)
                    {
                        count = genericColl.Count;
                    }
                    else
                    {
                        var coll = this.Items as ICollection;
                        if (coll != null)
                        {
                            count = coll.Count;
                        }
                    }
                }
            }

            var result = new SimpleItemListExecutionContext<S>()
                {
                    Action = action,
                    CanceledCallback = null,
                    CancellationContext = null,
                    CancellationSource = new CancellationTokenSource(),
                    CompletedCallback = null,
                    CreationOptions = taskCreationOptions,
                    Errors = null,
                    FaultedCallback = null,
                    ItemCount = count,
                    Items = this.Items,
                    IsFaulted = false,
                    IsRunning = false,
                    LastResult = null,
                    RunAsyn = runAsync,
                    State = actionState,
                    Scheduler = taskScheduler ?? TaskScheduler.Current,
                    SucceededCallback = null,
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
                                var cancelToken = listCtx.CancellationSource.Token;

                                ++i;
                                var itemCtx = new SimpleIItemExecutionContext<S>()
                                    {
                                        Cancel = false,
                                        CancelToken = cancelToken,
                                        Index = i,
                                        Item = item,
                                        ItemCount = listCtx.ItemCount,
                                        LastErrors = lastErr,
                                        Result = null,
                                        State = listCtx.State,
                                    };

                                try
                                {
                                    cancelToken.ThrowIfCancellationRequested();

                                    lastErr = null;
                                    listCtx.Action(itemCtx);

                                    listCtx.LastResult = itemCtx.Result;
                                    if (itemCtx.Cancel)
                                    {
                                        listCtx.CancellationContext = CreateCancellationContext<S>(i,
                                                                                                   listCtx,
                                                                                                   item,
                                                                                                   itemCtx.State,
                                                                                                   ItemCancellationSource.ItemContext);

                                        break;
                                    }
                                }
                                catch (OperationCanceledException)
                                {
                                    listCtx.CancellationContext = CreateCancellationContext<S>(i,
                                                                                               listCtx,
                                                                                               item,
                                                                                               itemCtx.State,
                                                                                               ItemCancellationSource.CancellationToken);
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

                        if (listCtx.IsFaulted)
                        {
                            // Callback für "Vorgang fehlerhaft"

                            var fcb = listCtx.FaultedCallback;
                            if (fcb != null)
                            {
                                fcb(listCtx);
                            }
                        }
                        else if (listCtx.IsCanceled)
                        {
                            // Callback für "Vorgang abgebrochen"

                            var ccb = listCtx.CanceledCallback;
                            if (ccb != null)
                            {
                                ccb(listCtx);
                            }
                        }
                        else
                        {
                            // Callback für "Vorgang erfolgreich"

                            var scb = listCtx.SucceededCallback;
                            if (scb != null)
                            {
                                scb(listCtx);
                            }
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
        // Private Methods (1) 

        private static IItemListCancellationContext<T, S> CreateCancellationContext<S>(long canceledAt,
                                                                                       IItemListExecutionContext<T> execCtx,
                                                                                       T item,
                                                                                       S state,
                                                                                       ItemCancellationSource src)
        {
            return new SimpleItemListCancellationContext<S>()
                {
                    CanceledAt = canceledAt,
                    ExecutionContext = execCtx,
                    Item = item,
                    Source = src,
                    State = state,
                };
        }

        #endregion Methods
    }
}
