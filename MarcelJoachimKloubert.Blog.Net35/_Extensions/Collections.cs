// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Extension Methoden für Sequenzen und Listen.
/// </summary>
public static partial class __CollectionExtensionMethods
{
    #region Methods (5)

    // Public Methods (5) 

    /// <summary>
    /// Fügt eine Liste von Elementen einer
    /// <see cref="ICollection{T}" /> hinzu.
    /// </summary>
    /// <typeparam name="C">Typ der Collection.</typeparam>
    /// <typeparam name="T">Typ der Elemente.</typeparam>
    /// <param name="coll">Die Collection.</param>
    /// <param name="seq">
    /// Die Sequenz mit Elementen, die hinzugefügt werden sollen.
    /// </param>
    /// <returns><paramref name="coll" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="coll" /> und/oder <paramref name="seq" />
    /// ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static C AddRange<C, T>(this C coll, IEnumerable<T> seq)
        where C : global::System.Collections.Generic.ICollection<T>
    {
        if (coll == null)
        {
            throw new ArgumentNullException("coll");
        }

        var list = coll as List<T>;
        if (list != null)
        {
            list.AddRange(seq);
        }
        else
        {
            if (seq == null)
            {
                throw new ArgumentNullException("seq");
            }

            using (var enumerator = seq.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    coll.Add(enumerator.Current);
                }
            }
        }

        return coll;
    }

    /// <summary>
    /// Fügt eine Liste von Elementen einer
    /// <see cref="ICollection{T}" /> hinzu.
    /// </summary>
    /// <typeparam name="C">Typ der Collection.</typeparam>
    /// <typeparam name="T">Typ der Elemente.</typeparam>
    /// <param name="coll">Die Collection.</param>
    /// <param name="items">
    /// Die Sequenz mit Elementen, die hinzugefügt werden sollen.
    /// </param>
    /// <returns><paramref name="coll" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="coll" /> und/oder <paramref name="items" />
    /// ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static C AddRange<C, T>(this C coll, params T[] items)
        where C : global::System.Collections.Generic.ICollection<T>
    {
        return AddRange<C, T>(coll, (IEnumerable<T>)items);
    }

    /// <summary>
    /// Castet eine Sequenz in ein Array.
    /// </summary>
    /// <typeparam name="T">Typ der Elemente.</typeparam>
    /// <param name="seq">Die Sequenz, die gecastet werden soll.</param>
    /// <returns>
    /// <paramref name="seq" /> als Array oder <see langword="null" />, wenn
    /// <paramref name="seq" /> ebenfalls <see langword="null" /> ist.
    /// </returns>
    /// <remarks>
    /// Sofern <paramref name="seq" /> bereits ein Array ist, wird es
    /// lediglich gecastet, d.h. das Original-Objekt in <paramref name="seq" />
    /// wird zurückgegeben.
    /// </remarks>
    public static T[] AsArray<T>(this IEnumerable<T> seq)
    {
        if (seq == null)
        {
            return null;
        }

        if (seq is T[])
        {
            return (T[])seq;
        }

        var list = seq as List<T>;
        if (list != null)
        {
            return list.ToArray();
        }

        return seq.ToArray();
    }

    /// <summary>
    /// Führt eine <see cref="Action{T}" /> für
    /// jedes Element einer Sequenz aus.
    /// </summary>
    /// <typeparam name="T">Typ der Elemente.</typeparam>
    /// <param name="seq">The Sequenz mit Elementen.</param>
    /// <param name="action">
    /// The Logik, die für jedes Element ausgeführt wird.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="seq" /> und/oder <paramref name="action" />
    /// ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <remarks>
    /// Das Ausführen wird unterbrochen, wenn mindestens eine
    /// <see cref="Exception" /> geworfen wird.
    /// </remarks>
    public static void ForEach<T>(this IEnumerable<T> seq, Action<T> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        var list = seq as List<T>;
        if (list != null)
        {
            list.ForEach(action);
        }
        else
        {
            ForEach<T, object>(seq,
                               (item, actionState) => action(item),
                               null);
        }
    }

    /// <summary>
    /// Führt eine <see cref="Action{T1, T2}" /> für
    /// jedes Element einer Sequenz aus mit einem zusätzlichen Statusobjekt.
    /// </summary>
    /// <typeparam name="T">typ der Elemente.</typeparam>
    /// <typeparam name="S">
    /// Typ des Objektes für das 2. Argument für <paramref name="action" />.
    /// </typeparam>
    /// <param name="seq">The Sequenz mit Elementen.</param>
    /// <param name="action">
    /// The Logik, die für jedes Element ausgeführt wird.
    /// </param>
    /// <param name="actionState">
    /// Das 2. Argument (Statusobjekt) für <paramref name="action" />.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="seq" /> und/oder <paramref name="action" />
    /// ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <remarks>
    /// Das Ausführen wird unterbrochen, wenn mindestens eine
    /// <see cref="Exception" /> geworfen wird.
    /// </remarks>
    public static void ForEach<T, S>(this IEnumerable<T> seq, Action<T, S> action, S actionState)
    {
        if (seq == null)
        {
            throw new ArgumentNullException("seq");
        }

        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        using (var enumerator = seq.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                action(enumerator.Current,
                       actionState);
            }
        }
    }

    #endregion Methods
}
