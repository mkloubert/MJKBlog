// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

/// <summary>
/// Extension Methoden für Reflection-Operationen in .NET 4.5.
/// </summary>
public static partial class __ReflectionExtensionMethodsNet45
{
    #region Methods (3)

    // Public Methods (2) 

    /// <summary>
    /// Wrapper für <see cref="Assembly.GetManifestResourceStream(Type, string)" />, welcher die
    /// Streamdaten, wenn möglich, als <see cref="Bitmap" /> mit Hilfe von FreeImage zurückgibt.
    /// </summary>
    /// <param name="asm">Das zugrundeliegende Assembly.</param>
    /// <param name="name">Der Name der Resource.</param>
    /// <returns>Das ausgelesene Bild oder <see langword="null" />, wenn die Resource nicht existiert.</returns>
    /// <exception cref="ArgumentException"><paramref name="name" /> besteht nur aus Leerezeichen.</exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="asm" /> und/oder <paramref name="name" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Bitmap GetManifestResourceBitmap(this Assembly asm, IEnumerable<char> name)
    {
        if (asm == null)
        {
            throw new ArgumentNullException("asm");
        }

        var stream = asm.GetManifestResourceStream(name.AsString());
        if (stream == null)
        {
            return null;
        }

        return stream.LoadBitmap();
    }

    /// <summary>
    /// Wrapper für <see cref="Assembly.GetManifestResourceStream(Type, string)" />, welcher die
    /// Streamdaten, wenn möglich, als <see cref="Bitmap" /> mit Hilfe von FreeImage zurückgibt.
    /// </summary>
    /// <param name="asm">Das zugrundeliegende Assembly.</param>
    /// <param name="name">Der Name der Resource.</param>
    /// <param name="type">Der Typ, dessen Namespace verwendet wird, um den Gültigkeitsbereich des Manifestressourcennamens festzulegen.</param>
    /// <returns>Das ausgelesene Bild oder <see langword="null" />, wenn die Resource nicht existiert.</returns>
    /// <exception cref="ArgumentException"><paramref name="name" /> besteht nur aus Leerezeichen.</exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="asm" />, <paramref name="type" /> und/oder <paramref name="name" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static Bitmap GetManifestResourceBitmap(this Assembly asm, Type type, IEnumerable<char> name)
    {
        if (type == null)
        {
            throw new ArgumentNullException("type");
        }

        return GetManifestResourceBitmap(asm,
                                         ToFullResourceName(type, name));
    }
    // Private Methods (1) 

    private static IEnumerable<char> ToFullResourceName(Type type, IEnumerable<char> name)
    {
        var @namespace = type.Namespace;
        if (!string.IsNullOrEmpty(@namespace))
        {
            return string.Format("{0}{1}{2}",
                                 @namespace,
                                 Type.Delimiter,
                                 name.AsString());
        }

        return name;
    }

    #endregion Methods
}
