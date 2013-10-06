// s. http://blog.marcel-kloubert.de


using System.Net;

namespace MarcelJoachimKloubert.Blog.Net.Http.Impl
{
    /// <summary>
    /// Eine Implementation von <see cref="HttpServerBase" />
    /// auf Basis von <see cref="HttpListener" /> basierend auf
    /// ansychroner Programmierungen mit C# 5.0.
    /// </summary>
    public class HttpListenerServer : HttpServerBase
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="HttpListenerServer" />.
        /// </summary>
        public HttpListenerServer()
        {

        }

        #endregion Constructors
    }
}
