// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;

/// <summary>
/// Extension Methoden für Sequenzen und Listen (.NET 4.0).
/// </summary>
public static partial class __CollectionExtensionMethodsNet40
{
    #region Methods (3)

    // Public Methods (3) 

    /// <summary>
    /// Führt eine Action für jedes Element einer Sequenz synchron aus.
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
    public static AggregateException ForAll<T>(this IEnumerable<T> items,
                                               Action<T> action,
                                               bool throwExceptions = true)
    {
        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        return ForAll<T, object>(items: items,
                                 action: (i, s) => action(i),
                                 actionState: null,
                                 throwExceptions: throwExceptions);
    }

    /// <summary>
    /// Führt eine Action für jedes Element einer Sequenz synchron aus.
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
    public static AggregateException ForAll<T, S>(this IEnumerable<T> items,
                                                  Action<T, S> action,
                                                  S actionState,
                                                  bool throwExceptions = true)
    {
        return ForAll<T, S>(items: items,
                            action: action,
                            actionStateFactory: (i) => actionState,
                            throwExceptions: throwExceptions);
    }

    /// <summary>
    /// Führt eine Action für jedes Element einer Sequenz synchron aus.
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
    public static AggregateException ForAll<T, S>(this IEnumerable<T> items,
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

        try
        {
            using (var enumerator = items.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    try
                    {
                        var i = enumerator.Current;

                        action(i,
                               actionStateFactory(i));
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }

        AggregateException result = null;
        if (exceptions.Count > 0)
        {
            result = new AggregateException(exceptions);
        }

        if (result != null &&
            throwExceptions)
        {
            throw result;
        }

        return result;
    }

    #endregion Methods
}
