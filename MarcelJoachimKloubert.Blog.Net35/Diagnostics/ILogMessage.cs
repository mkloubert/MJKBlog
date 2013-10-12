// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Reflection;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Speichert die Daten einer Log-Nachricht.
    /// </summary>
    public interface ILogMessage
    {
        #region Data Members (5)

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
        /// Gibt den Member zurück, in der der Logvorgang stattfand.
        /// </summary>
        MemberInfo Member { get; }

        /// <summary>
        /// Gibt das Nachrichtenobjekt zurück.
        /// </summary>
        object Message { get; }

        /// <summary>
        /// Gibt den Logzeitpunkt zurück.
        /// </summary>
        DateTimeOffset Time { get; }

        #endregion Data Members
    }
}
