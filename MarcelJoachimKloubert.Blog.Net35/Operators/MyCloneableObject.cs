// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Blog.Operators
{
    /// <summary>
    /// Klonbares Test-Objekt.
    /// </summary>
    public class MyCloneableObject : ICloneable
    {
        #region Methods (2)

        // Public Methods (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="MyCloneableObject.Clone()" />
        public object Clone()
        {
            return new MyCloneableObject();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="object.ToString()" />
        public override string ToString()
        {
            return this.GetType().FullName + "::" + this.GetHashCode();
        }

        #endregion Methods

        /// <summary>
        /// Klont ein Objekt mehrmals.
        /// </summary>
        /// <param name="count">Die Anzahl der zu klonenden Instanzen.</param>
        /// <param name="parent">Die zu klonende Instanz.</param>
        /// <returns>Die verzögerte Sequenz aus geklonten Instanzen.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parent" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> ist kleiner als 0.
        /// </exception>
        public static IEnumerable<MyCloneableObject> operator *(long count, MyCloneableObject parent)
        {
            return parent * count;
        }
        /// <summary>
        /// Klont ein Objekt mehrmals.
        /// </summary>
        /// <param name="parent">Die zu klonende Instanz.</param>
        /// <param name="count">Die Anzahl der zu klonenden Instanzen.</param>
        /// <returns>Die verzögerte Sequenz aus geklonten Instanzen.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parent" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> ist kleiner als 0.
        /// </exception>
        public static IEnumerable<MyCloneableObject> operator *(MyCloneableObject parent, long count)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (count == 0)
            {
                yield break;
            }

            yield return parent;

            for (long i = 1; i < count; i++)
            {
                yield return (MyCloneableObject)parent.Clone();
            }
        }
    }
}
