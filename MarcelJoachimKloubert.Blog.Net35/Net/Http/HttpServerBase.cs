// s. http://blog.marcel-kloubert.de



namespace MarcelJoachimKloubert.Blog.Net.Http
{
    /// <summary>
    /// Die Basis-Klasse eines HTTP Servers.
    /// </summary>
    public abstract class HttpServerBase : SyncDisposableBase, IHttpServer
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="HttpServerBase" />.
        /// </summary>
        protected HttpServerBase()
        {

        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IHttpServer.SyncRoot" />
        public object SyncRoot
        {
            get { return this._SYNC_ROOT; }
        }

        #endregion Properties
    }
}
