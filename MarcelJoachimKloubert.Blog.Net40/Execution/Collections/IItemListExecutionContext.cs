// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Blog.Execution.Collections
{
    #region IItemListExecutionContext<T>

    /// <summary>
    /// Beschreibt einen Kontext für das Ausführen der Elemente einer Liste.
    /// </summary>
    /// <typeparam name="T">Type der Elemente.</typeparam>
    public interface IItemListExecutionContext<T>
    {
        #region Data Members (16)

        /// <summary>
        /// Gibt den optionalen Callback zurück, der ausgeführt werden soll, wenn der Vorgang erfolgreich beendet wurde.
        /// </summary>
        Action<IItemListExecutionContext<T>> CanceledCallback { get; set; }

        /// <summary>
        /// Gibt zurück, sofern definiert, den Kontext zu Abbruch des Vorgangs zurück.
        /// </summary>
        IItemListCancellationContext<T> CancellationContext { get; }

        /// <summary>
        /// Gibt das <see cref="CancellationTokenSource" /> Objekt zurück, das zum Abbrechen von
        /// <see cref="IItemListExecutionContext{T}.Task" /> verwendet werden soll.
        /// </summary>
        CancellationTokenSource CancellationSource { get; }

        /// <summary>
        /// Gibt den optionalen Callback zurück, der ausgeführt werden soll, wenn der Vorgang beendet wurde,
        /// ob erfolgreich oder nicht.
        /// </summary>
        Action<IItemListExecutionContext<T>> CompletedCallback { get; set; }

        /// <summary>
        /// Gibt die Optionen urück, mit dem <see cref="IItemListExecutionContext{T}.Task" /> erstellt wurde.
        /// </summary>
        TaskCreationOptions CreationOptions { get; }

        /// <summary>
        /// Gibt die Liste der bei der Ausführung aufgetretenen Fehler zurück
        /// oder <see langword="null" />, wenn kein Fehler aufgetreten ist.
        /// </summary>
        AggregateException Errors { get; }

        /// <summary>
        /// Gibt den optionalen Callback zurück, der ausgeführt werden soll, wenn der Vorgang mit einem Fehler beendet wurde.
        /// </summary>
        Action<IItemListExecutionContext<T>> FaultedCallback { get; set; }

        /// <summary>
        /// Gibt zurück, ob die gesamte Operation abgeborchen wurde oder nicht.
        /// </summary>
        bool IsCanceled { get; }

        /// <summary>
        /// Gibt zurück, ob die gesamte Operation fehlgeschlagen ist oder nicht.
        /// </summary>
        bool IsFaulted { get; }

        /// <summary>
        /// Gibt zurück, ob der zugrundeliegende Task derzeit läuft oder nicht.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Gibt die Anzahl der zugrundeliegenden Elemente zurück, sofern ermittelbar.
        /// </summary>
        long? ItemCount { get; }

        /// <summary>
        /// Gibt die Liste der zugrundeliegenden Elemente zurück.
        /// </summary>
        IEnumerable<T> Items { get; }

        /// <summary>
        /// Gibt das letzte Resultat zurück.
        /// </summary>
        object LastResult { get; }

        /// <summary>
        /// Gibt den zugrundliegenden <see cref="TaskScheduler" /> für <see cref="IItemListExecutionContext{T}.Task" /> zurück.
        /// </summary>
        TaskScheduler Scheduler { get; }

        /// <summary>
        /// Gibt den optionalen Callback zurück, der ausgeführt werden soll, wenn der Vorgang erfolgreich beendet wurde.
        /// </summary>
        Action<IItemListExecutionContext<T>> SucceededCallback { get; set; }

        /// <summary>
        /// Gibt den zugrundeliegenden Task zurück.
        /// </summary>
        Task Task { get; }

        #endregion Data Members

        #region Operations (3)

        /// <summary>
        /// Bricht die Ausführung ab.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Startet die Ausführung.
        /// </summary>
        void Start();

        /// <summary>
        /// Wartet, bis die Ausführung beendet wurde.
        /// </summary>
        /// <param name="timeout">Die maximale Zeit, die gewartet werdem soll, bis es zu einem Timeout kommt.</param>
        void Wait(TimeSpan? timeout = null);

        #endregion Operations
    }

    #endregion

    #region IItemListExecutionContext<T, S>

    /// <summary>
    /// Beschreibt einen Kontext für das Ausführen der Elemente einer Liste
    /// mit zusätzlichen State-Objekt für die Elemente.
    /// </summary>
    /// <typeparam name="T">Type der Elemente.</typeparam>
    /// <typeparam name="S">Typ des State-Objektes.</typeparam>
    public interface IItemListExecutionContext<T, S> : IItemListExecutionContext<T>
    {
        #region Data Members (6)

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IItemListExecutionContext{T}.CanceledCallback" />
        new Action<IItemListExecutionContext<T, S>> CanceledCallback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IItemListExecutionContext{T}.CancellationContext" />
        new IItemListCancellationContext<T, S> CancellationContext { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IItemListExecutionContext{T}.CompletedCallback" />
        new Action<IItemListExecutionContext<T, S>> CompletedCallback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IItemListExecutionContext{T}.FaultedCallback" />
        new Action<IItemListExecutionContext<T, S>> FaultedCallback { get; set; }

        /// <summary>
        /// Gibt ggf. das State-Objekt zurück, das jedem Element übergeben wird.
        /// </summary>
        S State { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IItemListExecutionContext{T}.SucceededCallback" />
        new Action<IItemListExecutionContext<T, S>> SucceededCallback { get; set; }

        #endregion Data Members
    }

    #endregion
}
