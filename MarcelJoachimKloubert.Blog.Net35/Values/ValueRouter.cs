﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MarcelJoachimKloubert.Blog.Values
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class ValueRouter<TValue> : IValueRouter<TValue> where TValue : global::System.IComparable, global::System.IComparable<TValue>
    {
        #region Fields (3)

        private readonly HashSet<IValueRouter<TValue>> _MEDIATORS = new HashSet<IValueRouter<TValue>>();
        private TValue _myValue;
        private readonly HashSet<IValueRouter<TValue>> _OBSERVERS = new HashSet<IValueRouter<TValue>>();

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ValueRouter{TValue}" />.
        /// </summary>
        /// <param name="initalValue">
        /// Der initiale Wert für <see cref="ValueRouter{TValue}.MyValue" />.
        /// </param>
        public ValueRouter(TValue initalValue)
        {
            this.MyValue = initalValue;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ValueRouter{TValue}" />.
        /// </summary>
        public ValueRouter()
            : this(default(TValue))
        {

        }

        #endregion Constructors

        #region Properties (2)

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.MyValue" />
        public TValue MyValue
        {
            get { return this._myValue; }

            set
            {
                if (!EqualityComparer<TValue>.Default.Equals(this._myValue, value))
                {
                    this.OnPropertyChanging(() => this.MyValue);
                    this._myValue = value;
                    this.OnPropertyChanged(() => this.MyValue);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.RoutedValue" />
        public TValue RoutedValue
        {
            get { return this.CalculateRoutedValue(); }
        }

        #endregion Properties

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

        #region Methods (13)

        // Public Methods (5) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.AddMediator(IValueRouter{TValue})" />
        public void AddMediator(IValueRouter<TValue> router)
        {
            if (router == null)
            {
                throw new ArgumentNullException("router");
            }

            if (this._MEDIATORS.Add(router))
            {
                router.PropertyChanged += this.Router_PropertyChanged;

                router.AddObserver(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.AddObserver(IValueRouter{TValue})" />
        public void AddObserver(IValueRouter<TValue> router)
        {
            if (router == null)
            {
                throw new ArgumentNullException("router");
            }

            if (this._OBSERVERS.Add(router))
            {
                router.AddMediator(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.CalculateRoutedValue()" />
        public virtual TValue CalculateRoutedValue()
        {
            TValue result = this.MyValue;
            foreach (var mediatorValue in this.GetMediators()
                                              .Select(m => m.RoutedValue))
            {
                result = Max(result, mediatorValue);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.GetMediators()" />
        public IList<IValueRouter<TValue>> GetMediators()
        {
            return new List<IValueRouter<TValue>>(this._MEDIATORS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.GetObservers()" />
        public IList<IValueRouter<TValue>> GetObservers()
        {
            return new List<IValueRouter<TValue>>(this._OBSERVERS);
        }
        // Protected Methods (6) 

        /// <summary>
        /// Gibt den grösseren von zwei Werten zurück.
        /// </summary>
        /// <param name="x">Der linke Wert.</param>
        /// <param name="y">Der rechte Wert.</param>
        /// <returns>Der grössere Wert.</returns>
        protected static TValue Max(TValue x, TValue y)
        {
            if (x == null && y == null)
            {
                return x;
            }

            var left = x != null ? x : y;
            var right = x != null ? y : x;

            if (left.CompareTo(right) < 0)
            {
                return right;
            }
            else
            {
                return left;
            }
        }

        /// <summary>
        /// Gibt den kleineren von zwei Werten zurück.
        /// </summary>
        /// <param name="x">Der linke Wert.</param>
        /// <param name="y">Der rechte Wert.</param>
        /// <returns>Der kleinere Wert.</returns>
        protected static TValue Min(TValue x, TValue y)
        {
            if (x == null && y == null)
            {
                return x;
            }

            var left = x != null ? x : y;
            var right = x != null ? y : x;

            if (left.CompareTo(right) > 0)
            {
                return right;
            }
            else
            {
                return left;
            }
        }

        /// <summary>
        /// Führt das <see cref="ValueRouter{TValue}.PropertyChanged" />
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
        /// Führt das <see cref="ValueRouter{TValue}.PropertyChanged" />
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

            if (global::System.ComponentModel.TypeDescriptor.GetProperties(this)[pn] == null)
            {
                throw new global::System.MissingMemberException(this.GetType().FullName,
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
        /// Führt das <see cref="ValueRouter{TValue}.PropertyChanging" />
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
        /// Führt das <see cref="ValueRouter{TValue}.PropertyChanging" />
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

            if (global::System.ComponentModel.TypeDescriptor.GetProperties(this)[pn] == null)
            {
                throw new global::System.MissingMemberException(this.GetType().FullName,
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
        // Private Methods (2) 

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

        private void Router_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var router = (IValueRouter<TValue>)sender;

            if (e.PropertyName == this.GetMemberName(vr => vr.MyValue))
            {
                this.OnPropertyChanged(() => this.RoutedValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="object.ToString()" />.
        public override string ToString()
        {
            return string.Format("{0}={1}",
                                 this.GetType().Name,
                                 this.RoutedValue);
        }

        #endregion Methods
    }
}
