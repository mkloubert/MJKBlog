// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

/// <summary>
/// Extension Methoden für Komprimierungsoperationen.
/// </summary>
public static partial class __CompressionExtensionMethods
{
    #region Methods (12)

    // Public Methods (12) 

    /// <summary>
    /// Dekomprimiert einen <see cref="Stream" /> mittels GZip
    /// mit dem Standardpuffer.
    /// </summary>
    /// <param name="inputStream">Der komprimierte Stream.</param>
    /// <returns>Die dekomprimierten Daten.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="inputStream" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="inputStream" /> kann nicht gelesen werden.
    /// </exception>
    public static byte[] GUnzip(this Stream inputStream)
    {
        return GUnzip(inputStream, (int?)null);
    }

    /// <summary>
    /// Dekomprimiert Binärdaten mittels GZip
    /// mit der Standardpuffergrösse.
    /// </summary>
    /// <param name="data">Die komprimierten Daten.</param>
    /// <returns>Die dekomprimierten Daten.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="data" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static byte[] GUnzip(this IEnumerable<byte> data)
    {
        using (var outputStream = new MemoryStream())
        {
            GUnzip(data, outputStream);

            return outputStream.ToArray();
        }
    }

    /// <summary>
    /// Dekomprimiert einen <see cref="Stream" /> mittels GZip.
    /// </summary>
    /// <param name="inputStream">Der komprimierte Stream.</param>
    /// <param name="bufferSize">
    /// Die Puffergrösse, in Bytes (Standard: 81920).
    /// </param>
    /// <returns>Die dekomprimierten Daten.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="inputStream" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> ist kleiner als 1.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="inputStream" /> kann nicht gelesen werden.
    /// </exception>
    public static byte[] GUnzip(this Stream inputStream, int? bufferSize)
    {
        using (var outputStream = new MemoryStream())
        {
            GUnzip(inputStream, outputStream, bufferSize);

            return outputStream.ToArray();
        }
    }

    /// <summary>
    /// Dekomprimiert Binärdaten mittels GZip
    /// mit der Standardpuffergrösse.
    /// </summary>
    /// <param name="data">Die komprimierten Daten.</param>
    /// <param name="outputStream">
    /// Der Stream in dem die dekomprimierten Daten geschrieben werden sollen.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="data" /> und/oder <paramref name="outputStream" />
    /// sind eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="outputStream" /> kann nicht beschrieben werden.
    /// </exception>
    public static void GUnzip(this IEnumerable<byte> data, Stream outputStream)
    {
        if (data == null)
        {
            throw new ArgumentNullException("data");
        }

        var dataArray = data is byte[] ? (byte[])data : data.ToArray();
        using (var inputStream = new MemoryStream(dataArray, false))
        {
            GUnzip(inputStream, outputStream);
        }
    }

    /// <summary>
    /// Dekomprimiert einen <see cref="Stream" /> mittels GZip
    /// mit der Standardpuffergrösse.
    /// </summary>
    /// <typeparam name="S">Typ des Streams.</typeparam>
    /// <param name="inputStream">Der komprimierte Stream.</param>
    /// <param name="outputStream">
    /// Der Stream in dem die dekomprimierten Daten geschrieben werden sollen.
    /// </param>
    /// <returns><paramref name="inputStream" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="inputStream" /> und/oder <paramref name="outputStream" />
    /// sind eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="inputStream" /> kann nicht gelesen werden und/oder
    /// <paramref name="outputStream" /> kann nicht beschrieben werden.
    /// </exception>
    public static S GUnzip<S>(this S inputStream, Stream outputStream)
        where S : global::System.IO.Stream
    {
        return GUnzip<S>(inputStream, outputStream, null);
    }

