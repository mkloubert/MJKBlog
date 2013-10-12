// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Reflection;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Einfache Implementierung von <see cref="ILogMessage" />.
    /// </summary>
    public sealed class SimpleLogMessage : ILogMessage
    {
        #region Properties (5)

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILogMessage.Assembly" />
        public Assembly Assembly
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILogMessage.Categories" />
        public IList<LoggerFacadeCategories> Categories
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILogMessage.Member" />
        public MemberInfo Member
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILogMessage.Message" />
        public object Message
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILogMessage.Time" />
        public DateTimeOffset Time
        {
            get;
            set;
        }

        #endregion Properties
    }
}
