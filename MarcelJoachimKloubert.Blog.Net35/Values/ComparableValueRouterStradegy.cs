// s. http://blog.marcel-kloubert.de


using System;

namespace MarcelJoachimKloubert.Blog.Values
{
    /// <summary>
    /// Liste mit Strategien für <see cref="IValueRouter{TValue}" />, die mit
    /// Werten arbeiten, die <see cref="IComparable" /> und/oder
    /// <see cref="IComparable{TValue}" /> sind.
    /// </summary>
    public enum ComparableValueRouterStradegy
    {
        /// <summary>
        /// Größter Wert gewinnt
        /// </summary>
        Maximum,

        /// <summary>
        /// Kleinster Wert gewinnt 
        /// </summary>
        Minimum,
    }
}
