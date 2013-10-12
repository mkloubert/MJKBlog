// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Basis-Klasse für ein <see cref="ILoggerFacade" /> Objekt.
    /// </summary>
    public abstract class LoggerFacadeBase : ILoggerFacade
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="LoggerFacadeBase" />.
        /// </summary>
        protected LoggerFacadeBase()
        {

        }

        #endregion Constructors

        #region Methods (8)

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
        // Protected Methods (1) 

        /// <summary>
        /// Die Logger-Logik.
        /// </summary>
        /// <param name="msg">Die zu loggende Lognachricht.</param>
        protected abstract void OnLog(ILogMessage msg);
        // Private Methods (2) 

        private static string AsString(IEnumerable<char> charSequence)
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

                MemberInfo member = null;
                try
                {
                    var st = new StackTrace();
                    var sf = st.GetFrame(2);

                    member = sf.GetMethod();
                }
                catch
                {
                    member = null;
                }

                this.OnLog(new SimpleLogMessage()
                    {
                        Assembly = asm,
                        Categories = categories.ToString()
                                               .Split(',')
                                               .Select(s => s.Trim())
                                               .Where(s => s != string.Empty)
                                               .Distinct()
                                               .Select(s => (LoggerFacadeCategories)Enum.Parse(typeof(LoggerFacadeCategories), s, false))
                                               .ToArray(),
                        Member = member,
                        Message = msg,
                        Time = time,
                    });
            }
            catch
            {
                // Fehler beim Loggen ignorieren
            }
        }

        #endregion Methods
    }
}
