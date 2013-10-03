// s. http://blog.marcel-kloubert.de


using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Extension Methoden für Tasks.
/// </summary>
public static partial class __TaskExtensionMethodsNet40
{
    #region Delegates and Events (2)

    // Delegates (2) 

    /// <summary>
    /// Beschreibt eine Methode für eine 'StartNewTask{T}'-Methode.
    /// </summary>
    /// <typeparam name="T">Typ von <paramref name="state" />.</typeparam>
    /// <param name="state">Das State-Objekt.</param>
    /// <param name="cancellationToken">Das zugrundeliegende Cancellation-Token.</param>
    public delegate void StartNewTaskAction<T>(T state, CancellationToken cancellationToken);

    /// <summary>
    /// Beschreibt eine Funktion/Methode für eine 'StartNewTask{T, R}'-Methode.
    /// </summary>
    /// <typeparam name="R">Typ des Rückgabewertes für den zugrundeliegenden Task.</typeparam>
    /// <typeparam name="T">Typ von <paramref name="state" />.</typeparam>
    /// <param name="state">Das State-Objekt.</param>
    /// <param name="cancellationToken">Das zugrundeliegende Cancellation-Token.</param>
    /// <returns>Der Rückgabewert für den zugrundeliegenden Task.</returns>
    public delegate R StartNewTaskFunc<T, R>(T state, CancellationToken cancellationToken);

    #endregion Delegates and Events

    #region Nested Classes (2)

    private sealed class StartNewTaskState<T, R>
    {
        #region Fields (4)

        internal readonly CancellationToken CANCELLATION_TOKEN;
        internal readonly StartNewTaskFunc<T, R> FUNC;
        internal readonly Func<TaskFactory, T> FUNC_STATE_FACTORY;
        internal readonly TaskFactory TASK_FACTORY;

        #endregion Fields

        #region Constructors (1)

        internal StartNewTaskState(TaskFactory taskFactory,
                                   StartNewTaskFunc<T, R> func,
                                   Func<TaskFactory, T> funcStateFactory,
                                   CancellationToken cancellationToken)
        {
            this.FUNC = func;
            this.FUNC_STATE_FACTORY = funcStateFactory;
            this.TASK_FACTORY = taskFactory;
            this.CANCELLATION_TOKEN = cancellationToken;
        }

        #endregion Constructors

        #region Methods (1)

        // Internal Methods (1) 

        internal R Invoke()
        {
            return this.FUNC(cancellationToken: this.CANCELLATION_TOKEN,
                             state: this.FUNC_STATE_FACTORY(this.TASK_FACTORY));
        }

        #endregion Methods
    }

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
