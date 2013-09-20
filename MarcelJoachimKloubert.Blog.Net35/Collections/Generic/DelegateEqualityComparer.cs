// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Blog.Collections.Generic
{
    /// <summary>
    /// Ein genrischer <see cref="EqualityComparer{T}" />, der auf Delegates basiert.
    /// </summary>
    /// <typeparam name="T">Typ der zu prüfenden Objekte.</typeparam>
    public sealed class DelegateEqualityComparer<T> : EqualityComparer<T>
    {
        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="DelegateEqualityComparer{T}" /> Klasse.
        /// </summary>
        /// <param name="equ">
        /// Der Wert für die <see cref="DelegateEqualityComparer{T}.EqualsFunc" />
        /// Eigenschaft.
        /// </param>
        /// <param name="hashCode">
        /// Der Wert für die <see cref="DelegateEqualityComparer{T}.GetHashCodeFunc" />
        /// Eigenschaft.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="equ" /> und/oder <paramref name="hashCode" /> sind eine
        /// <see langword="null" /> Referenz.
        /// </exception>
        public DelegateEqualityComparer(Func<T, T, bool> equ,
                                        Func<T, int> hashCode)
        {
            if (equ == null)
            {
                throw new ArgumentNullException("equ");
            }

            if (hashCode == null)
            {
                throw new ArgumentNullException("hashCode");
            }

            this.EqualsFunc = equ;
            this.GetHashCodeFunc = hashCode;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="DelegateEqualityComparer{T}" /> Klasse.
        /// </summary>
        /// <param name="equ">
        /// Der Wert für die <see cref="DelegateEqualityComparer{T}.EqualsFunc" />
        /// Eigenschaft.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="equ" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public DelegateEqualityComparer(Func<T, T, bool> equ)
            : this(equ, DefaultGetHashCodeFunc)
        {

        }

        #endregion Constructors

        #region Properties (2)

        /// <summary>
        /// Gibt das Delegate für die <see cref="DelegateEqualityComparer{T}.Equals(T, T)" />
        /// Methode zurück.
        /// </summary>
        public Func<T, T, bool> EqualsFunc
        {
            get;
            private set;
        }

        /// <summary>
        /// Gibt das Delegate für die <see cref="DelegateEqualityComparer{T}.GetHashCode(T)" />
        /// Methode zurück.
        /// </summary>
        public Func<T, int> GetHashCodeFunc
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods (3)

        // Public Methods (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="EqualityComparer{T}.Equals(T, T)" />
        public override bool Equals(T x, T y)
        {
            return this.EqualsFunc(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="EqualityComparer{T}.GetHashCode(T)" />
        public override int GetHashCode(T obj)
        {
            return this.GetHashCodeFunc(obj);
        }
        // Private Methods (1) 

        private static int DefaultGetHashCodeFunc(T obj)
        {
            return obj != null ? obj.GetHashCode() : 0;
        }

        #endregion Methods
    }
}
