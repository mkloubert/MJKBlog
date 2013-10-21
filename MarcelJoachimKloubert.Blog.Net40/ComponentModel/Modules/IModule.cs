// s. http://blog.marcel-kloubert.de


using System;

namespace MarcelJoachimKloubert.Blog.ComponentModel.Modules
{
    /// <summary>
    /// Beschreibt ein Modul.
    /// </summary>
    public interface IModule : IDisposable
    {
        #region Data Members (2)

        /// <summary>
        /// Gibt zurück, ob dieses Modul bereits verworfen wurde oder nicht.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gibt zurück, ob dieses Modul bereits initialisiert wurde oder nicht.
        /// </summary>
        bool IsInitialized { get; }

        #endregion Data Members

        #region Operations (1)

        /// <summary>
        /// Initialisiert dieses Modul.
        /// </summary>
        /// <param name="context">Der zugrundeliegende Kontext.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="context" /> ist <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Das Modul ist bereits initialisiert.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// Das Modul wurde bereits initialisiert.
        /// </exception>
        void Initialize(IModuleInitializeContext context);

        #endregion Operations
    }
}
