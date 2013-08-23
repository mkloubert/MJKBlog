// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FreeImageAPI;

/// <summary>
/// Extension Methoden für Bild(-bearbeitungs)operationen.
/// </summary>
public static partial class __DrawingExtensionMethods
{
    #region Methods (3)

    // Public Methods (3) 

    /// <summary>
    /// Lädt ein <see cref="Bitmap" /> aus Binärdaten.
    /// </summary>
    /// <param name="data">Der Daten, die das Bild repräsentieren.</param>
    /// <returns>Das geladene Bild.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="data" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static Bitmap LoadBitmap(this IEnumerable<byte> data)
    {
        if (data == null)
        {
            throw new ArgumentNullException("data");
        }

        using (var stream = new MemoryStream(data is byte[] ? (byte[])data : data.ToArray(), false))
        {
            return LoadBitmap(stream);
        }
    }

    /// <summary>
    /// Lädt ein <see cref="Bitmap" /> aus einer Datei.
    /// </summary>
    /// <param name="file">Der Datei aus der das Bild gelesen werden soll.</param>
    /// <returns>Das geladene Bild.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="file" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// <paramref name="file" /> verweist auf eine Datei, die anscheinend nicht existiert.
    /// </exception>
    public static Bitmap LoadBitmap(this FileInfo file)
    {
        if (file == null)
        {
            throw new ArgumentNullException("file");
        }

        if (!file.Exists)
        {
            throw new FileNotFoundException();
        }

        using (var stream = file.OpenRead())
        {
            return LoadBitmap(stream);
        }
    }

    /// <summary>
    /// Lädt ein <see cref="Bitmap" /> aus einem <see cref="Stream" />.
    /// </summary>
    /// <param name="stream">Der Eingabestream.</param>
    /// <returns>Das geladene Bild.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="stream" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="stream" /> ist nicht lesbar.
    /// </exception>
    public static Bitmap LoadBitmap(this Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }

        if (!stream.CanRead)
        {
            throw new IOException("stream.CanRead");
        }

        FIBITMAP? dib = null;

        try
        {
            dib = FreeImage.LoadFromStream(stream);

            return FreeImage.GetBitmap(dib.Value);
        }
        finally
        {
            if (dib.HasValue)
            {
                FreeImage.Unload(dib.Value);
            }
        }
    }

    #endregion Methods
}
