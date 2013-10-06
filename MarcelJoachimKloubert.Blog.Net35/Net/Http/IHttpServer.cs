// s. http://blog.marcel-kloubert.de


using System;

namespace MarcelJoachimKloubert.Blog.Net.Http
{
    /// <summary>
    /// Beschreibt einen HTTP Server.
    /// </summary>
    public interface IHttpServer : IDisposable
    {
        #region Data Members (2)

        /// <summary>
        /// Gibt zurück, ob dieses Objekt bereits verworfen wurde oder nicht.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gibt das eindeutige Objekt zurück, das für Thread-sichere Operationen genutzt wird.
        /// </summary>
        object SyncRoot { get; }

        #endregion Data Members

        #region Delegates and Events (1)

        // Events (1) 

        /// <summary>
        /// Wird ausgeführt nachdem das Objekt verworfen wurde.
        /// </summary>
        event EventHandler Disposed;

        #endregion Delegates and Events
    }
}
