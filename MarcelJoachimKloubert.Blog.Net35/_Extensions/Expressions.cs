// s. http://blog.marcel-kloubert.de


using System;
using System.Linq.Expressions;

/// <summary>
/// Extension Methoden für LINQ Expressions.
/// </summary>
public static partial class __ExpressionExtensionMethods
{
    #region Methods (2)

    // Public Methods (2) 

    /// <summary>
    /// Gibt den Namen eines bestimmten Members eines Objektes mittels eines
    /// LINQ-Ausdrucks zurück.
    /// </summary>
    /// <typeparam name="O">Typ des zugrundeliegenden Objektes.</typeparam>
    /// <param name="obj">Der Objekt als Grundlage für <paramref name="expr" />.</param>
    /// <param name="expr">Der Ausdruck, der den Member enthält dessen Name ermittelt werden soll.</param>
    /// <returns>Der ermittelte Name des Members aus <paramref name="expr" />.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="expr" /> enthält keine <see cref="MemberExpression" />, um
    /// den Namen zu ermitteln.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="expr" /> ist <see langword="null" />.
    /// </exception>
    public static string GetMemberName<O>(this O obj, Expression<Action<O>> expr)
    {
        if (expr == null)
        {
            throw new ArgumentNullException("expr");
        }

        var memberExpr = expr.Body as MemberExpression;
        if (memberExpr == null)
        {
            throw new ArgumentException("expr.Body");
        }

        return memberExpr.Member.Name;
    }

    /// <summary>
    /// Gibt den Namen eines bestimmten Members eines Objektes mittels eines
    /// LINQ-Ausdrucks zurück.
    /// </summary>
    /// <typeparam name="O">Typ des zugrundeliegenden Objektes.</typeparam>
    /// <typeparam name="R">Rückgabetyp des Members.</typeparam>
    /// <param name="obj">Der Objekt als Grundlage für <paramref name="expr" />.</param>
    /// <param name="expr">Der Ausdruck, der den Member enthält dessen Name ermittelt werden soll.</param>
    /// <returns>Der ermittelte Name des Members aus <paramref name="expr" />.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="expr" /> enthält keine <see cref="MemberExpression" />, um
    /// den Namen zu ermitteln.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="expr" /> ist <see langword="null" />.
    /// </exception>
    public static string GetMemberName<O, R>(this O obj, Expression<Func<O, R>> expr)
    {
        if (expr == null)
        {
            throw new ArgumentNullException("expr");
        }

        var memberExpr = expr.Body as MemberExpression;
        if (memberExpr == null)
        {
            throw new ArgumentException("expr.Body");
        }

        return memberExpr.Member.Name;
    }

    #endregion Methods
}
