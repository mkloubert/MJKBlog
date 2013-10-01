// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Xml.Linq;

/// <summary>
/// Extension Methoden für XML-Operationen.
/// </summary>
public static partial class __XmlExtensionMethods
{
    #region Methods (1)

    // Public Methods (1) 

    /// <summary>
    /// Gibt die Attribute eines <see cref="XElement" /> als Wörterbuch zurück.
    /// </summary>
    /// <param name="element">
    /// Das Element aus dem die Attribute (und deren Werte) extrahiert werden soll.
    /// </param>
    /// <returns>Die extrahierten Attribute.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static Dictionary<string, string> GetAttributeDictionary(this XElement element)
    {
        if (element == null)
        {
            throw new ArgumentNullException("element");
        }

        var result = new Dictionary<string, string>();

        foreach (var attrib in element.Attributes())
        {
            result.Add(attrib.Name.ToString(),
                       attrib.Value);
        }

        return result;
    }

    #endregion Methods
}
