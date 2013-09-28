// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

/// <summary>
/// Extension Methoden für Reflection-Operationen.
/// </summary>
public static partial class __ReflectionExtensionMethods
{
    #region Methods (4)

    // Public Methods (4) 

    /// <summary>
    /// Wrapper für <see cref="Assembly.GetManifestResourceStream(string)" />, welcher die
    /// Streamdaten, wenn möglich, als UTF-8-String zurückgibt.
    /// </summary>
    /// <param name="asm">Das zugrundeliegende Assembly.</param>
    /// <param name="name">Der Name der Resource.</param>
    /// <returns>Der ausgelesene String oder <see langword="null" />, wenn die Resource nicht existiert.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="asm" /> und/oder <paramref name="name" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static string GetManifestResourceString(this Assembly asm, IEnumerable<char> name)
    {
        return GetManifestResourceString(asm, name, Encoding.UTF8);
    }

    /// <summary>
    /// Wrapper für <see cref="Assembly.GetManifestResourceStream(string)" />, welcher die
    /// Streamdaten, wenn möglich, String zurückgibt.
    /// </summary>
    /// <param name="asm">Das zugrundeliegende Assembly.</param>
    /// <param name="name">Der Name der Resource.</param>
    /// <param name="enc">Das <see cref="Encoding" />, das zum dekodieren benutzt werden soll.</param>
    /// <returns>Der ausgelesene String oder <see langword="null" />, wenn die Resource nicht existiert.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="asm" />, <paramref name="name" /> und/oder <paramref name="enc" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static string GetManifestResourceString(this Assembly asm, IEnumerable<char> name, Encoding enc)
    {
        if (asm == null)
        {
            throw new ArgumentNullException("asm");
        }

        if (enc == null)
        {
            throw new ArgumentNullException("enc");
        }

        var stream = asm.GetManifestResourceStream(name.AsString());
        if (stream == null)
        {
            return null;
        }

        using (stream)
        {
            using (var temp = new MemoryStream())
            {
                var buffer = new byte[81920];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    temp.Write(buffer, 0, bytesRead);
                }

                return enc.GetString(temp.ToArray());
            }
        }
    }

    /// <summary>
    /// Wrapper für <see cref="Assembly.GetManifestResourceStream(string)" />, welcher die
    /// Streamdaten, wenn möglich, UTF-8-String zurückgibt.
    /// </summary>
    /// <param name="asm">Das zugrundeliegende Assembly.</param>
    /// <param name="name">Der Name der Resource.</param>
    /// <param name="type">Der Typ, dessen Namespace verwendet wird, um den Gültigkeitsbereich des Manifestressourcennamens festzulegen.</param>
    /// <returns>Der ausgelesene String oder <see langword="null" />, wenn die Resource nicht existiert.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="asm" />, <paramref name="type" /> und/oder <paramref name="name" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static string GetManifestResourceString(this Assembly asm, Type type, IEnumerable<char> name)
    {
        return GetManifestResourceString(asm, type, name, Encoding.UTF8);
    }

    /// <summary>
    /// Wrapper für <see cref="Assembly.GetManifestResourceStream(string)" />, welcher die
    /// Streamdaten, wenn möglich, String zurückgibt.
    /// </summary>
    /// <param name="asm">Das zugrundeliegende Assembly.</param>
    /// <param name="name">Der Name der Resource.</param>
    /// <param name="type">Der Typ, dessen Namespace verwendet wird, um den Gültigkeitsbereich des Manifestressourcennamens festzulegen.</param>
    /// <param name="enc">Das <see cref="Encoding" />, das zum dekodieren benutzt werden soll.</param>
    /// <returns>Der ausgelesene String oder <see langword="null" />, wenn die Resource nicht existiert.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="asm" />, <paramref name="type" />, <paramref name="name" /> und/oder <paramref name="enc" />
    /// sind <see langword="null" /> Referenzen.
    /// </exception>
    public static string GetManifestResourceString(this Assembly asm, Type type, IEnumerable<char> name, Encoding enc)
    {
        if (type == null)
        {
            throw new ArgumentNullException("type");
        }

        var ns = type.Namespace;
        if (!string.IsNullOrEmpty(ns))
        {
            return GetManifestResourceString(asm,
                                             string.Format("{0}{1}{2}",
                                                           ns,
                                                           Type.Delimiter,
                                                           name.AsString()),
                                             enc);
        }

        return GetManifestResourceString(asm, name, enc);
    }

    #endregion Methods
}
