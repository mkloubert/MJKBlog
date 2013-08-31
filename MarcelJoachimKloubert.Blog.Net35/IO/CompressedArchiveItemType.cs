// s. http://blog.marcel-kloubert.de


namespace MarcelJoachimKloubert.Blog.IO
{
    /// <summary>
    /// Liste mit allen Typen eines Eintrages eines komprimierten Archivs.
    /// </summary>
    public enum CompressedArchiveItemType
    {
        /// <summary>
        /// Verzeichnis
        /// </summary>
        Directory,

        /// <summary>
        /// Datei
        /// </summary>
        File,
    }
}
