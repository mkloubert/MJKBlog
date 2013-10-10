// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MarcelJoachimKloubert.Blog.Values
{
    /// <summary>
    /// Beschreibt einen Router, der Werte in einem Pfad hocheskaliert.
    /// </summary>
    /// <typeparam name="TValue">Typ des zugrundeliegenden Wertes.</typeparam>
    public interface IValueRouter<TValue> : INotifyPropertyChanged, INotifyPropertyChanging
        where TValue : global::System.IComparable<TValue>
    {
        #region Data Members (2)

        /// <summary>
        /// Gibt den Wert dieses Routers zurück oder legt diesen fest.
        /// </summary>
        TValue MyValue { get; set; }

        /// <summary>
        /// Get den eskalierten Wert zurück.
        /// </summary>
        TValue RoutedValue { get; }

        #endregion Data Members

        #region Operations (5)

        /// <summary>
        /// Fügt einen Router hinzu, der seinen Wert an diese Instanz weiterleitet / meldet.
        /// </summary>
        /// <param name="router">Der hinzuzufügende Router.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="router" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        void AddMediator(IValueRouter<TValue> router);

        /// <summary>
        /// Fügt einen Router hinzu, der seinen Wert von dieser Instanz
        /// weitergeleitet / gemeldet bekommt.
        /// </summary>
        /// <param name="router">Der hinzuzufügende Router.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="router" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        void AddObserver(IValueRouter<TValue> router);

        /// <summary>
        /// Berechnet den Wert für <see cref="IValueRouter{TValue}.RoutedValue" />.
        /// </summary>
        /// <returns>Der berechnete Wert.</returns>
        TValue CalculateRoutedValue();

        /// <summary>
        /// Gibt die aktuelle Liste aller Router zurück, die dieser Instanz ihren
        /// Wert weiterleiten / melden.
        /// </summary>
        /// <returns>Die Liste der weiterleitenden Router.</returns>
        IList<IValueRouter<TValue>> GetMediators();

        /// <summary>
        /// Gibt die aktuelle Liste aller Router zurück, die von dieser Instanz ihren
        /// Wert weitergeleitet / gemelden gekommen.
        /// </summary>
        /// <returns>Die Liste der empfangenden Router.</returns>
        IList<IValueRouter<TValue>> GetObservers();

        #endregion Operations
    }
}
