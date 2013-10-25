// s. http://blog.marcel-kloubert.de


namespace MarcelJoachimKloubert.Blog.Execution.Collections
{
    /// <summary>
    /// Listet die Quellen auf, die einen Vorgang abbrechen können.
    /// </summary>
    public enum ItemCancellationSource
    {
        /// <summary>
        /// Unbekannt
        /// </summary>
        Unknown,

        /// <summary>
        /// Durch einen Aufruf von <see cref="global::System.Threading.CancellationToken.ThrowIfCancellationRequested()" />.
        /// </summary>
        CancellationToken,

        /// <summary>
        /// Durch das Setzen der <see cref="IItemExecutionContext{T, S}.Cancel" /> Eigenschaft auf <see langword="true" />.
        /// </summary>
        ItemContext,
    }
}
