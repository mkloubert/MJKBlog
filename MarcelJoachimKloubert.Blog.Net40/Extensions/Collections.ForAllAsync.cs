// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

static partial class __CollectionExtensionMethodsNet40
{
    #region Methods (3)

    // Public Methods (3) 

    /// <summary>
    /// Führt eine Action für jedes Element einer Sequenz asynchron aus.
    /// Jede <see cref="Exception" /> wird gesammelt und in Form einer
    /// <see cref="AggregateException" /> zurückgegeben oder geworfen.
    /// </summary>
    /// <typeparam name="T">Typ der Elemente.</typeparam>
    /// <param name="items">The Sequenz, die die Element enthält.</param>
    /// <param name="action">Die Logik, die für jedes Element von <paramref name="items" /> ausgeführt werden soll.</param>
    /// <param name="throwExceptions">
    /// Wenn mindestens eine <see cref="Exception" /> aufgetreten ist, werfe diese
    /// als <see cref="AggregateException" /> (<see langword="true" />) oder gebe sie stattdessen
    /// als solche zurück (<see langword="false" />).
    /// </param>
    /// <returns>
    /// Die Liste aller aufgetretenen <see cref="Exception" /> oder
    /// <see langword="null" />, wenn keine aufgetreten ist.
    /// </returns>
    /// <exception cref="AggregateException">
    /// Mindestens ein Fehler ist aufgetreten.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="items" /> und/oder <paramref name="action" /> sind
    /// <see langword="null" /> Referenzen.
    /// </exception>
    /// <remarks>
    /// Jede Aufruf von <paramref name="action" /> wird zwar in einem eigenen
    /// <see cref="Task" /> ausgeführt, es wird aber gewartet bis alle Elemente
    /// von <paramref name="items" /> behandelt wurden.
    /// </remarks>
    public static AggregateException ForAllAsync<T>(this IEnumerable<T> items,
                                                    Action<T> action,
                                                    bool throwExceptions = true)
    {
        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        return ForAllAsync<T, object>(items: items,
                                      action: (i, s) => action(i),
                                      actionState: null,
                                      throwExceptions: throwExceptions);
    }

    /// <summary>
    /// Führt eine Action für jedes Element einer Sequenz asynchron aus.
    /// Jede <see cref="Exception" /> wird gesammelt und in Form einer
    /// <see cref="AggregateException" /> zurückgegeben oder geworfen.
    /// </summary>
    /// <typeparam name="T">Typ der Elemente.</typeparam>
    /// <typeparam name="S">Typ des 2. Parameters von <paramref name="action" />.</typeparam>
    /// <param name="items">The Sequenz, die die Element enthält.</param>
    /// <param name="action">Die Logik, die für jedes Element von <paramref name="items" /> ausgeführt werden soll.</param>
    /// <param name="actionState">
    /// Das Objekt für den 2. Parameter von <paramref name="action" />.
    /// </param>
    /// <param name="throwExceptions">
    /// Wenn mindestens eine <see cref="Exception" /> aufgetreten ist, werfe diese
    /// als <see cref="AggregateException" /> (<see langword="true" />) oder gebe sie stattdessen
    /// als solche zurück (<see langword="false" />).
    /// </param>
    /// <returns>
    /// Die Liste aller aufgetretenen <see cref="Exception" /> oder
    /// <see langword="null" />, wenn keine aufgetreten ist.
    /// </returns>
    /// <exception cref="AggregateException">
    /// Mindestens ein Fehler ist aufgetreten.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="items" />, <paramref name="action" /> und/oder
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static AggregateException ForAllAsync<T, S>(this IEnumerable<T> items,
                                                       Action<T, S> action,
                                                       S actionState,
                                                       bool throwExceptions = true)
    {
        return ForAllAsync<T, S>(items: items,
                                 action: action,
                                 actionStateFactory: (i) => actionState,
                                 throwExceptions: throwExceptions);
    }

