// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Blog.Threading.Tasks
{
    /// <summary>
    /// Hilfs-/Erweiterungsklasse für (statische) <see cref="Task" />-Operationen.
    /// </summary>
    public static class TaskHelper
    {
        #region Methods (12)

        // Public Methods (12) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Task.WaitAll(Task[])" />
        public static void WaitAll(IEnumerable<Task> tasks)
        {
            var array = tasks.AsArray();
            if (array == null)
            {
                throw new ArgumentNullException("tasks");
            }

            Task.WaitAll(array);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Task.WaitAll(Task[], int)" />
        public static void WaitAll(IEnumerable<Task> tasks,
                                   int msecTimeout)
        {
            var array = tasks.AsArray();
            if (array == null)
            {
                throw new ArgumentNullException("tasks");
            }

            Task.WaitAll(tasks: array,
                         millisecondsTimeout: msecTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Task.WaitAll(Task[], TimeSpan)" />
        public static void WaitAll(IEnumerable<Task> tasks,
                                   TimeSpan timeout)
        {
            var array = tasks.AsArray();
            if (array == null)
            {
                throw new ArgumentNullException("tasks");
            }

            Task.WaitAll(tasks: array,
                         timeout: timeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Task.WaitAll(Task[], CancellationToken)" />
        public static void WaitAll(IEnumerable<Task> tasks,
                                   CancellationToken token)
        {
            var array = tasks.AsArray();
            if (array == null)
            {
                throw new ArgumentNullException("tasks");
            }

            Task.WaitAll(tasks: array,
                         cancellationToken: token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Task.WaitAll(Task[], int, CancellationToken)" />
        public static void WaitAll(IEnumerable<Task> tasks,
                                   int msecTimeout,
                                   CancellationToken token)
        {
            var array = tasks.AsArray();
            if (array == null)
            {
                throw new ArgumentNullException("tasks");
            }

            Task.WaitAll(tasks: array,
                         millisecondsTimeout: msecTimeout,
                         cancellationToken: token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TaskHelper.WaitAll(IEnumerable{Task}, int, CancellationToken)" />
        public static void WaitAll(IEnumerable<Task> tasks,
                                   TimeSpan timeout,
                                   CancellationToken token)
        {
            WaitAll(tasks: tasks,
                    msecTimeout: (int)timeout.TotalMilliseconds,
                    token: token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Task.WaitAny(Task[])" />
        public static int WaitAny(IEnumerable<Task> tasks)
        {
            var array = tasks.AsArray();
            if (array == null)
            {
                throw new ArgumentNullException("tasks");
            }

            return Task.WaitAny(array);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Task.WaitAny(Task[], TimeSpan)" />
        public static int WaitAny(IEnumerable<Task> tasks,
                                  TimeSpan timeout)
        {
            var array = tasks.AsArray();
            if (array == null)
            {
                throw new ArgumentNullException("tasks");
            }

            return Task.WaitAny(tasks: array,
                                timeout: timeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Task.WaitAny(Task[], int)" />
        public static int WaitAny(IEnumerable<Task> tasks,
                                  int msecTimeout)
        {
            var array = tasks.AsArray();
            if (array == null)
            {
                throw new ArgumentNullException("tasks");
            }

            return Task.WaitAny(tasks: array,
                                millisecondsTimeout: msecTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Task.WaitAny(Task[], CancellationToken)" />
        public static int WaitAny(IEnumerable<Task> tasks,
                                  CancellationToken token)
        {
            var array = tasks.AsArray();
            if (array == null)
            {
                throw new ArgumentNullException("tasks");
            }

            return Task.WaitAny(tasks: array,
                                cancellationToken: token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="TaskHelper.WaitAny(IEnumerable{Task}, int, CancellationToken)" />
        public static int WaitAny(IEnumerable<Task> tasks,
                                  TimeSpan timeout,
                                  CancellationToken token)
        {
            return WaitAny(tasks: tasks,
                           msecTimeout: (int)timeout.TotalMilliseconds,
                           token: token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Task.WaitAny(Task[], int, CancellationToken)" />
        public static int WaitAny(IEnumerable<Task> tasks,
                                  int msecTimeout,
                                  CancellationToken token)
        {
            var array = tasks.AsArray();
            if (array == null)
            {
                throw new ArgumentNullException("tasks");
            }

            return Task.WaitAny(tasks: array,
                                millisecondsTimeout: msecTimeout,
                                cancellationToken: token);
        }

        #endregion Methods
    }
}
