// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MarcelJoachimKloubert.Blog.WPF
{
    /// <summary>
    /// Ein Basis-Objekt für ein ViewModel.
    /// </summary>
    public abstract partial class NotificationObjectBase : INotifyPropertyChanged,
                                                           INotifyPropertyChanging
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz
        /// der Klasse <see cref="NotificationObjectBase" />.
        /// </summary>
        protected NotificationObjectBase()
        {

        }

        #endregion Constructors

        #region Delegates and Events (2)

        // Events (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="INotifyPropertyChanged.PropertyChanged" />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="INotifyPropertyChanging.PropertyChanging" />
        public event PropertyChangingEventHandler PropertyChanging;

        #endregion Delegates and Events

        #region Methods (6)

        // Protected Methods (5) 

        /// <summary>
        /// Führt das <see cref="NotificationObjectBase.PropertyChanged" />
        /// Ereignis aus.
        /// </summary>
        /// <typeparam name="T">Typ der zugrundeliegenden Eigenschaft.</typeparam>
        /// <param name="expr">Die zugrundeliegende Eigenschaft aus Ausdruck.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="expr" /> ist ungültig.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="expr" /> ist kein gültiger Ausdruck, der eine
        /// Eigenschaft beschreibt.
        /// </exception>
        /// <returns>
        /// Ereignis wurde ausgeführt oder nicht, da kein Delegate an Ereignis
        /// registriert wurde.
        /// </returns>
        protected bool OnPropertyChanged<T>(Expression<Func<T>> expr)
        {
            if (expr == null)
            {
                throw new ArgumentNullException("expr");
            }

            var memberExpr = expr.Body as MemberExpression;
            if (memberExpr == null)
            {
                throw new ArgumentException("expr.Body");
            }

            var property = memberExpr.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("expr.Body.Member");
            }

            return this.OnPropertyChanged(propertyName: property.Name);
        }

        /// <summary>
        /// Führt das <see cref="NotificationObjectBase.PropertyChanged" />
        /// Ereignis aus.
        /// </summary>
        /// <param name="propertyName">Der Name der zugrundeliegenden Eigenschaft.</param>
        /// <returns>
        /// Ereignis wurde ausgeführt oder nicht, da kein Delegate an Ereignis
        /// registriert wurde.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="propertyName" /> ist ungültig.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyName" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        protected bool OnPropertyChanged(IEnumerable<char> propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            var pn = AsString(propertyName).Trim();
            if (pn == string.Empty)
            {
                throw new ArgumentException("propertyName");
            }
#if DEBUG

            if (TypeDescriptor.GetProperties(this)[pn] == null)
            {
                throw new MissingMemberException(this.GetType().FullName,
                                                 pn);
            }
#endif

            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(pn));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Führt das <see cref="NotificationObjectBase.PropertyChanging" />
        /// Ereignis aus.
        /// </summary>
        /// <typeparam name="T">Typ der zugrundeliegenden Eigenschaft.</typeparam>
        /// <param name="expr">Die zugrundeliegende Eigenschaft aus Ausdruck.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="expr" /> ist ungültig.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="expr" /> ist kein gültiger Ausdruck, der eine
        /// Eigenschaft beschreibt.
        /// </exception>
        /// <returns>
        /// Ereignis wurde ausgeführt oder nicht, da kein Delegate an Ereignis
        /// registriert wurde.
        /// </returns>
        protected bool OnPropertyChanging<T>(Expression<Func<T>> expr)
        {
            if (expr == null)
            {
                throw new ArgumentNullException("expr");
            }

            var memberExpr = expr.Body as MemberExpression;
            if (memberExpr == null)
            {
                throw new ArgumentException("expr.Body");
            }

            var property = memberExpr.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("expr.Body.Member");
            }

            return this.OnPropertyChanging(propertyName: property.Name);
        }

        /// <summary>
        /// Führt das <see cref="NotificationObjectBase.PropertyChanging" />
        /// Ereignis aus.
        /// </summary>
        /// <param name="propertyName">Der Name der zugrundeliegenden Eigenschaft.</param>
        /// <returns>
        /// Ereignis wurde ausgeführt oder nicht, da kein Delegate an Ereignis
        /// registriert wurde.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="propertyName" /> ist ungültig.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyName" /> ist eine <see langword="null" /> Referenz.
        /// </exception>
        protected bool OnPropertyChanging(IEnumerable<char> propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            var pn = AsString(propertyName).Trim();
            if (pn == string.Empty)
            {
                throw new ArgumentException("propertyName");
            }
#if DEBUG

            if (TypeDescriptor.GetProperties(this)[pn] == null)
            {
                throw new MissingMemberException(this.GetType().FullName,
                                                 pn);
            }
#endif

            var handler = this.PropertyChanging;
            if (handler != null)
            {
                handler(this, new PropertyChangingEventArgs(pn));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Setzt das Feld einer Eigenschaft dieses Objektes, sofern sich der
        /// alte und der neue Wert unterscheiden.
        /// </summary>
        /// <typeparam name="T">Typ der Eigenschaft.</typeparam>
        /// <param name="field">
        /// Der alte Wert bzw. das Feld, das den Wert der zugrundeliegenden
        /// Eigenschaft speichert.
        /// </param>
        /// <param name="newValue">Der neue Wert.</param>
        /// <param name="propertyName">
        /// Der Name der zugrundeliegenden Eigenschaft.
        /// </param>
        /// <returns>
        /// Die Werte <paramref name="field" /> und <paramref name="newValue" />
        /// gelten als verschieden oder nicht.
        /// </returns>
        /// <remarks>
        /// Der Wert für <paramref name="propertyName" /> wird i.d.R. automatisch
        /// vom Compiler gesetzt. Diese Methode sollte stehts aus der zugrundeliegenden
        /// Eigenschaft aufgerufen werden, sofern <paramref name="propertyName" />
        /// nicht explizit gesetzt wird.
        /// </remarks>
        protected bool SetProperty<T>(ref T field,
                                      T newValue,
                                      [CallerMemberName] IEnumerable<char> propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                this.OnPropertyChanging(propertyName);
                field = newValue;
                this.OnPropertyChanged(propertyName);

                return false;
            }

            return false;
        }
        // Private Methods (1) 

        private static string AsString(IEnumerable<char> charSequence)
        {
            if (charSequence is string)
            {
                return (string)charSequence;
            }

            if (charSequence == null)
            {
                return null;
            }

            if (charSequence is char[])
            {
                return new string((char[])charSequence);
            }

            return new string(charSequence.ToArray());
        }

        #endregion Methods
    }
}
