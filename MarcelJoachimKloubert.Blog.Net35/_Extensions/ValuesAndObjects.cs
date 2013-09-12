// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

/// <summary>
/// Extension Methoden für Werte und Objekte.
/// </summary>
public static partial class __ValueAndObjectExtensionMethods
{
    #region Methods (6)

    // Public Methods (6) 

    /// <summary>
    /// Gibt die Zeichenketten-Repräsentation eines Objektes zurück, wobei
    /// <see cref="DBNull" />-Objekte automatisch wie eine 
    /// <see langword="null" /> Referenz behandelt werden.
    /// </summary>
    /// <param name="obj">
    /// Das Objekt dessen Zeichenketten-Repräsentation
    /// zurückgegeben werden soll.
    /// </param>
    /// <remarks>
    /// Ist <paramref name="obj" /> bereits ein <see cref="string" /> wird der Wert
    /// lediglich gecastet.
    /// </remarks>
    public static string AsString(this object obj)
    {
        return AsString(obj, true);
    }

    /// <summary>
    /// Gibt die Zeichenketten-Repräsentation eines Objektes zurück.
    /// </summary>
    /// <param name="obj">
    /// Das Objekt dessen Zeichenketten-Repräsentation
    /// zurückgegeben werden soll.
    /// </param>
    /// <param name="handleDBNullAsNull">
    /// <see cref="DBNull" />-Objekte als <see langword="null" /> Referenz
    /// behandeln oder nicht.
    /// </param>
    /// <remarks>
    /// Ist <paramref name="obj" /> bereits ein <see cref="string" /> wird der Wert
    /// lediglich gecastet.
    /// </remarks>
    public static string AsString(this object obj, bool handleDBNullAsNull)
    {
        if (obj == null)
        {
            return null;
        }

        if (obj is string)
        {
            return (string)obj;
        }

        if (handleDBNullAsNull &&
            DBNull.Value.Equals(obj))
        {
            return null;
        }

        var charSequence = obj as IEnumerable<char>;
        if (charSequence != null)
        {
            if (charSequence is char[])
            {
                return new string((char[])charSequence);
            }

            return new string(charSequence.ToArray());
        }

        var xmlNode = obj as XmlNode;
        if (xmlNode != null)
        {
            return xmlNode.OuterXml;
        }

        var reader = obj as TextReader;
        if (reader != null)
        {
            return reader.ReadToEnd();
        }

        return obj.ToString();
    }

    /// <summary>
    /// Prüft, ob ein String eine <see langword="null" /> Referenz ist
    /// oder nur aus Leerzeichen (WhiteSpaces) besteht.
    /// </summary>
    /// <param name="chars">Die zu prüfende Sequenz.</param>
    /// <returns>
    /// Ist eine <see langword="null" /> Referenz bzw. besteht nur
    /// aus Leerzeichen (WhiteSpaces) oder nicht.
    /// </returns>
    public static bool IsNullOrWhiteSpace(this IEnumerable<char> chars)
    {
        return chars == null ||
               chars.All(c => char.IsWhiteSpace(c));
    }

    /// <summary>
    /// Prüft, ob der Wert eines Parameters eine <see langword="null" /> Referenz ist
    /// und wirft in diesem Fall eine <see cref="ArgumentNullException" />.
    /// </summary>
    /// <typeparam name="T">Typ des zu prüfenden Wertes.</typeparam>
    /// <param name="value">Der zu prüfende Wert.</param>
    /// <param name="paramExpr">Der Name des Parameters als LINQ-Ausdruck.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="paramExpr" /> ist kein Ausdruck, der ein Feld bzw. einen
    /// Parameter repräsentiert.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="NullReferenceException">
    /// <paramref name="paramExpr" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static void ThrowIfParamIsNull<T>(this T? value,
                                             Expression<Func<T?>> paramExpr)
        where T : struct
    {
        if (paramExpr == null)
        {
            throw new ArgumentException("paramExpr");
        }

        if (value.HasValue)
        {
            return;
        }

        var memberExpr = paramExpr.Body as MemberExpression;
        if (memberExpr == null)
        {
            throw new ArgumentException("paramExpr.Body");
        }

        var fieldExpr = memberExpr.Member as FieldInfo;
        if (fieldExpr == null)
        {
            throw new ArgumentException("paramExpr.Body.Member");
        }

        throw new ArgumentNullException(fieldExpr.Name);
    }

    /// <summary>
    /// Prüft, ob der Wert eines Parameters eine <see langword="null" /> Referenz ist
    /// und wirft in diesem Fall eine <see cref="ArgumentNullException" />.
    /// </summary>
    /// <typeparam name="T">Typ der zu prüfenden Objektes.</typeparam>
    /// <param name="obj">Das zu prüfende Objekt.</param>
    /// <param name="paramExpr">Der Name des Parameters als LINQ-Ausdruck.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="paramExpr" /> ist kein Ausdruck, der ein Feld bzw. einen
    /// Parameter repräsentiert.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="obj" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="NullReferenceException">
    /// <paramref name="paramExpr" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <remarks>
    /// <see cref="DBNull" /> Objekte werden als <see langword="null" /> Referenzn
    /// behandelt.
    /// </remarks>
    public static void ThrowIfParamIsNull<T>(this T obj,
                                             Expression<Func<T>> paramExpr)
        where T : class
    {
        ThrowIfParamIsNull<T>(obj, paramExpr, true);
    }

    /// <summary>
    /// Prüft, ob der Wert eines Parameters eine <see langword="null" /> Referenz ist
    /// und wirft in diesem Fall eine <see cref="ArgumentNullException" />.
    /// </summary>
    /// <typeparam name="T">Typ der zu prüfenden Objektes.</typeparam>
    /// <param name="obj">Das zu prüfende Objekt.</param>
    /// <param name="paramExpr">Der Name des Parameters als LINQ-Ausdruck.</param>
    /// <param name="handleDbNullAsNull">
    /// Behandel <see cref="DBNull" /> Objekte als <see langword="null" /> Referenz
    /// oder nicht.
    /// </param>
    /// <exception cref="ArgumentException">
    /// <paramref name="paramExpr" /> ist kein Ausdruck, der ein Feld bzw. einen
    /// Parameter repräsentiert.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="obj" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="NullReferenceException">
    /// <paramref name="paramExpr" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static void ThrowIfParamIsNull<T>(this T obj,
                                             Expression<Func<T>> paramExpr,
                                             bool handleDbNullAsNull)
        where T : class
    {
        if (paramExpr == null)
        {
            throw new NullReferenceException("paramExpr");
        }

        object value = obj;
        if (handleDbNullAsNull &&
            DBNull.Value.Equals(value))
        {
            value = null;
        }

        if (value != null)
        {
            return;
        }

        var memberExpr = paramExpr.Body as MemberExpression;
        if (memberExpr == null)
        {
            throw new ArgumentException("paramExpr.Body");
        }

        var fieldExpr = memberExpr.Member as FieldInfo;
        if (fieldExpr == null)
        {
            throw new ArgumentException("paramExpr.Body.Member");
        }

        throw new ArgumentNullException(fieldExpr.Name);
    }

    #endregion Methods
}
