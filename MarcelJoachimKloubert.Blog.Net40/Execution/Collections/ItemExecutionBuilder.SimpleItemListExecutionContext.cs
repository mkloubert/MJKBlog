// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Blog.Execution.Collections
{
    partial class ItemExecutionBuilder<T>
    {
        #region Nested Classes (1)

        private sealed class SimpleItemListExecutionContext<S> : IItemListExecutionContext<T, S>
        {
            #region Fields (8)

            private Action<IItemListExecutionContext<T, S>> _completedCallback;
            private Action<IItemListExecutionContext<T>> _completedCallbackBase;
            private Action<IItemListExecutionContext<T, S>> _faultedCallback;
            private Action<IItemListExecutionContext<T>> _faultedCallbackBase;
            private Action<IItemListExecutionContext<T, S>> _succeededCallback;
            private Action<IItemListExecutionContext<T>> _succeededCallbackBase;
            internal Action<IItemExecutionContext<T, S>> Action;
            internal bool RunAsyn;

            #endregion Fields

            #region Properties (18)

            public long? CanceledAt
            {
                get;
                set;
            }

            public CancellationTokenSource CancellationSource
            {
                get;
                internal set;
            }

            public Action<IItemListExecutionContext<T, S>> CompletedCallback
            {
                get { return this._completedCallback; }

                set
                {
                    SetCallback(value,
                                ref this._completedCallback,
                                ref this._completedCallbackBase);
                }
            }

            public TaskCreationOptions CreationOptions
            {
                get;
                internal set;
            }

            public AggregateException Errors
            {
                get;
                internal set;
            }

            public Action<IItemListExecutionContext<T, S>> FaultedCallback
            {
                get { return this._faultedCallback; }

                set
                {
                    SetCallback(value,
                                ref this._faultedCallback,
                                ref this._faultedCallbackBase);
                }
            }

            Action<IItemListExecutionContext<T>> IItemListExecutionContext<T>.CompletedCallback
            {
                get { return this._completedCallbackBase; }

                set { this.CompletedCallback = value; }
            }

            Action<IItemListExecutionContext<T>> IItemListExecutionContext<T>.FaultedCallback
            {
                get { return this._faultedCallbackBase; }

                set { this.FaultedCallback = value; }
            }

            Action<IItemListExecutionContext<T>> IItemListExecutionContext<T>.SucceededCallback
            {
                get { return this._succeededCallbackBase; }

                set { this.SucceededCallback = value; }
            }

            public bool IsFaulted
            {
                get;
                set;
            }

            public bool IsRunning
            {
                get;
                internal set;
            }

            public long ItemCount
            {
                get;
                internal set;
            }

            public IEnumerable<T> Items
            {
                get;
                internal set;
            }

            public object LastResult
            {
                get;
                internal set;
            }

            public TaskScheduler Scheduler
            {
                get;
                internal set;
            }

            public S State
            {
                get;
                internal set;
            }

            public Action<IItemListExecutionContext<T, S>> SucceededCallback
            {
                get { return this._succeededCallback; }

                set
                {
                    SetCallback(value,
                                ref this._succeededCallback,
                                ref this._succeededCallbackBase);
                }
            }

            public Task Task
            {
                get;
                internal set;
            }

            #endregion Properties

            #region Methods (4)

            // Public Methods (3) 

            public void Cancel()
            {
                this.CancellationSource
                    .Cancel(throwOnFirstException: false);
            }

            public void Start()
            {
                try
                {
                    this.IsRunning = true;

                    if (this.RunAsyn)
                    {
                        this.Task.Start(this.Scheduler);
                    }
                    else
                    {
                        this.Task.RunSynchronously(this.Scheduler);
                    }
                }
                catch (Exception ex)
                {
                    this.IsRunning = false;
                    this.IsFaulted = true;

                    this.Errors = new AggregateException(ex);

                    throw;
                }
            }

            public void Wait(TimeSpan? timeout = null)
            {
                if (timeout.HasValue)
                {
                    this.Task.Wait(timeout.Value);
                }
                else
                {
                    this.Task.Wait();
                }
            }
            // Private Methods (1) 

            private static void SetCallback(Action<IItemListExecutionContext<T, S>> newCallback,
                                            ref Action<IItemListExecutionContext<T, S>> callbackField,
                                            ref Action<IItemListExecutionContext<T>> baseCallback)
            {
                callbackField = newCallback;

                if (newCallback != null)
                {
                    baseCallback = new Action<IItemListExecutionContext<T>>((ctx) =>
                        {
                            newCallback((IItemListExecutionContext<T, S>)ctx);
                        });
                }
                else
                {
                    baseCallback = null;
                }
            }

            #endregion Methods
        }

        #endregion Nested Classes
    }
}
