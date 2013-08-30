// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;

/// <summary>
/// Extension Methoden für Datenbank-Operationen.
/// </summary>
public static partial class __DataExtensionMethods
{
    #region Methods (11)

    // Public Methods (11) 

    /// <summary>
    /// Erzeugt eine Sequenz für ein <see cref="IDataReader" /> Objekt, um
    /// dessen Resultat / Zeilen in einer foreach-Schleife verwenden zu können.
    /// </summary>
    /// <param name="reader">Der Reader, der die Daten bereitstellt.</param>
    /// <returns>
    /// Ein Sequenz mit verzögerter Ausführung, die bei einem foreach Durchlauf
    /// Schritt-für-Schritt jede einzelne Zeile zurückgibt.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static IEnumerable<IDataRecord> AsEnumerable(this IDataReader reader)
    {
        return AsEnumerable<IDataRecord>(reader);
    }

    /// <summary>
    /// Erzeugt eine Sequenz für ein <see cref="SqlDataReader" /> Objekt, um
    /// dessen Resultat / Zeilen in einer foreach-Schleife verwenden zu können.
    /// </summary>
    /// <param name="sqlReader">Der Reader, der die Daten bereitstellt.</param>
    /// <returns>
    /// Ein Sequenz mit verzögerter Ausführung, die bei einem foreach Durchlauf
    /// Schritt-für-Schritt jede einzelne Zeile zurückgibt.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="sqlReader" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static IEnumerable<SqlDataReader> AsEnumerable(this SqlDataReader sqlReader)
    {
        return AsEnumerable<SqlDataReader>(sqlReader);
    }

    /// <summary>
    /// Erzeugt eine Sequenz für ein <see cref="DbDataReader" /> Objekt, um
    /// dessen Resultat / Zeilen in einer foreach-Schleife verwenden zu können.
    /// </summary>
    /// <param name="dbReader">Der Reader, der die Daten bereitstellt.</param>
    /// <returns>
    /// Ein Sequenz mit verzögerter Ausführung, die bei einem foreach Durchlauf
    /// Schritt-für-Schritt jede einzelne Zeile zurückgibt.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dbReader" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static IEnumerable<DbDataRecord> AsEnumerable(this DbDataReader dbReader)
    {
        return AsEnumerable<DbDataRecord>(dbReader);
    }

    /// <summary>
    /// Erzeugt eine Sequenz für ein <see cref="OdbcDataReader" /> Objekt, um
    /// dessen Resultat / Zeilen in einer foreach-Schleife verwenden zu können.
    /// </summary>
    /// <param name="odbcReader">Der Reader, der die Daten bereitstellt.</param>
    /// <returns>
    /// Ein Sequenz mit verzögerter Ausführung, die bei einem foreach Durchlauf
    /// Schritt-für-Schritt jede einzelne Zeile zurückgibt.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="odbcReader" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static IEnumerable<OdbcDataReader> AsEnumerable(this OdbcDataReader odbcReader)
    {
        return AsEnumerable<OdbcDataReader>(odbcReader);
    }

    /// <summary>
    /// Erzeugt eine Sequenz für ein <see cref="OleDbDataReader" /> Objekt, um
    /// dessen Resultat / Zeilen in einer foreach-Schleife verwenden zu können.
    /// </summary>
    /// <param name="oleReader">Der Reader, der die Daten bereitstellt.</param>
    /// <returns>
    /// Ein Sequenz mit verzögerter Ausführung, die bei einem foreach Durchlauf
    /// Schritt-für-Schritt jede einzelne Zeile zurückgibt.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="oleReader" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static IEnumerable<OleDbDataReader> AsEnumerable(this OleDbDataReader oleReader)
    {
        return AsEnumerable<OleDbDataReader>(oleReader);
    }

    /// <summary>
    /// Erzeugt eine Sequenz für ein <see cref="IDataReader" />-verwantes Objekt, um
    /// dessen Resultat / Zeilen in einer foreach-Schleife verwenden zu können.
    /// </summary>
    /// <param name="reader">Der Reader, der die Daten bereitstellt.</param>
    /// <returns>
    /// Ein Sequenz mit verzögerter Ausführung, die bei einem foreach Durchlauf
    /// Schritt-für-Schritt jede einzelne Zeile zurückgibt.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static IEnumerable<TRec> AsEnumerable<TRec>(this IDataReader reader)
        where TRec : global::System.Data.IDataRecord
    {
        if (reader == null)
        {
            throw new ArgumentNullException("reader");
        }

        while (reader.Read())
        {
            yield return (TRec)reader;
        }
    }

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

    /// <summary>
    /// Wandelt sämtliche Werte eines <see cref="IDataRecord" /> in ein
    /// <see cref="Dictionary{TKey, TValue}" /> um.
    /// </summary>
    /// <param name="rec">Die Zeile mit den Daten.</param>
    /// <returns>Die umgewandelten / extrahierten Daten aus <paramref name="rec" />.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="rec" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static Dictionary<string, object> ToDictionary(this IDataRecord rec)
    {
        if (rec == null)
        {
            throw new ArgumentNullException("rec");
        }

        var result = new Dictionary<string, object>();

        for (var i = 0; i < rec.FieldCount; i++)
        {
            result.Add(rec.GetName(i) ?? string.Empty,
                       rec.IsDBNull(i) ? null : rec.GetValue(i));
        }

        return result;
    }

    /// <summary>
    /// Wandelt sämtliche Zeilen eines <see cref="IDataReader" /> in eine Sequenz von
    /// <see cref="Dictionary{TKey, TValue}" />s mit verzögerter Ausführung um.
    /// </summary>
    /// <param name="reader">Der Reader, der die Daten / Zeilen bereitstellt.</param>
    /// <returns>Die umgewandelten / extrahierten Daten aus <paramref name="reader" />.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static IEnumerable<Dictionary<string, object>> ToDictionaryEnumerable(this IDataReader reader)
    {
        if (reader == null)
        {
            throw new ArgumentNullException("reader");
        }

        while (reader.Read())
        {
            yield return ToDictionary(reader);
        }
    }

    #endregion Methods
}
