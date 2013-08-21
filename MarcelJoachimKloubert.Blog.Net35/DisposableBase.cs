// s. http://blog.marcel-kloubert.de


using System;

/// <summary>
/// Basis-Objekt für ein erweitertes <see cref="IDisposable" />-Objekte.
/// </summary>
public abstract partial class SyncDisposableBase : IDisposable
{
    #region Fields (1)

    /// <summary>
    /// Eindeutiges Objekt für Thread-sichere Operationen.
    /// </summary>
    protected readonly object _SYNC_ROOT = new object();

    #endregion Fields

    #region Constructors (2)

    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="SyncDisposableBase" /> Klasse.
    /// </summary>
    protected SyncDisposableBase()
    {

    }

    /// <summary>
    /// Gibt diese Instanz samt ihrer Resourcen wieder frei.
    /// </summary>
    ~SyncDisposableBase()
    {
        this.Dispose(DisposeContext.Finalizer);
    }

    #endregion Constructors

    #region Properties (1)

    /// <summary>
    /// Gibt zurück, ob dieses Objekt bereits verworfen
    /// / disposed wurde oder nicht.
    /// </summary>
    public bool IsDisposed
    {
        get;
        private set;
    }

    #endregion Properties

    #region Delegates and Events (2)

    // Events (2) 

    /// <summary>
    /// Wird ausgeführt nachdem das Objekt verworfen wurde.
    /// </summary>
    public event EventHandler Disposed;

    /// <summary>
    /// Wird ausgeführt bevor das Objekt verworfen wird.
    /// </summary>
    public event EventHandler Disposing;

    #endregion Delegates and Events

    #region Methods (5)

    // Public Methods (1) 

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="IDisposable.Dispose()" />
    public void Dispose()
    {
        this.Dispose(DisposeContext.DisposeMethod);
        GC.SuppressFinalize(this);
    }
    // Protected Methods (3) 

    /// <summary>
    /// Die Logik für die <see cref="SyncDisposableBase.Dispose()" />-Methode
    /// sowie des Destruktors.
    /// </summary>
    /// <param name="context">Der Kontext.</param>
    protected virtual void OnDispose(DisposeContext context)
    {

    }

    /// <summary>
    /// Führt einen <see cref="EventHandler" /> aus.
    /// </summary>
    /// <param name="handler">Das Delegate, das ausgeführt werden soll.</param>
    /// <returns>
    /// <paramref name="handler" /> wurde ausgeführt oder nicht, da dieser
    /// seine <see langword="null" /> Referenz ist.
    /// </returns>
    protected bool RaiseEventHandler(EventHandler handler)
    {
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Wirft eine <see cref="ObjectDisposedException" />, wenn diese
    /// Instanz bereits verworfen wurde.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// <see cref="SyncDisposableBase.IsDisposed" /> ist
    /// <see langword="true" /> und somit bereits verworfen.
    /// </exception>
    protected void ThrowIfDisposed()
    {
        if (this.IsDisposed)
        {
            var objName = this.GetType().FullName;

            throw new ObjectDisposedException(objName,
                                              string.Format("'{0}' ({1})",
                                                            objName,
                                                            this.GetHashCode()));
        }
    }
    // Private Methods (1) 

    private void Dispose(DisposeContext context)
    {
        lock (this._SYNC_ROOT)
        {
            if (context == DisposeContext.DisposeMethod)
            {
                if (this.IsDisposed)
                {
                    return;
                }

                this.RaiseEventHandler(this.Disposing);
            }

            this.OnDispose(context);

            if (context == DisposeContext.DisposeMethod)
            {
                this.RaiseEventHandler(this.Disposed);
                this.IsDisposed = true;
            }
        }
    }

    #endregion Methods
}
