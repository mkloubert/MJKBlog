// s. http://blog.marcel-kloubert.de


using System;
using System.Runtime.Serialization;
using System.Security;

namespace MarcelJoachimKloubert.Blog.Execution.Collections
{
    /// <summary>
    /// Speichert die Ausnahme, die beim Ausführen der Logik für ein Element aufgetreten ist.
    /// </summary>
    /// <typeparam name="T">Typ des Elements.</typeparam>
    /// <typeparam name="S">Typ des zugrundeliegenden STate-Objektes.</typeparam>
    public class ItemExecutionException<T, S> : Exception
    {
        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="ItemExecutionException{T, S}" />-Klasse mit serialisierten Daten.
        /// </summary>
        /// <param name="item">Das Element bei dem der Fehler aufgetreten ist.</param>
        /// <param name="state">Das zugrundeliegende State-Objekt.</param>
        /// <param name="index">Der Index bei dem der Fehler aufgetreten ist.</param>
        /// <param name="message">Die Fehlermeldung.</param>
        /// <param name="innerException">Die eigentliche Ausnahme die aufgetreten ist.</param>
        public ItemExecutionException(T item, S state, long index, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Item = item;
            this.State = state;
            this.Index = index;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="ItemExecutionException{T, S}" />-Klasse mit serialisierten Daten.
        /// </summary>
        /// <param name="info">
        /// Die <see cref="SerializationInfo" />-Klasse, die die serialisierten für die ausgelöste Ausnahme enthält.
        /// </param>
        /// <param name="context">
        /// Der <see cref="StreamingContext" />, der die Kontextinformationen über die Quelle oder das Ziel enthält.
        /// </param>
        [SecuritySafeCritical]
        protected ItemExecutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        #endregion Constructors

        #region Properties (3)

        /// <summary>
        /// Gibt den Fehler zurück, bei dem der Fehler aufgetreten ist.
        /// </summary>
        public long Index
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt das zugrundeliegende Element zurück.
        /// </summary>
        public T Item
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt das zugrundeliegende State-Objekt zurück.
        /// </summary>
        public S State
        {
            get;
            private set;
        }

        #endregion Properties
    }
}
