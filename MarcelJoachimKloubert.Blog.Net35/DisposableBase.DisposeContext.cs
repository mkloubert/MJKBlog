// s. http://blog.marcel-kloubert.de


using System;

partial class SyncDisposableBase
{
    #region Enums (1)

    /// <summary>
    /// Liste mit 'Dispose'-Kontexten
    /// </summary>
    public enum DisposeContext
    {
        /// <summary>
        /// Destruktor
        /// </summary>
        Finalizer,

        /// <summary>
        /// <see cref="IDisposable.Dispose()" />-Methode
        /// </summary>
        DisposeMethod,
    }

    #endregion Enums
}
