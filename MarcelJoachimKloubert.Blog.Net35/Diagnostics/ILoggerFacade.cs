// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Beschreibt ein Objekt, das Meldungen/Objekte loggt.
    /// </summary>
    public interface ILoggerFacade
    {
        #region Operations (5)

        /// <summary>
        /// Loggt eine Nachricht.
        /// </summary>
        /// <param name="msgObj">Die Nachricht.</param>
        /// <returns>Diese Instanz.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="msgObj" /> ist <see langword="null" />.
        /// </exception>
        void Log(ILogMessage msgObj);

        /// <summary>
        /// Loggt ein Objekt.
        /// </summary>
        /// <param name="msg">Das Objekt / die Nachricht.</param>
        /// <returns>Diese Instanz.</returns>
        void Log(object msg);

        /// <summary>
        /// Loggt ein Objekt.
        /// </summary>
        /// <param name="msg">Das Objekt / die Nachricht.</param>
        /// <param name="tag">Bspw. der Name der Quelle der Nachricht.</param>
        void Log(object msg,
                 IEnumerable<char> tag);

        /// <summary>
        /// Loggt ein Objekt.
        /// </summary>
        /// <param name="msg">Das Objekt / die Nachricht.</param>
        /// <param name="categories">Die Liste von Kategorien.</param>
        void Log(object msg,
                 LoggerFacadeCategories categories);

        /// <summary>
        /// Loggt ein Objekt.
        /// </summary>
        /// <param name="msg">Das Objekt / die Nachricht.</param>
        /// <param name="tag">Bspw. der Name der Quelle der Nachricht.</param>
        /// <param name="categories">Die Liste von Kategorien.</param>
        void Log(object msg,
                 IEnumerable<char> tag,
                 LoggerFacadeCategories categories);

        #endregion Operations
    }
}
