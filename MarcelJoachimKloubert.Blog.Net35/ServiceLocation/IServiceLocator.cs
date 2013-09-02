// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Blog.ServiceLocation
{
    /// <summary>
    /// Beschreibt einen Service Locator.
    /// </summary>
    public interface IServiceLocator : IServiceProvider
    {
        #region Operations (6)

        /// <summary>
        /// Gibt sämtliche Instanzen eines Dienstes zurück.
        /// </summary>
        /// <typeparam name="S">Typ des Dienstes.</typeparam>
        /// <returns>Sämtliche Instanzen des Dienstes.</returns>
        /// <exception cref="ServiceActivationException">
        /// Beim Ermitteln/Erzeugen der Instanzen ist mindestens ein Fehler aufgetreten.
        /// </exception>
        IEnumerable<S> GetAllInstances<S>();

        /// <summary>
        /// Gibt sämtliche Instanzen eines Dienstes zurück.
        /// </summary>
        /// <param name="serviceType">Typ des Dienstes.</param>
        /// <returns>Sämtliche Instanzen des Dienstes.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="ServiceActivationException">
        /// Beim Ermitteln/Erzeugen der Instanzen ist mindestens ein Fehler aufgetreten.
        /// </exception>
        IEnumerable<object> GetAllInstances(Type serviceType);

        /// <summary>
        /// Gibt die einzige Instanz eines Dienstes zurück.
        /// </summary>
        /// <typeparam name="S">Typ des Dienstes.</typeparam>
        /// <returns>Die Instanz des Dienstes.</returns>
        /// <exception cref="ServiceActivationException">
        /// Beim Ermitteln/Erzeugen der Instanz ist mindestens ein Fehler aufgetreten.
        /// </exception>
        S GetInstance<S>();

        /// <summary>
        /// Gibt die einzige Instanz eines Dienstes zurück mit
        /// einem bestimmten Schlüssel zurück.
        /// </summary>
        /// <param name="key">Der Schlüssel.</param>
        /// <typeparam name="S">Typ des Dienstes.</typeparam>
        /// <returns>Die Instanz des Dienstes.</returns>
        /// <exception cref="ServiceActivationException">
        /// Beim Ermitteln/Erzeugen der Instanz ist mindestens ein Fehler aufgetreten.
        /// </exception>
        S GetInstance<S>(object key);

        /// <summary>
        /// Gibt die einzige Instanz eines Dienstes zurück.
        /// </summary>
        /// <param name="serviceType">Typ des Dienstes.</param>
        /// <returns>Die Instanz des Dienstes.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="ServiceActivationException">
        /// Beim Ermitteln/Erzeugen der Instanz ist mindestens ein Fehler aufgetreten.
        /// </exception>
        object GetInstance(Type serviceType);

        /// <summary>
        /// Gibt die einzige Instanz eines Dienstes zurück.
        /// </summary>
        /// <param name="serviceType">Typ des Dienstes.</param>
        /// <param name="key">Der Schlüssel.</param>
        /// <returns>Die Instanz des Dienstes.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceType" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        /// <exception cref="ServiceActivationException">
        /// Beim Ermitteln/Erzeugen der Instanz ist mindestens ein Fehler aufgetreten.
        /// </exception>
        object GetInstance(Type serviceType, object key);

        #endregion Operations
    }
}