    /// <summary>
    /// Führt eine Action für jedes Element einer Sequenz asynchron aus.
    /// Jede <see cref="Exception" /> wird gesammelt und in Form einer
    /// <see cref="AggregateException" /> zurückgegeben oder geworfen.
    /// </summary>
    /// <typeparam name="T">Typ der Elemente.</typeparam>
    /// <typeparam name="S">Typ des 2. Parameters von <paramref name="action" />.</typeparam>
    /// <param name="items">The Sequenz, die die Element enthält.</param>
    /// <param name="action">Die Logik, die für jedes Element von <paramref name="items" /> ausgeführt werden soll.</param>
    /// <param name="actionStateFactory">
    /// Das Factory-Delegate, welches das Objekt für den 2. Parameter von <paramref name="action" />
    /// erzeugt. Der 1. Parameter dieses Delegates ist das aktuelle Element von
    /// <paramref name="items" />.
    /// </param>
    /// <param name="throwExceptions">
    /// Wenn mindestens eine <see cref="Exception" /> aufgetreten ist, werfe diese
    /// als <see cref="AggregateException" /> (<see langword="true" />) oder gebe sie stattdessen
    /// als solche zurück (<see langword="false" />).
    /// </param>
    /// <returns>
    /// Die Liste aller aufgetretenen <see cref="Exception" /> oder
    /// <see langword="null" />, wenn keine aufgetreten ist.
    /// </returns>
    /// <exception cref="AggregateException">
    /// Mindestens ein Fehler ist aufgetreten.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="items" />, <paramref name="action" /> und/oder
    /// <paramref name="actionStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static AggregateException ForAllAsync<T, S>(this IEnumerable<T> items,
                                                       Action<T, S> action,
                                                       Func<T, S> actionStateFactory,
                                                       bool throwExceptions = true)
    {
        if (items == null)
        {
            throw new ArgumentNullException("items");
        }

        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        if (actionStateFactory == null)
        {
            throw new ArgumentNullException("actionStateFactory");
        }

        var exceptions = new List<Exception>();
        var syncRoot = new object();

        var tuples = items.Select(i => new ForAllTuple<T, S>(
            item: i,
            action: action,
            actionStateFactory: actionStateFactory,
            exceptions: exceptions,
            syncRoot: syncRoot
        ));

        var tasks = tuples.Select(t =>
        {
            return new Task(action:
                (state) =>
                {
                    var tuple = (ForAllTuple<T, S>)state;
                    try
                    {
                        var i = tuple.ITEM;

                        tuple.ACTION(i,
                                     tuple.ACTION_STATE_FACTORY(i));
                    }
                    catch (Exception ex)
                    {
                        lock (tuple.SYNC)
                        {
                            tuple.EXCEPTION_LIST
                                 .Add(ex);
                        }
                    }
                }, state: t);
        });

        try
        {
            var runningTasks = new List<Task>();
            using (var enumerator = tasks.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    try
                    {
                        var t = enumerator.Current;

                        t.Start();
                        runningTasks.Add(t);
                    }
                    catch (Exception ex)
                    {
                        lock (syncRoot)
                        {
                            exceptions.Add(ex);
                        }
                    }
                }
            }

            Task.WaitAll(runningTasks.ToArray());
            runningTasks.Clear();
        }
        catch (Exception ex)
        {
            lock (syncRoot)
            {
                exceptions.Add(ex);
            }
        }

        AggregateException result = null;
        lock (syncRoot)
        {
            if (exceptions.Count > 0)
            {
                result = new AggregateException(exceptions);
            }
        }

        if (result != null &&
            throwExceptions)
        {
            throw result;
        }

        return result;
    }

    #endregion Methods

    #region Nested Classes (1)

    private sealed class ForAllTuple<T, S>
    {
        #region Fields (5)

        internal readonly Action<T, S> ACTION;
        internal readonly Func<T, S> ACTION_STATE_FACTORY;
        internal readonly ICollection<Exception> EXCEPTION_LIST;
        internal readonly T ITEM;
        internal readonly object SYNC;

        #endregion Fields

        #region Constructors (1)

        internal ForAllTuple(T item,
                                 Action<T, S> action,
                                 Func<T, S> actionStateFactory,
                                 ICollection<Exception> exceptions,
                                 object syncRoot)
        {
            this.ITEM = item;
            this.ACTION = action;
            this.ACTION_STATE_FACTORY = actionStateFactory;
            this.EXCEPTION_LIST = exceptions;
            this.SYNC = syncRoot;
        }

        #endregion Constructors
    }

    #endregion Nested Classes
}
