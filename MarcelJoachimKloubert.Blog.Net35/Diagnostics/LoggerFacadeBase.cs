// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Basis-Klasse für ein <see cref="ILoggerFacade" /> Objekt.
    /// </summary>
    public abstract class LoggerFacadeBase : ILoggerFacade
    {
        #region Fields (2)

        private readonly Action<ILogMessage> _ON_LOG_ACTION;
        /// <summary>
        /// Ein eindeutiges Objekt für Thread-sichere Operationen.
        /// </summary>
        protected readonly object _SYNC = new object();

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="LoggerFacadeBase" />.
        /// </summary>
        /// <param name="isThreadSafe">Log-Vorgänge Thread-sicher ausführen oder nicht.</param>
        protected LoggerFacadeBase(bool isThreadSafe)
        {
            if (isThreadSafe)
            {
                this._ON_LOG_ACTION = this.OnLog_ThreadSafe;
            }
            else
            {
                this._ON_LOG_ACTION = this.OnLog;
            }
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="LoggerFacadeBase" />.
        /// </summary>
        /// <remarks>Das verarbeiten Log-Vorgängen geschieht Thread-safe.</remarks>
        protected LoggerFacadeBase()
            : this(true)
        {

        }

        #endregion Constructors

        #region Methods (9)

        // Public Methods (5) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILoggerFacade.Log(object)" />
        public void Log(object msg)
        {
            this.LogInner(DateTimeOffset.Now,
                          Assembly.GetCallingAssembly(),
                          null,
                          null,
                          msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILoggerFacade.Log(ILogMessage)" />
        public void Log(ILogMessage msgObj)
        {
            if (msgObj == null)
            {
                throw new ArgumentNullException("msgObj");
            }

            this.OnLog(msgObj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILoggerFacade.Log(object, IEnumerable{char})" />
        public void Log(object msg, IEnumerable<char> tag)
        {
            this.LogInner(DateTimeOffset.Now,
                          Assembly.GetCallingAssembly(),
                          null,
                          AsString(tag),
                          msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILoggerFacade.Log(object, LoggerFacadeCategories)" />
        public void Log(object msg, LoggerFacadeCategories categories)
        {
            this.LogInner(DateTimeOffset.Now,
                          Assembly.GetCallingAssembly(),
                          categories,
                          null,
                          msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILoggerFacade.Log(object, IEnumerable{char}, LoggerFacadeCategories)" />
        public void Log(object msg, IEnumerable<char> tag, LoggerFacadeCategories categories)
        {
            this.LogInner(DateTimeOffset.Now,
                          Assembly.GetCallingAssembly(),
                          categories,
                          AsString(tag),
                          msg);
        }
        // Protected Methods (2) 

        /// <summary>
        /// Gibt eine Zeichenfolge als <see cref="string" /> zurück.
        /// </summary>
        /// <param name="charSequence">Die umzuwandelnde Sequenz</param>
        /// <returns>Die umgewandelete Zeichenfolge.</returns>
        protected static string AsString(IEnumerable<char> charSequence)
        {
            if (charSequence is string)
            {
                return (string)charSequence;
            }

            if (charSequence == null)
            {
                return null;
            }

            if (charSequence is char[])
            {
                return new string((char[])charSequence);
            }

            return new string(charSequence.ToArray());
        }

        /// <summary>
        /// Die Logger-Logik.
        /// </summary>
        /// <param name="msg">Die zu loggende Lognachricht.</param>
        protected abstract void OnLog(ILogMessage msg);
        // Private Methods (2) 

        private void LogInner(DateTimeOffset time,
                              Assembly asm,
                              LoggerFacadeCategories? categories,
                              string tag,
                              object msg)
        {
            try
            {
                if (DBNull.Value.Equals(msg))
                {
                    msg = null;
                }

                if (msg is IEnumerable<char>)
                {
                    msg = AsString((IEnumerable<char>)msg);
                }

                var thread = Thread.CurrentThread;

                MemberInfo member = null;
                try
                {
                    var st = new StackTrace(thread, false);
                    var sf = st.GetFrame(2);

                    member = sf.GetMethod();
                }
                catch
                {
                    member = null;
                }

                this._ON_LOG_ACTION(new SimpleLogMessage()
                    {
                        Assembly = asm,
                        Categories = categories.ToString()
                                               .Split(',')
                                               .Select(s => s.Trim())
                                               .Where(s => s != string.Empty)
                                               .Distinct()
                                               .Select(s => (LoggerFacadeCategories)Enum.Parse(typeof(LoggerFacadeCategories), s, false))
                                               .ToArray(),
                        Context = Thread.CurrentContext,
                        Id = Guid.NewGuid(),
                        Member = member,
                        Message = msg,
                        Principal = Thread.CurrentPrincipal,
                        Thread = thread,
                        Time = time,
                    });
            }
            catch
            {
                // Fehler beim Loggen ignorieren
            }
        }

        private void OnLog_ThreadSafe(ILogMessage msg)
        {
            lock (this._SYNC)
            {
                this.OnLog(msg);
            }
        }

        #endregion Methods
    }
}
