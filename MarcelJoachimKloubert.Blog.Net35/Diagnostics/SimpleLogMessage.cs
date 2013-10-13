// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Threading;

namespace MarcelJoachimKloubert.Blog.Diagnostics
{
    /// <summary>
    /// Einfache Implementierung von <see cref="ILogMessage" />.
    /// </summary>
    public sealed class SimpleLogMessage : ILogMessage
    {
        #region Properties (9)

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
        /// <see cref="ILogMessage.Context" />
        public Context Context
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILogMessage.Id" />
        public Guid Id
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
        /// <see cref="ILogMessage.Principal" />
        public IPrincipal Principal
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ILogMessage.Thread" />
        public Thread Thread
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

        #region Methods (3)

        // Public Methods (3) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IEquatable{T}.Equals(T)" />
        public bool Equals(ILogMessage other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="object.Equals(object)" />
        public override bool Equals(object other)
        {
            if (other is ILogMessage)
            {
                return this.Equals((ILogMessage)other);
            }

            return base.Equals(other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="object.GetHashCode()" />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion Methods
    }
}
