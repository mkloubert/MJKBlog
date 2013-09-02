// s. http://blog.marcel-kloubert.de


using System;
using System.Runtime.Serialization;

namespace MarcelJoachimKloubert.Blog.ServiceLocation
{
    /// <summary>
    /// Wird geworfen, wenn die Instanz eines Dienstes nicht erstellt wurde
    /// bzw. erstellt werden konnte.
    /// </summary>
    public class ServiceActivationException : Exception
    {
        #region Fields (1)

        /// <summary>
        /// Speichert die Standard Nachricht für diese Exception.
        /// </summary>
        public const string DEFAULT_EXCEPTION_MESSAGE = "Es konnte keine Instanz dieses Typs erstellt werden!";

        #endregion Fields

        #region Constructors (3)

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="serviceType">Der Typ des zugrundeliegenden Dienstes.</param>
        /// <param name="key">Der Schlüssel.</param>
        /// <param name="innerException">Die innere Ausnahme.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public ServiceActivationException(Type serviceType, object key,
                                          Exception innerException)
            : base(DEFAULT_EXCEPTION_MESSAGE,
                   innerException)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            this.ServiceType = serviceType;
            this.Key = key;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="serviceType">Der Typ des zugrundeliegenden Dienstes.</param>
        /// <param name="key">Der Schlüssel.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public ServiceActivationException(Type serviceType, object key)
            : base(DEFAULT_EXCEPTION_MESSAGE)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            this.ServiceType = serviceType;
            this.Key = key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Exception.Exception(SerializationInfo, StreamingContext)" />
        protected ServiceActivationException(SerializationInfo info,
                                             StreamingContext context)
            : base(info,
                   context)
        {

        }

        #endregion Constructors

        #region Properties (2)

        /// <summary>
        /// Gibt den Schlüssel zurück.
        /// </summary>
        public object Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt den Typ des Dienstes zurück.
        /// </summary>
        public Type ServiceType
        {
            get;
            private set;
        }

        #endregion Properties
    }
}
