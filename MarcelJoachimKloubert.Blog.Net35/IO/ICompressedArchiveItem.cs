// s. http://blog.marcel-kloubert.de


using System;
using System.IO;

namespace MarcelJoachimKloubert.Blog.IO
{
    /// <summary>
    /// Beschreibt einen Eintrag eines komprimierten Archivs.
    /// </summary>
    public interface ICompressedArchiveItem
    {
        #region Data Members (2)

        /// <summary>
        /// Gibt den Namen des Eintrags zurück.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gibt den Typ zurück.
        /// </summary>
        CompressedArchiveItemType Type { get; }

        #endregion Data Members

        #region Operations (1)

        /// <summary>
        /// Öffnet die Daten dieses Eintrags als neuen <see cref="Stream" />.
        /// </summary>
        /// <returns>Die Daten des Eintrags als Stream.</returns>
        /// <exception cref="InvalidOperationException">
        /// Es kann kein Stream geöffnet werden, möglicherweise ist dieser Eintrag
        /// ein Verzeichnis.
        /// </exception>
        Stream OpenStream();

        #endregion Operations
    }
}
