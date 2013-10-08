// s. http://blog.marcel-kloubert.de


using System;
using System.Net;

namespace MarcelJoachimKloubert.Blog.Net.Http.Impl
{
    /// <summary>
    /// Eine Implementation von <see cref="HttpServerBase" />
    /// auf Basis von <see cref="HttpListener" /> basierend auf
    /// ansychroner Programmierungen mit C# 5.0.
    /// </summary>
    public partial class HttpListenerServer : HttpServerBase
    {
        #region Fields (1)

        private HttpListener _listener;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="HttpListenerServer" />.
        /// </summary>
        public HttpListenerServer()
        {

        }

        #endregion Constructors

        #region Methods (3)

        // Protected Methods (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="HttpServerBase.OnStart()" />
        protected override void OnStart()
        {
            this.StartListening();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="HttpServerBase.OnStop()" />
        protected override void OnStop()
        {
            using (var l = this._listener)
            {
                this._listener = null;
            }
        }
        // Private Methods (1) 

        private void HandleHttpListenerContext(HttpListenerContext context)
        {
            if (context == null)
            {
                return;
            }

            //TODO
        }

        #endregion Methods

        private async void StartListening()
        {
            HttpListener newListener = null;
            try
            {
                var port = this.TcpPort;

                newListener = new HttpListener();
                newListener.Prefixes.Add("http://*:" + port + "/");
                newListener.Prefixes.Add("http://+:" + port + "/");
                newListener.Start();

                this._listener = newListener;
                while (newListener.IsListening)
                {
                    try
                    {
                        var httpCtx = await newListener.GetContextAsync();
                        this.HandleHttpListenerContext(httpCtx);
                    }
                    catch
                    {
                        //TODO
                    }
                }
            }
            catch
            {
                this._listener = null;

                if (newListener != null)
                {
                    try
                    {
                        ((IDisposable)newListener).Dispose();
                    }
                    catch
                    {
                        //TODO
                    }
                }

                //TODO
            }
        }
    }
}
