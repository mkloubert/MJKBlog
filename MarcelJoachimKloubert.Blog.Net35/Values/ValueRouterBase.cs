// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MarcelJoachimKloubert.Blog.Values
{
    /// <summary>
    /// Ein Basis <see cref="IValueRouter{TValue}" />.
    /// </summary>
    /// <typeparam name="TValue">Typ des zugrundeliegenden Wertes.</typeparam>
    public abstract class ValueRouterBase<TValue> : IValueRouter<TValue>
    {
        #region Fields (8)

        private object _dataContext;
        private readonly HashSet<IValueRouter<TValue>> _MEDIATORS = new HashSet<IValueRouter<TValue>>();
        private string _myDescription;
        private TValue _myValue;
        private string _name;
        private readonly HashSet<IValueRouter<TValue>> _OBSERVERS = new HashSet<IValueRouter<TValue>>();
        /// <summary>
        /// Eindeutiges Objekt für Thread-sichere Operationen.
        /// </summary>
        protected readonly object _SYNC = new object();
        private string _title;

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ValueRouterBase{TValue}" />.
        /// </summary>
        /// <param name="initalValue">
        /// Der initiale Wert für <see cref="ValueRouterBase{TValue}.MyValue" />.
        /// </param>
        public ValueRouterBase(TValue initalValue)
        {
            this.MyValue = initalValue;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ValueRouterBase{TValue}" />.
        /// </summary>
        public ValueRouterBase()
            : this(default(TValue))
        {

        }

        #endregion Constructors

        #region Properties (7)

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.DataContext" />
        public object DataContext
        {
            get { return this._dataContext; }

            set
            {
                if (!EqualityComparer<object>.Default.Equals(this._dataContext, value))
                {
                    this.OnPropertyChanging(() => this.DataContext);
                    this._dataContext = value;
                    this.OnPropertyChanged(() => this.DataContext);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.MyDescription" />
        public string MyDescription
        {
            get { return this._myDescription; }

            set
            {
                if (!EqualityComparer<string>.Default.Equals(this._myDescription, value))
                {
                    this.OnPropertyChanging(() => this.MyDescription);
                    this._myDescription = value;
                    this.OnPropertyChanged(() => this.MyDescription);

                    this.OnPropertyChanged(() => this.RoutedDescription);
                }
            }
        }

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

                    this.OnPropertyChanged(() => this.RoutedValue);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.Name" />
        public string Name
        {
            get { return this._name; }

            set
            {
                if (!EqualityComparer<string>.Default.Equals(this._name, value))
                {
                    this.OnPropertyChanging(() => this.Name);
                    this._name = value;
                    this.OnPropertyChanged(() => this.Name);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.RoutedDescription" />
        public string RoutedDescription
        {
            get { return this.CalculateRoutedDescription(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.RoutedValue" />
        public TValue RoutedValue
        {
            get { return this.CalculateRoutedValue(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.Title" />
        public string Title
        {
            get { return this._title; }

            set
            {
                if (!EqualityComparer<string>.Default.Equals(this._title, value))
                {
                    this.OnPropertyChanging(() => this.Title);
                    this._title = value;
                    this.OnPropertyChanged(() => this.Title);
                }
            }
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

        #region Methods (17)

        // Public Methods (11) 

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

            lock (this._SYNC)
            {
                if (this._MEDIATORS.Add(router))
                {
                    router.PropertyChanged += this.Router_PropertyChanged;

                    router.AddObserver(this);
                }
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

            lock (this._SYNC)
            {
                if (this._OBSERVERS.Add(router))
                {
                    router.AddMediator(this);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.CalculateRoutedDescription()" />
        public abstract string CalculateRoutedDescription();

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.CalculateRoutedValue()" />
        public abstract TValue CalculateRoutedValue();

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.ClearMediators()" />
        public void ClearMediators()
        {
            foreach (var mediator in this.GetMediators())
            {
                this.RemoveMediator(mediator);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.ClearObservers()" />
        public void ClearObservers()
        {
            foreach (var observer in this.GetObservers())
            {
                this.RemoveObserver(observer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.GetMediators()" />
        public IList<IValueRouter<TValue>> GetMediators()
        {
            lock (this._SYNC)
            {
                return new List<IValueRouter<TValue>>(this._MEDIATORS);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.GetObservers()" />
        public IList<IValueRouter<TValue>> GetObservers()
        {
            lock (this._SYNC)
            {
                return new List<IValueRouter<TValue>>(this._OBSERVERS);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.RemoveMediator(IValueRouter{TValue})" />
        public void RemoveMediator(IValueRouter<TValue> router)
        {
            if (router == null)
            {
                throw new ArgumentNullException("router");
            }

            lock (this._SYNC)
            {
                if (this._MEDIATORS.Remove(router))
                {
                    router.PropertyChanged -= this.Router_PropertyChanged;

                    router.RemoveObserver(this);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueRouter{TValue}.RemoveObserver(IValueRouter{TValue})" />
        public void RemoveObserver(IValueRouter<TValue> router)
        {
            if (router == null)
            {
                throw new ArgumentNullException("router");
            }

            lock (this._SYNC)
            {
                if (this._OBSERVERS.Remove(router))
                {
                    router.RemoveMediator(this);
                }
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
        // Protected Methods (4) 

        /// <summary>
        /// Führt das <see cref="ValueRouterBase{TValue}.PropertyChanged" />
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
        /// Führt das <see cref="ValueRouterBase{TValue}.PropertyChanged" />
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
        /// Führt das <see cref="ValueRouterBase{TValue}.PropertyChanging" />
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
        /// Führt das <see cref="ValueRouterBase{TValue}.PropertyChanging" />
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
            else if (e.PropertyName == this.GetMemberName(vr => vr.MyDescription))
            {
                this.OnPropertyChanged(() => this.RoutedDescription);
            }
        }

        #endregion Methods
    }
}
