﻿// s. http://blog.marcel-kloubert.de


using System;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Liste mit Logger-Kategorien.
    /// </summary>
    [Flags]
    public enum LoggerFacadeCategories
    {
        /// <summary>
        /// Keine Karegorie / unbekannt
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Informationen (blaues i)
        /// </summary>
        Information = 1,

        /// <summary>
        /// Warnung (gelebes Ausrufezeichen)
        /// </summary>
        Warnings = 2,

        /// <summary>
        /// Fehler (rotes X)
        /// </summary>
        Errors = 4,

        /// <summary>
        /// Schwerer Fehler (Atompilz)
        /// </summary>
        FatalErrors = 8,
    }
}