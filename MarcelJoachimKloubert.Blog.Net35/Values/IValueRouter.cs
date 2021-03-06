﻿// s. http://blog.marcel-kloubert.de


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
        #region Data Members (9)

        /// <summary>
        /// Gibt das Objekt zurück, das mit dieser Instanz verlinkt werden soll oder nicht,
        /// oder legt dieses fest.
        /// </summary>
        object DataContext { get; set; }

        /// <summary>
        /// Gibt die Langfassung des aktuellen Status zurück, oder legt diesen fest.
        /// </summary>
        string MyDescription { get; set; }

        /// <summary>
        /// Gibt den Titel des aktuellen Status zurück, oder legt diesen fest.
        /// </summary>
        string MyTitle { get; set; }

        /// <summary>
        /// Gibt den Wert dieses Routers zurück oder legt diesen fest.
        /// </summary>
        TValue MyValue { get; set; }

        /// <summary>
        /// Gibt Namen dieser Instanz zurück, oder legt dieses fest.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gibt die eskalierte Beschreibung zurück.
        /// </summary>
        string RoutedDescription { get; }

        /// <summary>
        /// 
        /// </summary>
        IValueRouter<TValue> RoutedSource { get; }

        /// <summary>
        /// Gibt den eskalierten Titel zurück.
        /// </summary>
        string RoutedTitle { get; }

        /// <summary>
        /// Gibt den eskalierten Wert zurück.
        /// </summary>
        TValue RoutedValue { get; }

        #endregion Data Members

        #region Operations (9)

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
        /// Gibt den <see cref="IValueRouter{TValue}" />, der die gerouteten
        /// Daten liefert.
        /// </summary>
        /// <returns>
        /// Der <see cref="IValueRouter{TValue}" />, der die gerouteten
        /// Daten liefert.
        /// </returns>
        IValueRouter<TValue> CalculateRoutedSource();

        /// <summary>
        /// Leert die Liste der Router, die ihren Wert an diese Instanz weiterleiten / melden.
        /// </summary>
        void ClearMediators();

        /// <summary>
        /// Leert die Liste der Router, die ihren Wert von dieser Instanz weitergeleitet /
        /// gemeldet bekommen.
        /// </summary>
        void ClearObservers();

        /// <summary>
        /// Gibt die aktuelle Liste aller Router zurück, die dieser Instanz ihren
        /// Wert weiterleiten / melden.
        /// </summary>
        /// <returns>Die Liste der weiterleitenden Router.</returns>
        IList<IValueRouter<TValue>> GetMediators();

        /// <summary>
        /// Gibt die aktuelle Liste aller Router zurück, die von dieser Instanz ihren
        /// Wert weitergeleitet / gemeldet bekommen.
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
