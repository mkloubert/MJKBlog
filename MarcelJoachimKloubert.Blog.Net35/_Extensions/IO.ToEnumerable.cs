// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

static partial class __IOExtensionMethods
{
    #region Methods (2)

    // Public Methods (2) 

    /// <summary>
    /// Gibt einen <see cref="Stream" /> als Sequenz zurück, der blockweise
    /// mit einer <see langword="foreach" /> Schleife gelesen werden kann.
    /// </summary>
    /// <param name="stream">Der Stream, der gelesen werden soll.</param>
    /// <returns>Die Sequenz der Daten von <paramref name="stream" /> mit verzögerter Ausführung.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="stream" /> ist <see langword="null" />.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="stream" /> kann nicht gelesen werden.
    /// </exception>
    /// <remarks>Die Blockgrösse beträgt 81920 Bytes.</remarks>
    public static IEnumerable<byte[]> ToEnumerable(this Stream stream)
    {
        return ToEnumerable(stream,
                            81920);
    }

    /// <summary>
    /// Gibt einen <see cref="Stream" /> als Sequenz zurück, der blockweise
    /// mit einer <see langword="foreach" /> Schleife gelesen werden kann.
    /// </summary>
    /// <param name="stream">Der Stream, der gelesen werden soll.</param>
    /// <param name="blockSize">Die Blockgrösse, in Bytes.</param>
    /// <returns>Die Sequenz der Daten von <paramref name="stream" /> mit verzögerter Ausführung.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="stream" /> ist <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="blockSize" /> ist kleiner als 1.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="stream" /> kann nicht gelesen werden.
    /// </exception>
    public static IEnumerable<byte[]> ToEnumerable(this Stream stream, int blockSize)
    {
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }

        if (!stream.CanRead)
        {
            throw new IOException("stream.CanRead");
        }

        if (blockSize < 1)
        {
            throw new ArgumentOutOfRangeException("blockSize");
        }

        var buffer = new byte[blockSize];

        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            if (bytesRead == blockSize)
            {
                yield return buffer;
            }
            else
            {
                yield return buffer.Take(bytesRead)
                                   .ToArray();
            }
        }
    }

    #endregion Methods
}
