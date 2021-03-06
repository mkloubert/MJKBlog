﻿// s. http://blog.marcel-kloubert.de


using System;
using System.Threading;
using System.Threading.Tasks;

partial class __TaskExtensionMethodsNet40
{
    #region Delegates and Events (1)

    // Delegates (1) 

    /// <summary>
    /// Beschreibt eine Methode für eine 'StartNewTask{T}'-Methode.
    /// </summary>
    /// <typeparam name="T">Typ von <paramref name="state" />.</typeparam>
    /// <param name="state">Das State-Objekt.</param>
    /// <param name="cancellationToken">Das zugrundeliegende Cancellation-Token.</param>
    public delegate void StartNewTaskAction<T>(T state, CancellationToken cancellationToken);

    #endregion Delegates and Events

    #region Methods (14)

    // Public Methods (14) 

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionState">Der Wert für den ersten Parameter von <paramref name="action" />.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" /> und/oder <paramref name="action" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       T actionState)
    {
        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: f => actionState);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="action" /> liefert.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="action" /> und/oder
    /// <paramref name="actionStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       Func<TaskFactory, T> actionStateFactory)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: actionStateFactory,
                               cancellationToken: factory.CancellationToken,
                               creationOptions: factory.CreationOptions,
                               scheduler: factory.Scheduler ?? TaskScheduler.Default);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionState">Der Wert für den ersten Parameter von <paramref name="action" />.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" /> und/oder <paramref name="action" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       T actionState,
                                       CancellationToken cancellationToken)
    {
        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: f => actionState,
                               cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionState">Der Wert für den ersten Parameter von <paramref name="action" />.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" /> und/oder <paramref name="action" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       T actionState,
                                       TaskCreationOptions creationOptions)
    {
        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: f => actionState,
                               creationOptions: creationOptions);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionState">Der Wert für den ersten Parameter von <paramref name="action" />.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="action" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       T actionState,
                                       TaskScheduler scheduler)
    {
        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: f => actionState,
                               scheduler: scheduler);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="action" /> liefert.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="action" /> und/oder
    /// <paramref name="actionStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       Func<TaskFactory, T> actionStateFactory,
                                       CancellationToken cancellationToken)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: actionStateFactory,
                               cancellationToken: cancellationToken,
                               creationOptions: factory.CreationOptions,
                               scheduler: factory.Scheduler ?? TaskScheduler.Default);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="action" /> liefert.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="action" /> und/oder
    /// <paramref name="actionStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       Func<TaskFactory, T> actionStateFactory,
                                       TaskCreationOptions creationOptions)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: actionStateFactory,
                               cancellationToken: factory.CancellationToken,
                               creationOptions: creationOptions,
                               scheduler: factory.Scheduler ?? TaskScheduler.Default);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="action" /> liefert.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="action" />, <paramref name="actionStateFactory" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       Func<TaskFactory, T> actionStateFactory,
                                       TaskScheduler scheduler)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: actionStateFactory,
                               cancellationToken: factory.CancellationToken,
                               creationOptions: factory.CreationOptions,
                               scheduler: scheduler);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionState">Der Wert für den ersten Parameter von <paramref name="action" />.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" /> und/oder <paramref name="action" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       T actionState,
                                       CancellationToken cancellationToken,
                                       TaskCreationOptions creationOptions)
    {
        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: f => actionState,
                               cancellationToken: cancellationToken,
                               creationOptions: creationOptions);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionState">Der Wert für den ersten Parameter von <paramref name="action" />.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="action" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       T actionState,
                                       CancellationToken cancellationToken,
                                       TaskScheduler scheduler)
    {
        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: f => actionState,
                               cancellationToken: cancellationToken,
                               scheduler: scheduler);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="action" /> liefert.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="action" /> und/oder
    /// <paramref name="actionStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       Func<TaskFactory, T> actionStateFactory,
                                       CancellationToken cancellationToken,
                                       TaskCreationOptions creationOptions)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: actionStateFactory,
                               cancellationToken: factory.CancellationToken,
                               creationOptions: factory.CreationOptions,
                               scheduler: factory.Scheduler ?? TaskScheduler.Default);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="action" /> liefert.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="action" />, <paramref name="actionStateFactory" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       Func<TaskFactory, T> actionStateFactory,
                                       CancellationToken cancellationToken,
                                       TaskScheduler scheduler)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: actionStateFactory,
                               cancellationToken: factory.CancellationToken,
                               creationOptions: factory.CreationOptions,
                               scheduler: scheduler);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionState">Der Wert für den ersten Parameter von <paramref name="action" />.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="action" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       T actionState,
                                       CancellationToken cancellationToken,
                                       TaskCreationOptions creationOptions,
                                       TaskScheduler scheduler)
    {
        return StartNewTask<T>(factory: factory,
                               action: action,
                               actionStateFactory: f => actionState,
                               cancellationToken: cancellationToken,
                               creationOptions: creationOptions,
                               scheduler: scheduler);
    }

    /// <summary>
    /// Wrapper für <see cref="TaskFactory.StartNew(Action{object}, object, CancellationToken, TaskCreationOptions, TaskScheduler)" />
    /// mit typisiertem State-Objekt für <paramref name="action" />.
    /// </summary>
    /// <typeparam name="T">Typ des ersten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="factory">Die zugrundeliegende Task-Factory.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionStateFactory">Die Funktion, die den Wert für den ersten Parameter von <paramref name="action" /> liefert.</param>
    /// <param name="cancellationToken">Das Token zum Abbrechen des Tasks.</param>
    /// <param name="creationOptions">Die Optionen zum Erstellen des Tasks.</param>
    /// <param name="scheduler">Der zugrundeliegende Scheduler.</param>
    /// <returns>Der neue Task.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory" />, <paramref name="action" />, <paramref name="actionStateFactory" /> und/oder
    /// <paramref name="scheduler" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Task StartNewTask<T>(this TaskFactory factory,
                                       StartNewTaskAction<T> action,
                                       Func<TaskFactory, T> actionStateFactory,
                                       CancellationToken cancellationToken,
                                       TaskCreationOptions creationOptions,
                                       TaskScheduler scheduler)
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        if (actionStateFactory == null)
        {
            throw new ArgumentNullException("actionStateFactory");
        }

        if (scheduler == null)
        {
            throw new ArgumentNullException("scheduler");
        }

        return factory.StartNew(
            action:
                (state) =>
                {
                    var s = (StartNewTaskState<T>)state;
                    s.Invoke();
                },
            state: new StartNewTaskState<T>(taskFactory: factory,
                                            action: action,
                                            actionStateFactory: actionStateFactory,
                                            cancellationToken: cancellationToken),
            cancellationToken: cancellationToken,
            creationOptions: creationOptions,
            scheduler: scheduler);
    }

    #endregion Methods

    #region Nested Classes (1)

    private sealed class StartNewTaskState<T>
    {
        #region Fields (4)

        internal readonly StartNewTaskAction<T> ACTION;
        internal readonly Func<TaskFactory, T> ACTION_STATE_FACTORY;
        internal readonly CancellationToken CANCELLATION_TOKEN;
        internal readonly TaskFactory TASK_FACTORY;

        #endregion Fields

        #region Constructors (1)

        internal StartNewTaskState(TaskFactory taskFactory,
                                   StartNewTaskAction<T> action,
                                   Func<TaskFactory, T> actionStateFactory,
                                   CancellationToken cancellationToken)
        {
            this.ACTION = action;
            this.ACTION_STATE_FACTORY = actionStateFactory;
            this.TASK_FACTORY = taskFactory;
            this.CANCELLATION_TOKEN = cancellationToken;
        }

        #endregion Constructors

        #region Methods (1)

        // Internal Methods (1) 

        internal void Invoke()
        {
            this.ACTION(cancellationToken: this.CANCELLATION_TOKEN,
                        state: this.ACTION_STATE_FACTORY(this.TASK_FACTORY));
        }

        #endregion Methods
    }

    #endregion Nested Classes
}
