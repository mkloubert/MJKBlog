// s. http://blog.marcel-kloubert.de


using System;
using System.Threading;

namespace MarcelJoachimKloubert.Blog.Execution.Collections
{
    /// <summary>
    /// Der Kontext zum Ausführen eines Elements.
    /// </summary>
    /// <typeparam name="T">Typ der Elemente.</typeparam>
    /// <typeparam name="S">Typ des State-Objektes</typeparam>
    public interface IItemExecutionContext<T, S>
    {
        #region Data Members (8)

        /// <summary>
        /// Gibt zurück, ob die weitere Ausführung abgebrochen werden soll oder nicht.
        /// </summary>
        bool Cancel { get; set; }

        /// <summary>
        /// Gibt das <see cref="CancellationToken" /> zurück, mit dem der zugrundeliegende Task abgebrochen werden kann.
        /// </summary>
        CancellationToken CancelToken { get; }

        /// <summary>
        /// Gibt den aktuellen 0 basierten Index zurück.
        /// </summary>
        long Index { get; }

        /// <summary>
        /// Gibt das aktuelle Element zurück.
        /// </summary>
        T Item { get; }

        /// <summary>
        /// Gibt die Anzahl aller Elemente zurück.
        /// </summary>
        long ItemCount { get; }

        /// <summary>
        /// Gibt die Fehler von der letzten Ausführung zurück oder <see langword="null" />, wenn kein Fehler aufgetreten ist.
        /// </summary>
        AggregateException LastErrors { get; }

        /// <summary>
        /// Gibt das Objekt zurück, das von vorherigen Element weitergericht wurde
        /// oder legt eines für das nächste Element bzw. den zugrundliegenden <see cref="IItemListExecutionContext{T}" />
        /// fest.
        /// </summary>
        object Result { get; set; }

        /// <summary>
        /// Gibt ggf. das State-Objekt zurück, das für <see cref="IItemExecutionContext{T, S}.Item" /> übergeben wird.
        /// </summary>
        S State { get; }

        #endregion Data Members
    }
}
