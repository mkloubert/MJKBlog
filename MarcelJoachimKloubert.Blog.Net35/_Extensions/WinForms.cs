// s. http://blog.marcel-kloubert.de


using System;
using System.Windows.Forms;

/// <summary>
/// Extension Methoden für Windows Forms.
/// </summary>
public static partial class __WinFormsExtensionMethods
{
    #region Methods (3)

    // Public Methods (3) 

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
                                  null);
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
        if (ctrl == null)
        {
            throw new ArgumentNullException("ctrl");
        }

        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        if (actionStateFactory == null)
        {
            throw new ArgumentNullException("actionStateFactory");
        }

        if (ctrl.InvokeRequired)
        {
            ctrl.Invoke(new Action<TCtrl, Action<TCtrl, T>, Func<TCtrl, T>>(InvokeSafe<TCtrl, T>),
                        new object[] { ctrl, action, actionStateFactory });
        }
        else
        {
            action(ctrl,
                   actionStateFactory(ctrl));
        }
    }

    #endregion Methods
}
