// s. http://blog.marcel-kloubert.de


using System;
using System.Windows.Forms;

/// <summary>
/// Extension Methoden für Windows Forms.
/// </summary>
public static partial class __WinFormsExtensionMethods
{
    #region Methods (6)

    // Public Methods (6) 

    /// <summary>
    /// Führt Logik für ein WinForms <see cref="Control" /> Thread-sicher aus.
    /// </summary>
    /// <typeparam name="TCtrl">Typ des Controls.</typeparam>
    /// <param name="ctrl">Das zugrundeliegende Control.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="ctrl" /> und/oder <paramref name="action" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static void InvokeSafe<TCtrl>(this TCtrl ctrl, Action<TCtrl> action)
        where TCtrl : global::System.Windows.Forms.Control
    {
        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        InvokeSafe<TCtrl, object>(ctrl,
                                  (c, s) => action(c),
                                  (object)null);
    }

    /// <summary>
    /// Führt Logik für ein WinForms <see cref="Control" /> Thread-sicher aus.
    /// </summary>
    /// <typeparam name="TCtrl">Typ des Controls.</typeparam>
    /// <typeparam name="R">Typ der Rückgabe.</typeparam>
    /// <param name="ctrl">Das zugrundeliegende Control.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <returns>Rückgabe von <paramref name="func" />.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="ctrl" /> und/oder <paramref name="func" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static R InvokeSafe<TCtrl, R>(this TCtrl ctrl, Func<TCtrl, R> func)
        where TCtrl : global::System.Windows.Forms.Control
    {
        if (func == null)
        {
            throw new ArgumentNullException("func");
        }

        return InvokeSafe<TCtrl, object, R>(ctrl,
                                            (c, s) => func(c),
                                            (object)null);
    }

    /// <summary>
    /// Führt Logik für ein WinForms <see cref="Control" /> Thread-sicher aus.
    /// </summary>
    /// <typeparam name="TCtrl">Typ des Controls.</typeparam>
    /// <typeparam name="T">Typ des zweiten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="ctrl">Das zugrundeliegende Control.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionState">
    /// Der Wert für den zweiten Parameters von <paramref name="action" />
    /// generiert.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="ctrl" /> und/oder <paramref name="action" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static void InvokeSafe<TCtrl, T>(this TCtrl ctrl,
                                            Action<TCtrl, T> action,
                                            T actionState)
        where TCtrl : global::System.Windows.Forms.Control
    {
        InvokeSafe<TCtrl, T>(ctrl,
                             action,
                             (c) => actionState);
    }

    /// <summary>
    /// Führt Logik für ein WinForms <see cref="Control" /> Thread-sicher aus.
    /// </summary>
    /// <typeparam name="TCtrl">Typ des Controls.</typeparam>
    /// <typeparam name="T">Typ des zweiten Parameters von <paramref name="action" />.</typeparam>
    /// <param name="ctrl">Das zugrundeliegende Control.</param>
    /// <param name="action">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="actionStateFactory">
    /// Die Methode/Funktion, die den zweiten Parameters für <paramref name="action" />
    /// generiert.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="ctrl" />, <paramref name="action" /> und/oder
    /// <paramref name="actionStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static void InvokeSafe<TCtrl, T>(this TCtrl ctrl,
                                            Action<TCtrl, T> action,
                                            Func<TCtrl, T> actionStateFactory)
        where TCtrl : global::System.Windows.Forms.Control
    {
        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        if (actionStateFactory == null)
        {
            throw new ArgumentNullException("actionStateFactory");
        }

        InvokeSafe<TCtrl, T, object>(ctrl,
                                     (c, s) =>
                                     {
                                         action(c, s);
                                         return null;
                                     },
                                     actionStateFactory);
    }

    /// <summary>
    /// Führt Logik für ein WinForms <see cref="Control" /> Thread-sicher aus.
    /// </summary>
    /// <typeparam name="TCtrl">Typ des Controls.</typeparam>
    /// <typeparam name="T">Typ des zweiten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Typ der Rückgabe.</typeparam>
    /// <param name="ctrl">Das zugrundeliegende Control.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcState">
    /// Der Wert für den zweiten Parameters von <paramref name="func" />
    /// generiert.
    /// </param>
    /// <returns>Rückgabe von <paramref name="func" />.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="ctrl" /> und/oder <paramref name="func" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static R InvokeSafe<TCtrl, T, R>(this TCtrl ctrl,
                                            Func<TCtrl, T, R> func,
                                            T funcState)
        where TCtrl : global::System.Windows.Forms.Control
    {
        return InvokeSafe<TCtrl, T, R>(ctrl,
                                       func,
                                       (c) => funcState);
    }

    /// <summary>
    /// Führt Logik für ein WinForms <see cref="Control" /> Thread-sicher aus.
    /// </summary>
    /// <typeparam name="TCtrl">Typ des Controls.</typeparam>
    /// <typeparam name="T">Typ des zweiten Parameters von <paramref name="func" />.</typeparam>
    /// <typeparam name="R">Typ der Rückgabe.</typeparam>
    /// <param name="ctrl">Das zugrundeliegende Control.</param>
    /// <param name="func">Die Logik, die ausgeführt werden soll.</param>
    /// <param name="funcStateFactory">
    /// Die Methode/Funktion, die den zweiten Parameters für <paramref name="func" />
    /// generiert.
    /// </param>
    /// <returns>Rückgabe von <paramref name="func" />.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="ctrl" />, <paramref name="func" /> und/oder
    /// <paramref name="funcStateFactory" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static R InvokeSafe<TCtrl, T, R>(this TCtrl ctrl,
                                            Func<TCtrl, T, R> func,
                                            Func<TCtrl, T> funcStateFactory)
        where TCtrl : global::System.Windows.Forms.Control
    {
        if (ctrl == null)
        {
            throw new ArgumentNullException("ctrl");
        }

        if (func == null)
        {
            throw new ArgumentNullException("func");
        }

        if (funcStateFactory == null)
        {
            throw new ArgumentNullException("funcStateFactory");
        }

        if (ctrl.InvokeRequired)
        {
            return (R)ctrl.Invoke(new Func<TCtrl, Func<TCtrl, T, R>, Func<TCtrl, T>, R>(InvokeSafe<TCtrl, T, R>),
                                  new object[] { ctrl, func, funcStateFactory });
        }

        return (R)func(ctrl,
                       funcStateFactory(ctrl));
    }

    #endregion Methods
}
