// s. http://blog.marcel-kloubert.de


using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WcfHttpServerService = MarcelJoachimKloubert.Blog.Net.HTTP.WcfHttpServer;

namespace MarcelJoachimKloubert.Blog.Net.Http.Impl
{
    /// <summary>
    /// Eine WCF-Implementation von <see cref="HttpServerBase" />
    /// </summary>
    public partial class WcfHttpServer : HttpServerBase
    {
        #region Fields (1)

        private ServiceHost _host;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="WcfHttpServer" />.
        /// </summary>
        public WcfHttpServer()
        {

        }

        #endregion Constructors

        #region Methods (2)

        // Protected Methods (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="HttpServerBase.OnStart()" />
        protected override void OnStart()
        {
            ServiceHost newHost = null;
            try
            {
                newHost = new ServiceHost(new WcfHttpServerService());

                var port = this.TcpPort;

                var baseUrl = new Uri("http://localhost:" + port);

                var transport = new HttpTransportBindingElement();
                transport.KeepAliveEnabled = false;
                transport.AuthenticationScheme = AuthenticationSchemes.Anonymous;
                transport.TransferMode = TransferMode.Buffered;
                transport.MaxReceivedMessageSize = int.MaxValue;
                transport.MaxBufferPoolSize = int.MaxValue;
                transport.MaxBufferSize = int.MaxValue;

                var binding = new CustomBinding(WcfHttpServerService.CreateWebMessageBindingEncoder(),
                                                transport);

                newHost.AddServiceEndpoint(typeof(global::MarcelJoachimKloubert.Blog.Net.HTTP.IWcfHttpServer), binding, baseUrl);
                newHost.Open();

                this._host = newHost;
            }
            catch
            {
                if (newHost != null)
                {
                    ((IDisposable)newHost).Dispose();
                }

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="HttpServerBase.OnStop()" />
        protected override void OnStop()
        {
            using (var s = this._host)
            {
                this._host = null;
            }
        }

        #endregion Methods
    }
}
