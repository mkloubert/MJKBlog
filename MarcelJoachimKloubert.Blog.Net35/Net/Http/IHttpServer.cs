// s. http://blog.marcel-kloubert.de


using System;

namespace MarcelJoachimKloubert.Blog.Net.Http
{
    /// <summary>
    /// Beschreibt einen HTTP Server.
    /// </summary>
    public interface IHttpServer : IDisposable
    {
        #region Data Members (4)

        /// <summary>
        /// Gibt zurück, ob dieses Objekt bereits verworfen wurde oder nicht.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gibt zurück, ob dieser Server derzeit läuft oder nicht.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Gibt das eindeutige Objekt zurück, das für Thread-sichere Operationen genutzt wird.
        /// </summary>
        object SyncRoot { get; }

        /// <summary>
        /// Gibt den TCP Port, der für den Server verwendet werden soll.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Der neue Wert ist ausserhalb des gültigen Bereichs.
        /// </exception>
        int TcpPort { get; }

        #endregion Data Members

        #region Delegates and Events (1)

        // Events (1) 

        /// <summary>
        /// Wird ausgeführt nachdem das Objekt verworfen wurde.
        /// </summary>
        event EventHandler Disposed;

        #endregion Delegates and Events

        #region Operations (2)

        /// <summary>
        /// Startet den Server.
        /// </summary>
        void Start();

        /// <summary>
        /// Stoppt den Server.
        /// </summary>
        void Stop();

        #endregion Operations
    }
}
