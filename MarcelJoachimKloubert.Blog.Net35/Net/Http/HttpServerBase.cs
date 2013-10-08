// s. http://blog.marcel-kloubert.de


using System;
using System.Net;
namespace MarcelJoachimKloubert.Blog.Net.Http
{
    /// <summary>
    /// Die Basis-Klasse eines HTTP Servers.
    /// </summary>
    public abstract class HttpServerBase : SyncDisposableBase, IHttpServer
    {
        #region Fields (2)

        private int _tcpPort;
        /// <summary>
        /// Speichert den Standardwert für den TCP-Ports eines HTTP-Servers.
        /// </summary>
        public const int DEFAULT_HTTP_PORT = 80;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="HttpServerBase" />.
        /// </summary>
        protected HttpServerBase()
        {
            this.TcpPort = DEFAULT_HTTP_PORT;
        }

        #endregion Constructors

        #region Properties (3)

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IHttpServer.IsRunning" />
        public bool IsRunning
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IHttpServer.SyncRoot" />
        public object SyncRoot
        {
            get { return this._SYNC_ROOT; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IHttpServer.TcpPort" />
        public int TcpPort
        {
            get { return this._tcpPort; }

            set
            {
                if (value < IPEndPoint.MinPort || value > IPEndPoint.MaxPort)
                {
                    throw new ArgumentOutOfRangeException("value",
                                                          value,
                                                          string.Format("Der Wert muss zwischen {0} und {1} liegen!",
                                                                        IPEndPoint.MinPort,
                                                                        IPEndPoint.MaxPort));
                }

                this._tcpPort = value;
            }
        }

        #endregion Properties

        #region Methods (5)

        // Public Methods (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IHttpServer.Start()" />
        public void Start()
        {
            lock (this.SyncRoot)
            {
                if (this.IsRunning)
                {
                    return;
                }

                this.OnStart();
                this.IsRunning = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IHttpServer.Stop()" />
        public void Stop()
        {
            lock (this.SyncRoot)
            {
                if (!this.IsRunning)
                {
                    return;
                }

                this.OnStop();
                this.IsRunning = false;
            }
        }
        // Protected Methods (3) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="SyncDisposableBase.OnDispose(DisposeContext)"/>
        protected override void OnDispose(DisposeContext context)
        {
            base.OnDispose(context);

            this.OnStop();
        }

        /// <summary>
        /// Die Logik für <see cref="HttpServerBase.Start()" />.
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Die Logik für <see cref="HttpServerBase.Stop()" />.
        /// </summary>
        protected abstract void OnStop();

        #endregion Methods
    }
}
