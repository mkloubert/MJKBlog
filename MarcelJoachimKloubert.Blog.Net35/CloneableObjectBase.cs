// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Blog
{
    /// <summary>
    /// Eine Basis-Klasse für ein klonbares Objekt.
    /// </summary>
    public abstract class CloneableObjectBase : ICloneable
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="CloneableObjectBase" />
        /// </summary>
        protected CloneableObjectBase()
        {

        }

        #endregion Constructors

        #region Methods (3)

        // Public Methods (1) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ICloneable.Clone()" />
        public CloneableObjectBase Clone()
        {
            var result = this.OnClone();
            if (result == null)
            {
                throw new NullReferenceException("result");
            }

            if (object.ReferenceEquals(this, result))
            {
                throw new InvalidOperationException();
            }

            return result;
        }
        // Protected Methods (1) 

        /// <summary>
        /// Die eigentliche Logik für <see cref="CloneableObjectBase.Clone()" />.
        /// </summary>
        /// <returns>Das geklonte Objekt.</returns>
        protected abstract CloneableObjectBase OnClone();
        // Private Methods (1) 

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion Methods

        /// <summary>
        /// Klont ein Objekt in einer gewissen Anzahl.
        /// </summary>
        /// <param name="parent">Das zu klonende Objekt.</param>
        /// <param name="count">Die Anzahl.</param>
        /// <returns>Die geklonten Instanzen als verzögerte Sequenz.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> ist kleiner als 0.
        /// </exception>
        /// <remarks>
        /// Das erste Element der Sequenz ist immer die Instanz aus <paramref name="parent" />.
        /// Ist <paramref name="parent" /> eine <see langword="null" /> Referenz, sind sämtliche
        /// Elemente der Rückgabesequenz ebenfalls <see langword="null" />.
        /// </remarks>
        public static IEnumerable<CloneableObjectBase> operator *(CloneableObjectBase parent, long count)
        {
            return count * parent;
        }

        /// <summary>
        /// Klont ein Objekt in einer gewissen Anzahl.
        /// </summary>
        /// <param name="parent">Das zu klonende Objekt.</param>
        /// <param name="count">Die Anzahl.</param>
        /// <returns>Die geklonten Instanzen als verzögerte Sequenz.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count" /> ist kleiner als 0.
        /// </exception>
        /// <remarks>
        /// Das erste Element der Sequenz ist immer die Instanz aus <paramref name="parent" />.
        /// Ist <paramref name="parent" /> eine <see langword="null" /> Referenz, sind sämtliche
        /// Elemente der Rückgabesequenz ebenfalls <see langword="null" />.
        /// </remarks>
        public static IEnumerable<CloneableObjectBase> operator *(long count, CloneableObjectBase parent)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (count == 0)
            {
                yield break;
            }

            // parent
            yield return parent;

            for (long i = 1; i < count; i++)
            {
                yield return parent != null ? parent.Clone() : null;
            }
        }
    }
}
