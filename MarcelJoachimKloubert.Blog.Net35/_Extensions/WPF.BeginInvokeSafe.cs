// s. http://blog.marcel-kloubert.de


using System;
using System.Windows.Threading;

partial class __WPFExtensionMethods
{
    #region Methods (6)

    // Public Methods (6) 

    /// <summary>
    /// Wrapper für die Methode <see cref="Dispatcher.BeginInvoke(Delegate, DispatcherPriority, object[])" />.
    /// </summary>
    /// <typeparam name="TDispatcher">Typ des zugrundeliegenden <see cref="DispatcherObject" />.</typeparam>
    /// <param name="dispObj">Das zugrundeliegende <see cref="DispatcherObject" />.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <returns>Der zugrundeliegende <see cref="DispatcherOperation" />-Kontext.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dispObj" /> und/oder <paramref name="action" /> und/oder
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static DispatcherOperation BeginInvokeSafe<TDispatcher>(this TDispatcher dispObj,
                                                                   Action<TDispatcher> action)
        where TDispatcher : global::System.Windows.Threading.DispatcherObject
    {
        return BeginInvokeSafe<TDispatcher>(dispObj,
                                            DispatcherPriority.Normal,
                                            action);
    }

    /// <summary>
    /// Wrapper für die Methode <see cref="Dispatcher.BeginInvoke(Delegate, DispatcherPriority, object[])" />.
    /// </summary>
    /// <typeparam name="TDispatcher">Typ des zugrundeliegenden <see cref="DispatcherObject" />.</typeparam>
    /// <param name="dispObj">Das zugrundeliegende <see cref="DispatcherObject" />.</param>
    /// <param name="prio">
    /// Die Priorität des zugrundeliegenden <see cref="Dispatcher" />s
    /// von <paramref name="dispObj" />.
    /// </param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <returns>Der zugrundeliegende <see cref="DispatcherOperation" />-Kontext.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dispObj" /> und/oder <paramref name="action" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static DispatcherOperation BeginInvokeSafe<TDispatcher>(this TDispatcher dispObj,
                                                                   DispatcherPriority prio,
                                                                   Action<TDispatcher> action)
        where TDispatcher : global::System.Windows.Threading.DispatcherObject
    {
        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        return BeginInvokeSafe<TDispatcher, object>(dispObj,
                                                    prio,
                                                    (@do, s) => action(@do),
                                                    (object)null);
    }

    /// <summary>
    /// Wrapper für die Methode <see cref="Dispatcher.BeginInvoke(Delegate, DispatcherPriority, object[])" />
    /// mit stark typisiertem state-Objekt.
    /// </summary>
    /// <typeparam name="TDispatcher">Typ des zugrundeliegenden <see cref="DispatcherObject" />.</typeparam>
    /// <typeparam name="T">Typ des zweiten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="dispObj">Das zugrundeliegende <see cref="DispatcherObject" />.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionState">
    /// Der zweite Parameter für <paramref name="action" />.
    /// </param>
    /// <returns>Der zugrundeliegende <see cref="DispatcherOperation" />-Kontext.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dispObj" />, und/oder <paramref name="action" /> und/oder
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static DispatcherOperation BeginInvokeSafe<TDispatcher, T>(this TDispatcher dispObj,
                                                                      Action<TDispatcher, T> action,
                                                                      T actionState)
        where TDispatcher : global::System.Windows.Threading.DispatcherObject
    {
        return BeginInvokeSafe<TDispatcher, T>(dispObj,
                                               action,
                                               (ctrl) => actionState);
    }

    /// <summary>
    /// Wrapper für die Methode <see cref="Dispatcher.BeginInvoke(Delegate, DispatcherPriority, object[])" />
    /// mit stark typisiertem state-Objekt.
    /// </summary>
    /// <typeparam name="TDispatcher">Typ des zugrundeliegenden <see cref="DispatcherObject" />.</typeparam>
    /// <typeparam name="T">Typ des zweiten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="dispObj">Das zugrundeliegende <see cref="DispatcherObject" />.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionStateFactory">
    /// Die Funktion / Methode, die den zweiten Parameter für <paramref name="action" /> liefert.
    /// </param>
    /// <returns>Der zugrundeliegende <see cref="DispatcherOperation" />-Kontext.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dispObj" />, <paramref name="action" /> und/oder
    /// <paramref name="actionStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static DispatcherOperation BeginInvokeSafe<TDispatcher, T>(this TDispatcher dispObj,
                                                                      Action<TDispatcher, T> action,
                                                                      Func<TDispatcher, T> actionStateFactory)
        where TDispatcher : global::System.Windows.Threading.DispatcherObject
    {
        return BeginInvokeSafe<TDispatcher, T>(dispObj,
                                               DispatcherPriority.Normal,
                                               action,
                                               actionStateFactory);
    }

    /// <summary>
    /// Wrapper für die Methode <see cref="Dispatcher.BeginInvoke(Delegate, DispatcherPriority, object[])" />
    /// mit stark typisiertem state-Objekt.
    /// </summary>
    /// <typeparam name="TDispatcher">Typ des zugrundeliegenden <see cref="DispatcherObject" />.</typeparam>
    /// <typeparam name="T">Typ des zweiten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="dispObj">Das zugrundeliegende <see cref="DispatcherObject" />.</param>
    /// <param name="prio">
    /// Die Priorität des zugrundeliegenden <see cref="Dispatcher" />s
    /// von <paramref name="dispObj" />.
    /// </param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionState">
    /// Der zweite Parameter für <paramref name="action" />.
    /// </param>
    /// <returns>Der zugrundeliegende <see cref="DispatcherOperation" />-Kontext.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dispObj" /> und/oder <paramref name="action" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static DispatcherOperation BeginInvokeSafe<TDispatcher, T>(this TDispatcher dispObj,
                                                                      DispatcherPriority prio,
                                                                      Action<TDispatcher, T> action,
                                                                      T actionState)
        where TDispatcher : global::System.Windows.Threading.DispatcherObject
    {
        return BeginInvokeSafe<TDispatcher, T>(dispObj,
                                               prio,
                                               action,
                                               (ctrl) => actionState);
    }

    /// <summary>
    /// Wrapper für die Methode <see cref="Dispatcher.BeginInvoke(Delegate, DispatcherPriority, object[])" />
    /// mit stark typisiertem state-Objekt.
    /// </summary>
    /// <typeparam name="TDispatcher">Typ des zugrundeliegenden <see cref="DispatcherObject" />.</typeparam>
    /// <typeparam name="T">Typ des zweiten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="dispObj">Das zugrundeliegende <see cref="DispatcherObject" />.</param>
    /// <param name="prio">
    /// Die Priorität des zugrundeliegenden <see cref="Dispatcher" />s
    /// von <paramref name="dispObj" />.
    /// </param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionStateFactory">
    /// Die Funktion / Methode, die den zweiten Parameter für <paramref name="action" /> liefert.
    /// </param>
    /// <returns>Der zugrundeliegende <see cref="DispatcherOperation" />-Kontext.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dispObj" />, <paramref name="action" /> und/oder
    /// <paramref name="actionStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static DispatcherOperation BeginInvokeSafe<TDispatcher, T>(this TDispatcher dispObj,
                                                                      DispatcherPriority prio,
                                                                      Action<TDispatcher, T> action,
                                                                      Func<TDispatcher, T> actionStateFactory)
        where TDispatcher : global::System.Windows.Threading.DispatcherObject
    {
        if (dispObj == null)
        {
            throw new ArgumentNullException("dispObj");
        }

        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        if (actionStateFactory == null)
        {
            throw new ArgumentNullException("actionStateFactory");
        }

        var actionToInvoke = new Action(
            () =>
            {
                action(dispObj,
                       actionStateFactory(dispObj));
            });

        var dispatcher = dispObj.Dispatcher;
        if (dispatcher != null)
        {
            return dispatcher.BeginInvoke(actionToInvoke,
                                          prio);
        }
        else
        {
            actionToInvoke();
            return null;
        }
    }

    #endregion Methods
}
