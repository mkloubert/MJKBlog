// s. http://blog.marcel-kloubert.de


namespace MarcelJoachimKloubert.Blog.Execution.Collections
{
    #region INTERFACE: IItemListCancellationContext<T>

    /// <summary>
    /// Beschreibt den Kontext zum Abbruch eines gesamten Vorgangs.
    /// </summary>
    /// <typeparam name="T">Typ des zugrundeliegenden Elements.</typeparam>
    public interface IItemListCancellationContext<T>
    {
        #region Data Members (4)

        /// <summary>
        /// Gibt den Index zurück an dem der Abbruch stattfand.
        /// </summary>
        long CanceledAt { get; }

        /// <summary>
        /// Gibt den zugrundeliegenden Ausführungskontext zurück.
        /// </summary>
        IItemListExecutionContext<T> ExecutionContext { get; }

        /// <summary>
        /// Gibt das Element zurück, bei dem abgebrochen wurde.
        /// </summary>
        T Item { get; }

        /// <summary>
        /// Gibt die Quelle zurück, die den Vorgang abgebrochen hat.
        /// </summary>
        ItemCancellationSource Source { get; }

        #endregion Data Members
    }

    #endregion

    #region INTERFACE: IItemListCancellationContext<T, S>

    /// <summary>
    /// Beschreibt den Kontext zum Abbruch eines gesamten Vorgangs.
    /// </summary>
    /// <typeparam name="T">Typ des zugrundeliegenden Elements.</typeparam>
    /// <typeparam name="S">Typ des zugrundeliegenden State-Objektes.</typeparam>
    public interface IItemListCancellationContext<T, S> : IItemListCancellationContext<T>
    {
        #region Data Members (1)

        /// <summary>
        /// Gibt das State-Objekt zurück.
        /// </summary>
        S State { get; }

        #endregion Data Members
    }

    #endregion

}
