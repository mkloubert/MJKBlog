// s. http://blog.marcel-kloubert.de


using System;
using System.IO;
using System.Linq;

/// <summary>
/// Extension Methoden für Sicherheitsoperationen.
/// </summary>
public static partial class __SecurityExtensionMethods
{
    #region Methods (4)

    // Public Methods (4) 

    /// <summary>
    /// Shreddert einen <see cref="Stream" /> nach DoD 5220-22.M mit
    /// 3 Durchläufen und einer Blockgrösse von 8192 Bytes.
    /// Es wird versucht den Wert der Eigenschaft
    /// <see cref="Stream.Position" /> von <paramref name="stream" />
    /// nach Beendigung des Vorgangs wiederherzustellen.
    /// </summary>
    /// <typeparam name="S">Typ des Streams.</typeparam>
    /// <param name="stream">Der Stream.</param>
    /// <returns><paramref name="stream" /> selbst.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// <paramref name="stream" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="stream" /> ist nicht beschreibbar und/oder
    /// sein Cursor kann nicht gesetzt werden.
    /// </exception>
    public static S Shredder<S>(this S stream)
        where S : global::System.IO.Stream
    {
        return Shredder<S>(stream, 3);
    }

    /// <summary>
    /// Shreddert einen <see cref="Stream" /> nach DoD 5220-22.M mit
    /// einer Blockgrösse von 8192 Bytes.
    /// Es wird versucht den Wert der Eigenschaft
    /// <see cref="Stream.Position" /> von <paramref name="stream" />
    /// nach Beendigung des Vorgangs wiederherzustellen.
    /// </summary>
    /// <typeparam name="S">Typ des Streams.</typeparam>
    /// <param name="stream">Der Stream.</param>
    /// <param name="count">Anzahl der Durchläufe.</param>
    /// <returns><paramref name="stream" /> selbst.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// <paramref name="stream" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="stream" /> ist nicht beschreibbar und/oder
    /// sein Cursor kann nicht gesetzt werden.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="count" /> ist ungültig.
    /// </exception>
    public static S Shredder<S>(this S stream, int count)
        where S : global::System.IO.Stream
    {
        return Shredder<S>(stream, count, 8192);
    }

    /// <summary>
    /// Shreddert einen <see cref="Stream" /> nach DoD 5220-22.M.
    /// Es wird versucht den Wert der Eigenschaft
    /// <see cref="Stream.Position" /> von <paramref name="stream" />
    /// nach Beendigung des Vorgangs wiederherzustellen.
    /// </summary>
    /// <typeparam name="S">Typ des Streams.</typeparam>
    /// <param name="stream">Der Stream.</param>
    /// <param name="count">Anzahl der Durchläufe.</param>
    /// <param name="blockSize">Grösse eines Blocks, in Bytes.</param>
    /// <returns><paramref name="stream" /> selbst.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// <paramref name="stream" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="stream" /> ist nicht beschreibbar und/oder
    /// sein Cursor kann nicht gesetzt werden.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="count" /> und/oder <paramref name="blockSize" />
    /// sind ungültig.
    /// </exception>
    public static S Shredder<S>(this S stream, int count, int blockSize)
        where S : global::System.IO.Stream
    {
        return Shredder<S>(stream, count, blockSize, true);
    }

    /// <summary>
    /// Shreddert einen <see cref="Stream" /> nach DoD 5220-22.M.
    /// </summary>
    /// <typeparam name="S">Typ des Streams.</typeparam>
    /// <param name="stream">Der Stream.</param>
    /// <param name="count">Anzahl der Durchläufe.</param>
    /// <param name="blockSize">Grösse eines Blocks, in Bytes.</param>
    /// <param name="restorePos">
    /// Den Wert der Eigenschaft <see cref="Stream.Position" /> von
    /// <paramref name="stream" /> nach Beendigung des Vorgangs
    /// versuchen wiederherzustellen. Hierbei spielt es keine Rolle, ob der
    /// Vorgang erfolgreich war oder nicht.
    /// </param>
    /// <returns><paramref name="stream" /> selbst.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// <paramref name="stream" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="stream" /> ist nicht beschreibbar und/oder
    /// sein Cursor kann nicht gesetzt werden.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="count" /> und/oder <paramref name="blockSize" />
    /// sind ungültig.
    /// </exception>
    public static S Shredder<S>(this S stream,
                                int count,
                                int blockSize,
                                bool restorePos)
        where S : global::System.IO.Stream
    {
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }

        if (!stream.CanSeek)
        {
            throw new IOException("stream.CanSeek");
        }

        if (!stream.CanWrite)
        {
            throw new IOException("stream.CanWrite");
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        if (blockSize < 1)
        {
            throw new ArgumentOutOfRangeException("blockSize");
        }

        long? lastKnownPos = null;
        if (restorePos)
        {
            lastKnownPos = stream.Position;
        }

        try
        {
            var len = stream.Length;

            for (var i = 0; i < count; i++)
            {
                stream.Position = 0;

                byte byteToWrite = 0;
                switch (i % 3)
                {
                    case 0:
                        byteToWrite = 255;
                        break;

                    case 2:
                        byteToWrite = 151;
                        break;
                }

                var block = Enumerable.Repeat(byteToWrite, blockSize)
                                      .ToArray();

                var blockCount = (long)Math.Floor((decimal)len /
                                                  (decimal)block.LongLength);
                for (long ii = 0; ii < blockCount; ii++)
                {
                    stream.Write(block, 0, block.Length);
                    stream.Flush();
                }

                var lastBlockSize = (int)(len % (long)blockSize);
                if (lastBlockSize > 0)
                {
                    stream.Write(block, 0, lastBlockSize);
                    stream.Flush();
                }
            }
        }
        finally
        {
            if (restorePos)
            {
                stream.Position = lastKnownPos.Value;
            }
        }

        return stream;
    }

    #endregion Methods
}