// s. http://blog.marcel-kloubert.de


using System;
using System.Threading;

namespace MarcelJoachimKloubert.Blog.Execution.Collections
{
    partial class ItemListExecutor<T>
    {
        #region Nested Classes (1)

        private sealed class SimpleIItemExecutionContext<S> : IItemExecutionContext<T, S>
        {
            #region Properties (8)

            public bool Cancel
            {
                get;
                set;
            }

            public CancellationToken CancelToken
            {
                get;
                set;
            }

            public long Index
            {
                get;
                internal set;
            }

            public T Item
            {
                get;
                internal set;
            }

            public long? ItemCount
            {
                get;
                internal set;
            }

            public AggregateException LastErrors
            {
                get;
                internal set;
            }

            public object Result
            {
                get;
                set;
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
