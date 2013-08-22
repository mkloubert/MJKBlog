// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Extension Methoden für Datenbank-Operationen.
/// </summary>
public static partial class __DataExtensionMethods
{
    #region Methods (3)

    // Public Methods (3) 

    /// <summary>
    /// Gibt die typisierte Version eines Wertes aus einem
    /// <see cref="IDataRecord" /> zurück und prüft dabei automatisch
    /// auf <see cref="DBNull" />.
    /// </summary>
    /// <typeparam name="T">Zieltyp.</typeparam>
    /// <param name="rec">Die zugrundeliegende Zeile.</param>
    /// <param name="name">Der Name der Spalte.</param>
    /// <returns>Der Wert.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rec" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static T GetDbValue<T>(this IDataRecord rec, IEnumerable<char> name)
    {
        if (rec == null)
        {
            throw new ArgumentNullException("rec");
        }

        return GetDbValue<T>(rec,
                             rec.GetOrdinal(name.AsString()));
    }

    /// <summary>
    /// Gibt die typisierte Version eines Wertes aus einem
    /// <see cref="IDataRecord" /> zurück und prüft dabei automatisch
    /// auf <see cref="DBNull" />.
    /// </summary>
    /// <typeparam name="T">Zieltyp.</typeparam>
    /// <param name="rec">Die zugrundeliegende Zeile.</param>
    /// <param name="ordinal">Der 0-basierende Spaltenindex.</param>
    /// <returns>Der Wert.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rec" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static T GetDbValue<T>(this IDataRecord rec, int ordinal)
    {
        if (rec == null)
        {
            throw new ArgumentNullException("rec");
        }

        return (T)(rec.IsDBNull(ordinal) ? null : rec.GetValue(ordinal));
    }

    /// <summary>
    /// Wandelt einen Wert in ein für eine Datenbankoperation gültiges Objekt um,
    /// bspw. wird eine <see langword="null" /> Referenz in ein <see cref="DBNull" />
    /// umgewandelt.
    /// </summary>
    /// <param name="value">Der Eingabewert.</param>
    /// <returns>Der Ausgabewert.</returns>
    public static object ToDbValue(this object value)
    {
        return value ?? DBNull.Value;
    }

    #endregion Methods
}
