// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Threading;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Speichert die Daten einer Log-Nachricht.
    /// </summary>
    public interface ILogMessage : IEquatable<ILogMessage>
    {
        #region Data Members (9)

        /// <summary>
        /// Gibt das aufrufende Assembly zurück.
        /// </summary>
        Assembly Assembly { get; }

        /// <summary>
        /// Gibt eine schreibgeschützte Liste mit allen Kategorien zurück,
        /// der dieser Nachricht zugeordnet ist.
        /// </summary>
        IList<LoggerFacadeCategories> Categories { get; }

        /// <summary>
        /// Gibt den zugrundeliegenden Kontext zurück.
        /// </summary>
        Context Context { get; }

        /// <summary>
        /// Gibt die ID dieser Nachricht zurück.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gibt den Member zurück, in der der Logvorgang stattfand.
        /// </summary>
        MemberInfo Member { get; }

        /// <summary>
        /// Gibt das Nachrichtenobjekt zurück.
        /// </summary>
        object Message { get; }

        /// <summary>
        /// Gibt den zugrundeliegenden Principal zurück.
        /// </summary>
        IPrincipal Principal { get; }

        /// <summary>
        /// Gibt den Thread zurück, in dem die Nachricht geschrieben wurde.
        /// </summary>
        Thread Thread { get; }

        /// <summary>
        /// Gibt den Logzeitpunkt zurück.
        /// </summary>
        DateTimeOffset Time { get; }

        #endregion Data Members
    }
}
