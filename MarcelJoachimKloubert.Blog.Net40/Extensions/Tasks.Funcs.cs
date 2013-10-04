// s. http://blog.marcel-kloubert.de


using System;
using System.Threading;
using System.Threading.Tasks;

partial class __TaskExtensionMethodsNet40
{
    #region Methods (14)

    // Public Methods (14) 

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcState">Der Wert für den ersten Parameter von <paramref name="func" />.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" /> und/oder <paramref name="func" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             T funcState)
    {
        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: f => funcState);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="func" /> liefert.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="func" /> und/oder
    /// <paramref name="funcStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             Func<TaskFactory, T> funcStateFactory)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: funcStateFactory,
                                  cancellationToken: factory.CancellationToken,
                                  creationOptions: factory.CreationOptions,
                                  scheduler: factory.Scheduler ?? TaskScheduler.Default);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcState">Der Wert für den ersten Parameter von <paramref name="func" />.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" /> und/oder <paramref name="func" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             T funcState,
                                             CancellationToken cancellationToken)
    {
        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: f => funcState,
                                  cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcState">Der Wert für den ersten Parameter von <paramref name="func" />.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" /> und/oder <paramref name="func" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             T funcState,
                                             TaskCreationOptions creationOptions)
    {
        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: f => funcState,
                                  creationOptions: creationOptions);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcState">Der Wert für den ersten Parameter von <paramref name="func" />.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="func" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             T funcState,
                                             TaskScheduler scheduler)
    {
        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: f => funcState,
                                  scheduler: scheduler);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="func" /> liefert.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="func" /> und/oder
    /// <paramref name="funcStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             Func<TaskFactory, T> funcStateFactory,
                                             CancellationToken cancellationToken)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: funcStateFactory,
                                  cancellationToken: cancellationToken,
                                  creationOptions: factory.CreationOptions,
                                  scheduler: factory.Scheduler ?? TaskScheduler.Default);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="func" /> liefert.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="func" /> und/oder
    /// <paramref name="funcStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             Func<TaskFactory, T> funcStateFactory,
                                             TaskCreationOptions creationOptions)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: funcStateFactory,
                                  cancellationToken: factory.CancellationToken,
                                  creationOptions: creationOptions,
                                  scheduler: factory.Scheduler ?? TaskScheduler.Default);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="func" /> liefert.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="func" />, <paramref name="funcStateFactory" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             Func<TaskFactory, T> funcStateFactory,
                                             TaskScheduler scheduler)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: funcStateFactory,
                                  cancellationToken: factory.CancellationToken,
                                  creationOptions: factory.CreationOptions,
                                  scheduler: scheduler);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcState">Der Wert für den ersten Parameter von <paramref name="func" />.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" /> und/oder <paramref name="func" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             T funcState,
                                             CancellationToken cancellationToken,
                                             TaskCreationOptions creationOptions)
    {
        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: f => funcState,
                                  cancellationToken: cancellationToken,
                                  creationOptions: creationOptions);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcState">Der Wert für den ersten Parameter von <paramref name="func" />.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="func" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             T funcState,
                                             CancellationToken cancellationToken,
                                             TaskScheduler scheduler)
    {
        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: f => funcState,
                                  cancellationToken: cancellationToken,
                                  scheduler: scheduler);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="func" /> liefert.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="func" /> und/oder
    /// <paramref name="funcStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             Func<TaskFactory, T> funcStateFactory,
                                             CancellationToken cancellationToken,
                                             TaskCreationOptions creationOptions)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: funcStateFactory,
                                  cancellationToken: factory.CancellationToken,
                                  creationOptions: factory.CreationOptions,
                                  scheduler: factory.Scheduler ?? TaskScheduler.Default);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="func" /> liefert.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="func" />, <paramref name="funcStateFactory" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             Func<TaskFactory, T> funcStateFactory,
                                             CancellationToken cancellationToken,
                                             TaskScheduler scheduler)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: funcStateFactory,
                                  cancellationToken: factory.CancellationToken,
                                  creationOptions: factory.CreationOptions,
                                  scheduler: scheduler);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcState">Der Wert für den ersten Parameter von <paramref name="func" />.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="func" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             T funcState,
                                             CancellationToken cancellationToken,
                                             TaskCreationOptions creationOptions,
                                             TaskScheduler scheduler)
    {
        return StartNewTask<T, R>(factory: factory,
                                  func: func,
                                  funcStateFactory: f => funcState,
                                  cancellationToken: cancellationToken,
                                  creationOptions: creationOptions,
                                  scheduler: scheduler);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew{TResult}(Func{object, TResult}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="func" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Der Rückgabewert von <paramref name="func" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="func" /> liefert.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="func" />, <paramref name="funcStateFactory" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task<R> StartNewTask<T, R>(this TaskFactory factory,
                                             StartNewTaskFunc<T, R> func,
                                             Func<TaskFactory, T> funcStateFactory,
                                             CancellationToken cancellationToken,
                                             TaskCreationOptions creationOptions,
                                             TaskScheduler scheduler)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        if (func == null)
        {
            throw new ArgumentNullException("action");
        }

        if (funcStateFactory == null)
        {
            throw new ArgumentNullException("funcStateFactory");
        }

        if (scheduler == null)
        {
            throw new ArgumentNullException("scheduler");
        }

        return factory.StartNew<R>(
            function:
                (state) =>
                {
                    var s = (StartNewTaskState<T, R>)state;
                    return s.Invoke();
                },
            state: new StartNewTaskState<T, R>(taskFactory: factory,
                                               func: func,
                                               funcStateFactory: funcStateFactory,
                                               cancellationToken: cancellationToken),
            cancellationToken: cancellationToken,
            creationOptions: creationOptions,
            scheduler: scheduler);
    }

    #endregion Methods
}