    /// <summary>
    /// Dekomprimiert einen <see cref="Stream" /> mittels GZip.
    /// </summary>
    /// <typeparam name="S">Typ des Streams.</typeparam>
    /// <param name="inputStream">Der komprimierte Stream.</param>
    /// <param name="outputStream">
    /// Der Stream in dem die dekomprimierten Daten geschrieben werden sollen.
    /// </param>
    /// <param name="bufferSize">
    /// Die Puffergrösse, in Bytes (Standard: 81920).
    /// </param>
    /// <returns><paramref name="inputStream" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="inputStream" /> und/oder <paramref name="outputStream" />
    /// sind eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> ist kleiner als 1.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="inputStream" /> kann nicht gelesen werden und/oder
    /// <paramref name="outputStream" /> kann nicht beschrieben werden.
    /// </exception>
    public static S GUnzip<S>(this S inputStream, Stream outputStream, int? bufferSize)
        where S : global::System.IO.Stream
    {
        if (inputStream == null)
        {
            throw new ArgumentNullException("inputStream");
        }

        if (!inputStream.CanRead)
        {
            throw new IOException("inputStream.CanRead");
        }

        if (outputStream == null)
        {
            throw new ArgumentNullException("outputStream");
        }

        if (!outputStream.CanWrite)
        {
            throw new IOException("outputStream.CanWrite");
        }

        if (bufferSize < 1)
        {
            throw new ArgumentOutOfRangeException("bufferSize");
        }

        using (var gunzip = new GZipStream(inputStream, CompressionMode.Decompress, true))
        {
            var buffer = new byte[bufferSize ?? 81920];
            int bytesRead;
            while ((bytesRead = gunzip.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
            }

            gunzip.Flush();
        }

        return inputStream;
    }

    /// <summary>
    /// Komprimiert einen <see cref="Stream" /> mittels GZip
    /// mit dem Standardpuffer.
    /// </summary>
    /// <param name="inputStream">Der unkomprimierte Stream.</param>
    /// <returns>Die komprimierten Daten.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="inputStream" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="inputStream" /> kann nicht gelesen werden.
    /// </exception>
    public static byte[] GZip(this Stream inputStream)
    {
        return GZip(inputStream, (int?)null);
    }

    /// <summary>
    /// Komprimiert Binärdaten mittels GZip
    /// mit der Standardpuffergrösse.
    /// </summary>
    /// <param name="data">Die komprimierten Daten.</param>
    /// <returns>Die komprimierten Daten.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="data" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static byte[] GZip(this IEnumerable<byte> data)
    {
        using (var outputStream = new MemoryStream())
        {
            GZip(data, outputStream);

            return outputStream.ToArray();
        }
    }

    /// <summary>
    /// Komprimiert einen <see cref="Stream" /> mittels GZip.
    /// </summary>
    /// <param name="inputStream">Der unkomprimierte Stream.</param>
    /// <param name="bufferSize">
    /// Die Puffergrösse, in Bytes (Standard: 81920).
    /// </param>
    /// <returns>Die komprimierten Daten.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="inputStream" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> ist kleiner als 1.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="inputStream" /> kann nicht gelesen werden.
    /// </exception>
    public static byte[] GZip(this Stream inputStream, int? bufferSize)
    {
        using (var outputStream = new MemoryStream())
        {
            GZip(inputStream, outputStream, bufferSize);

            return outputStream.ToArray();
        }
    }

    /// <summary>
    /// Komprimiert Binärdaten mittels GZip
    /// mit der Standardpuffergrösse.
    /// </summary>
    /// <param name="data">Die unkomprimierten Daten.</param>
    /// <param name="outputStream">
    /// Der Stream in dem die komprimierten Daten geschrieben werden sollen.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="data" /> und/oder <paramref name="outputStream" />
    /// sind eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="outputStream" /> kann nicht beschrieben werden.
    /// </exception>
    public static void GZip(this IEnumerable<byte> data, Stream outputStream)
    {
        if (data == null)
        {
            throw new ArgumentNullException("data");
        }

        var dataArray = data is byte[] ? (byte[])data : data.ToArray();
        using (var inputStream = new MemoryStream(dataArray, false))
        {
            GZip(inputStream, outputStream);
        }
    }

    /// <summary>
    /// Komprimiert einen <see cref="Stream" /> mittels GZip
    /// mit der Standardpuffergrösse.
    /// </summary>
    /// <typeparam name="S">Typ des Streams.</typeparam>
    /// <param name="inputStream">Der unkomprimierte Stream.</param>
    /// <param name="outputStream">
    /// Der Stream in dem die komprimierten Daten geschrieben werden sollen.
    /// </param>
    /// <returns><paramref name="inputStream" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="inputStream" /> und/oder <paramref name="outputStream" />
    /// sind eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="inputStream" /> kann nicht gelesen werden und/oder
    /// <paramref name="outputStream" /> kann nicht beschrieben werden.
    /// </exception>
    public static S GZip<S>(this S inputStream, Stream outputStream)
        where S : global::System.IO.Stream
    {
        return GZip<S>(inputStream, outputStream, null);
    }

    /// <summary>
    /// Komprimiert einen <see cref="Stream" /> mittels GZip.
    /// </summary>
    /// <typeparam name="S">Typ des Streams.</typeparam>
    /// <param name="inputStream">Der unkomprimierte Stream.</param>
    /// <param name="outputStream">
    /// Der Stream in dem die komprimierten Daten geschrieben werden sollen.
    /// </param>
    /// <param name="bufferSize">
    /// Die Puffergrösse, in Bytes (Standard: 81920).
    /// </param>
    /// <returns><paramref name="inputStream" /> selbst.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="inputStream" /> und/oder <paramref name="outputStream" />
    /// sind eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> ist kleiner als 1.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="inputStream" /> kann nicht gelesen werden und/oder
    /// <paramref name="outputStream" /> kann nicht beschrieben werden.
    /// </exception>
    public static S GZip<S>(this S inputStream, Stream outputStream, int? bufferSize)
        where S : global::System.IO.Stream
    {
        if (inputStream == null)
        {
            throw new ArgumentNullException("inputStream");
        }

        if (!inputStream.CanRead)
        {
            throw new IOException("inputStream.CanRead");
        }

        if (outputStream == null)
        {
            throw new ArgumentNullException("outputStream");
        }

        if (!outputStream.CanWrite)
        {
            throw new IOException("outputStream.CanWrite");
        }

        if (bufferSize < 1)
        {
            throw new ArgumentOutOfRangeException("bufferSize");
        }

        using (var gzip = new GZipStream(outputStream, CompressionMode.Compress, true))
        {
            var buffer = new byte[bufferSize ?? 81920];
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                gzip.Write(buffer, 0, bytesRead);
            }

            gzip.Flush();
        }

        return inputStream;
    }

    #endregion Methods
}