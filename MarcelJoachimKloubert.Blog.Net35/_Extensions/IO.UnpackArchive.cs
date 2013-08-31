// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MarcelJoachimKloubert.Blog.IO;
using SevenZip;

static partial class __IOExtensionMethods
{
    #region Methods (6)

    // Public Methods (5) 

    /// <summary>
    /// Entpackt alle Dateien aus einer gepackten Archivdatei.
    /// </summary>
    /// <param name="archiveFile">Die Archivdatei, welche die gepackten Dateien enthält.</param>
    /// <returns>Die Sequenz mit Dateien.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="archiveFile" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// <paramref name="archiveFile" /> repräsentiert eine Datei, die dem
    /// Anschein nach nicht existiert.
    /// </exception>
    public static IEnumerable<ICompressedArchiveItem> UnpackArchive(this FileInfo archiveFile)
    {
        Stream stream;
        return UnpackArchive(archiveFile,
                             out stream);
    }

    /// <summary>
    /// Entpackt alle Dateien aus einem gepackten Archiv.
    /// </summary>
    /// <param name="archiveData">Die Binärdaten, welche die gepackten Dateien enthalten.</param>
    /// <returns>Die Sequenz mit Dateien.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="archiveData" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static IEnumerable<ICompressedArchiveItem> UnpackArchive(this IEnumerable<byte> archiveData)
    {
        Stream stream;
        return UnpackArchive(archiveData,
                             out stream);
    }

    /// <summary>
    /// Entpackt alle Dateien aus einem gepackten Archiv.
    /// </summary>
    /// <param name="archiveStream">Der Stream, der die gepackten Dateien enthält.</param>
    /// <returns>Die Sequenz mit Dateien.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="archiveStream" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="IOException">
    /// <paramref name="archiveStream" /> kann nicht gelesen werden.
    /// </exception>
    public static IEnumerable<ICompressedArchiveItem> UnpackArchive(this Stream archiveStream)
    {
        if (archiveStream == null)
        {
            throw new ArgumentNullException("archiveStream");
        }

        if (!archiveStream.CanRead)
        {
            throw new IOException("archiveStream.CanRead");
        }

        var extractor = new SevenZipExtractor(archiveStream);
        foreach (var file in extractor.ArchiveFileData)
        {
            var item = new SevenZipCompressedArchiveItem();
            item.Name = file.FileName;
            item.OpenStreamFunc = CreateOpenStreamFunc(extractor,
                                                       file);
            if (file.IsDirectory)
            {
                item.Type = CompressedArchiveItemType.Directory;
            }
            else
            {
                item.Type = CompressedArchiveItemType.File;
            }

            if (item != null)
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Entpackt alle Dateien aus einer gepackten Archivdatei.
    /// </summary>
    /// <param name="archiveFile">Die Archivdatei, welche die gepackten Dateien enthält.</param>
    /// <param name="stream">
    /// Die Variabel, die den zugrundeliegenden <see cref="Stream" /> speichern soll,
    /// der die die gepackten Dateien enthält, um diesen ggf. später zu disposen.
    /// </param>
    /// <returns>Die Sequenz mit Dateien.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="archiveFile" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// <paramref name="archiveFile" /> repräsentiert eine Datei, die dem
    /// Anschein nach nicht existiert.
    /// </exception>
    public static IEnumerable<ICompressedArchiveItem> UnpackArchive(this FileInfo archiveFile,
                                                                    out Stream stream)
    {
        if (archiveFile == null)
        {
            throw new ArgumentNullException("archiveFile");
        }

        if (!archiveFile.Exists)
        {
            throw new FileNotFoundException();
        }

        stream = archiveFile.OpenRead();
        return UnpackArchive(stream);
    }

    /// <summary>
    /// Entpackt alle Dateien aus einem gepackten Archiv.
    /// </summary>
    /// <param name="archiveData">Die Binärdaten, welche die gepackten Dateien enthalten.</param>
    /// <param name="stream">
    /// Die Variabel, die den zugrundeliegenden <see cref="Stream" /> speichern soll,
    /// der die die gepackten Dateien enthält, um diesen ggf. später zu disposen.
    /// </param>
    /// <returns>Die Sequenz mit Dateien.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="archiveData" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static IEnumerable<ICompressedArchiveItem> UnpackArchive(this IEnumerable<byte> archiveData,
                                                                    out Stream stream)
    {
        if (archiveData == null)
        {
            throw new ArgumentNullException("archiveData");
        }

        stream = new MemoryStream(archiveData is byte[] ? (byte[])archiveData : archiveData.ToArray(),
                                  false);

        return UnpackArchive(stream);
    }
    // Private Methods (1) 

    private static Func<Stream> CreateOpenStreamFunc(SevenZipExtractor extractor,
                                                     ArchiveFileInfo file)
    {
        if (file.IsDirectory)
        {
            return new Func<Stream>(() =>
            {
                throw new InvalidOperationException();
            });
        }

        return new Func<Stream>(() =>
        {
            var result = new MemoryStream();
            try
            {
                extractor.ExtractFile(file.Index,
                                      result);
            }
            catch
            {
                result.Dispose();
                throw;
            }

            return result;
        });
    }

    #endregion Methods

    #region Nested Classes (1)

    private sealed class SevenZipCompressedArchiveItem : ICompressedArchiveItem
    {
        #region Properties (3)

        public string Name
        {
            get;
            internal set;
        }

        internal Func<Stream> OpenStreamFunc
        {
            get;
            set;
        }

        public CompressedArchiveItemType Type
        {
            get;
            internal set;
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        public Stream OpenStream()
        {
            return this.OpenStreamFunc();
        }

        #endregion Methods
    }

    #endregion Nested Classes
}
