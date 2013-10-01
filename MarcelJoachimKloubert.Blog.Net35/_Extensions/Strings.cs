// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

/// <summary>
/// Extension Methoden für Zeichenketten.
/// </summary>
public static partial class __StringExtensionMethods
{
    #region Methods (5)

    // Public Methods (5) 

    /// <summary>
    /// Berecht die Ähnlichkeit zweier Zeichenketten nach dem
    /// Levenshtein-Distanz Algorithmus.
    /// </summary>
    /// <param name="left">Die erste Zeichenkette.</param>
    /// <param name="right">Die zweite Zeichenkette.</param>
    /// <returns>Die Ähnlichkeit zwischen 0 (0%) und 1 (100%).</returns>
    public static double CalcSimilarity(this IEnumerable<char> left,
                                        IEnumerable<char> right)
    {
        return CalcSimilarity(left, right, false);
    }

    /// <summary>
    /// Berecht die Ähnlichkeit zweier Zeichenketten nach dem
    /// Levenshtein-Distanz Algorithmus.
    /// </summary>
    /// <param name="left">Die erste Zeichenkette.</param>
    /// <param name="right">Die zweite Zeichenkette.</param>
    /// <param name="ignoreCase">Groß- und Kleinschreibung ignorieren oder nicht.</param>
    /// <returns>Die Ähnlichkeit zwischen 0 (0%) und 1 (100%).</returns>
    public static double CalcSimilarity(this IEnumerable<char> left,
                                        IEnumerable<char> right,
                                        bool ignoreCase)
    {
        return CalcSimilarity(left, right, ignoreCase, false);
    }

    /// <summary>
    /// Berecht die Ähnlichkeit zweier Zeichenketten nach dem
    /// Levenshtein-Distanz Algorithmus.
    /// </summary>
    /// <param name="charsLeft">Die erste Zeichenkette.</param>
    /// <param name="charsRight">Die zweite Zeichenkette.</param>
    /// <param name="ignoreCase">Groß- und Kleinschreibung ignorieren oder nicht.</param>
    /// <param name="trim">Leerzeichen am Anfang und Ende entfernen oder nicht.</param>
    /// <returns>Die Ähnlichkeit zwischen 0 (0%) und 1 (100%).</returns>
    public static double CalcSimilarity(this IEnumerable<char> charsLeft,
                                        IEnumerable<char> charsRight,
                                        bool ignoreCase,
                                        bool trim)
    {
        if (charsLeft == null &&
            charsRight == null)
        {
            return 1.0f;
        }

        if (charsLeft == null ^
            charsRight == null)
        {
            return 0.0f;
        }

        var left = charsLeft.AsString();
        {
            if (ignoreCase)
            {
                left = left.ToLower();
            }

            if (trim)
            {
                left = left.Trim();
            }
        }

        var right = charsRight.AsString();
        {
            if (ignoreCase)
            {
                right = right.ToLower();
            }

            if (trim)
            {
                right = right.Trim();
            }
        }

        if (left == right)
        {
            return 1.0f;
        }

        var n = left.Length;
        var m = right.Length;
        var d = new int[n + 1, m + 1];

        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        for (var i = 0; i <= n; d[i, 0] = i++)
        { }

        for (var j = 0; j <= m; d[0, j] = j++)
        { }

        for (var i = 1; i <= n; i++)
        {
            for (var j = 1; j <= m; j++)
            {
                var cost = (right[j - 1] == left[i - 1]) ? 0 : 1;

                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                                   d[i - 1, j - 1] + cost);
            }
        }

        return 1.0f - (double)d[n, m] /
                      (double)Math.Max(left.Length, right.Length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <see href="http://msdn.microsoft.com/de-de/library/system.string.isnullorwhitespace%28v=vs.100%29.aspx" />
    public static bool IsNullOrWhiteSpace(this IEnumerable<char> chars)
    {
        return chars == null ||
               chars.All(c => char.IsWhiteSpace(c));
    }

    /// <summary>
    /// Wandelt ein sicheres <see cref="SecureString" />-Objekt zurück
    /// in einen lesbaren <see cref="string" /> zurück.
    /// </summary>
    /// <param name="str">Der sichere String.</param>
    /// <returns>
    /// Die lesbare, unsichere String-Repräsentation oder
    /// <see langword="null" />, wenn <paramref name="str" /> ebenfalls
    /// eine <see langword="null" /> Referenz ist.
    /// </returns>
    public static string ToUnsecureString(this SecureString str)
    {
        if (str == null)
        {
            return null;
        }

        var ptr = IntPtr.Zero;
        try
        {
            ptr = Marshal.SecureStringToGlobalAllocUnicode(str);
            return Marshal.PtrToStringUni(ptr);
        }
        finally
        {
            Marshal.ZeroFreeGlobalAllocUnicode(ptr);
        }
    }

    #endregion Methods
}
