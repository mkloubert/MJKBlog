// s. http://blog.marcel-kloubert.de



using System;
using System.Data;

static partial class __DataExtensionMethods
{
    #region Methods (3)

    // Public Methods (3) 

    /// <summary>
    /// Erzeugt eine Instanz eines Typs aus einem <see cref="IDataRecord" /> Objekt.
    /// </summary>
    /// <typeparam name="TRec">Typ des <see cref="IDataRecord" /> Objekts.</typeparam>
    /// <typeparam name="TObj">Typ des zu erzeugenden Objektes.</typeparam>
    /// <param name="rec">Der Datensatz, der die Daten der aktuellen Zeile bereitstellt.</param>
    /// <param name="factory">Das Delegate, welches das Objekt erzeugt.</param>
    /// <returns>Das erzeugte Objekt.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rec" /> und/oder <paramref name="factory" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static TObj BuildObject<TRec, TObj>(this TRec rec,
                                               Func<TRec, TObj> factory)
        where TRec : global::System.Data.IDataRecord
    {
        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        return BuildObject<TRec, TObj, object>(rec,
                                               (r, s) => factory(r),
                                               null);
    }

    /// <summary>
    /// Erzeugt eine Instanz eines Typs aus einem <see cref="IDataRecord" /> Objekt.
    /// </summary>
    /// <typeparam name="TRec">Typ des <see cref="IDataRecord" /> Objekts.</typeparam>
    /// <typeparam name="TObj">Typ des zu erzeugenden Objektes.</typeparam>
    /// <typeparam name="TState">
    /// Typ des zusätzlichen Arguments für das <paramref name="factory" /> Delegate.
    /// </typeparam>
    /// <param name="rec">Der Datensatz, der die Daten der aktuellen Zeile bereitstellt.</param>
    /// <param name="factory">Das Delegate, welches das Objekt erzeugt.</param>
    /// <param name="factoryState">
    /// Das zusätzliche Argument für <paramref name="factory" />.
    /// </param>
    /// <returns>Das erzeugte Objekt.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rec" /> und/oder <paramref name="factory" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static TObj BuildObject<TRec, TObj, TState>(this TRec rec,
                                                       Func<TRec, TState, TObj> factory,
                                                       TState factoryState)
        where TRec : global::System.Data.IDataRecord
    {
        return BuildObject<TRec, TObj, TState>(rec,
                                               factory,
                                               (r) => factoryState);
    }

    /// <summary>
    /// Erzeugt eine Instanz eines Typs aus einem <see cref="IDataRecord" /> Objekt.
    /// </summary>
    /// <typeparam name="TRec">Typ des <see cref="IDataRecord" /> Objekts.</typeparam>
    /// <typeparam name="TObj">Typ des zu erzeugenden Objektes.</typeparam>
    /// <typeparam name="TState">
    /// Typ des zusätzlichen Arguments für das <paramref name="factory" /> Delegate.
    /// </typeparam>
    /// <param name="rec">Der Datensatz, der die Daten der aktuellen Zeile bereitstellt.</param>
    /// <param name="factory">Das Delegate, welches das Objekt erzeugt.</param>
    /// <param name="factoryStateFunc">
    /// Das Delegate, das das zusätzliche Argument für <paramref name="factory" /> liefert.
    /// </param>
    /// <returns>Das erzeugte Objekt.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rec" />, <paramref name="factory" /> und/oder
    /// <paramref name="factoryStateFunc" /> sind <see langword="null" /> Referenzen.
    /// </exception>
    public static TObj BuildObject<TRec, TObj, TState>(this TRec rec,
                                                       Func<TRec, TState, TObj> factory,
                                                       Func<TRec, TState> factoryStateFunc)
        where TRec : global::System.Data.IDataRecord
    {
        if (rec == null)
        {
            throw new ArgumentNullException("rec");
        }

        if (factory == null)
        {
            throw new ArgumentNullException("factory");
        }

        if (factoryStateFunc == null)
        {
            throw new ArgumentNullException("factoryStateFunc");
        }

        return factory(rec,
                       factoryStateFunc(rec));
    }

    #endregion Methods
}
