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
    {
        #region Data Members (6)

        /// <summary>
        /// Gibt das Objekt zurück, das mit dieser Instanz verlinkt werden soll oder nicht,
        /// oder legt dieses fest.
        /// </summary>
        object DataContext { get; set; }

        /// <summary>
        /// Gibt die Langfassung des aktuellen Status zurück, oder legt diesen fest.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gibt den Wert dieses Routers zurück oder legt diesen fest.
        /// </summary>
        TValue MyValue { get; set; }

        /// <summary>
        /// Gibt Namen dieser Instanz zurück, oder legt dieses fest.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Get den eskalierten Wert zurück.
        /// </summary>
        TValue RoutedValue { get; }

        /// <summary>
        /// Gibt den Titel des aktuellen Status zurück, oder legt diesen fest.
        /// </summary>
        string Title { get; set; }

        #endregion Data Members

        #region Operations (7)

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

        /// <summary>
        /// Entfernt einen Router, der seinen Wert an diese Instanz weiterleitet / meldet.
        /// </summary>
        /// <param name="router">Der zu löschende Router.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="router" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        void RemoveMediator(IValueRouter<TValue> router);

        /// <summary>
        /// Entfernt einen Router, der seinen Wert von dieser Instanz
        /// weitergeleitet / gemeldet bekommt.
        /// </summary>
        /// <param name="router">Der zu löschende Router.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="router" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        void RemoveObserver(IValueRouter<TValue> router);

        #endregion Operations
    }
}
