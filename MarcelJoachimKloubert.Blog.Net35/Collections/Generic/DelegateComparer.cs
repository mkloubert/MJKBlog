// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Blog.Collections.Generic
{
    /// <summary>
    /// Ein <see cref="IComparer{T}" /> der auf Delegates basiert.
    /// </summary>
    /// <typeparam name="T">Zugrundeliegender Typ.</typeparam>
    public sealed class DelegateComparer<T> : Comparer<T>
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="DelegateComparer{T}" /> Klasse.
        /// </summary>
        /// <param name="compareFunc">
        /// Der Wert für die <see cref="DelegateComparer{T}.CompareFunc" /> Eigenschaft.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="compareFunc" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        public DelegateComparer(Func<T, T, int> compareFunc)
        {
            if (compareFunc == null)
            {
                throw new ArgumentNullException("compareFunc");
            }

            this.CompareFunc = compareFunc;
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Speichert die Logik für die <see cref="DelegateComparer{T}.Compare(T, T)" />
        /// Methode.
        /// </summary>
        public Func<T, T, int> CompareFunc
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="Comparer{T}.Compare(T, T)" />
        public override int Compare(T x, T y)
        {
            return this.CompareFunc(x, y);
        }

        #endregion Methods
    }
}
