// s. http://blog.marcel-kloubert.de


using System;

namespace MarcelJoachimKloubert.Blog.Values
{
    /// <summary>
    /// Ein <see cref="ComparableValueRouter{TValue}" /> for Enumerationen.
    /// </summary>
    /// <typeparam name="TValue">Typ des Enums.</typeparam>
    public sealed class EnumValueRouter<TValue> : ComparableValueRouter<TValue>
        where TValue : struct, global::System.IComparable
    {
        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="EnumValueRouter{TValue}" />.
        /// </summary>
        /// <param name="initalValue">
        /// Der initiale Wert für <see cref="ValueRouterBase{TValue}.MyValue" />.
        /// </param>
        public EnumValueRouter(TValue initalValue)
            : base(initalValue)
        {
            CheckIfEnumOrThrow();
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="EnumValueRouter{TValue}" />.
        /// </summary>
        public EnumValueRouter()
            : base()
        {
            CheckIfEnumOrThrow();
        }

        #endregion Constructors

        #region Methods (1)

        // Private Methods (1) 

        private static void CheckIfEnumOrThrow()
        {
            if (!typeof(TValue).IsEnum)
            {
                throw new ArgumentException("TValue");
            }
        }

        #endregion Methods
    }
}
