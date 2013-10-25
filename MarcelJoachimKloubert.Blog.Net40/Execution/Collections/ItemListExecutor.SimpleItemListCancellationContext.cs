// s. http://blog.marcel-kloubert.de


namespace MarcelJoachimKloubert.Blog.Execution.Collections
{
    partial class ItemListExecutor<T>
    {
        #region Nested Classes (1)

        private sealed class SimpleItemListCancellationContext<S> : IItemListCancellationContext<T, S>
        {
            #region Properties (5)

            public long CanceledAt
            {
                get;
                internal set;
            }

            public IItemListExecutionContext<T> ExecutionContext
            {
                get;
                internal set;
            }

            public T Item
            {
                get;
                internal set;
            }

            public ItemCancellationSource Source
            {
                get;
                internal set;
            }

            public S State
            {
                get;
                internal set;
            }

            #endregion Properties
        }

        #endregion Nested Classes
    }
}
