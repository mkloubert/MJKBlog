// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Linq;

namespace MarcelJoachimKloubert.Blog.Values
{
    /// <summary>
    /// Ein <see cref="IValueRouter{TValue}" /> für
    /// <see cref="IComparable" /> basierende Werte.
    /// </summary>
    /// <typeparam name="TValue">Typ des zugrundeliegenden Wertes</typeparam>
    public class ComparableValueRouter<TValue> : ValueRouterBase<TValue>
        where TValue : global::System.IComparable
    {
        #region Fields (1)

        private ComparableValueRouterStradegy _stradegy;

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ComparableValueRouter{TValue}" />.
        /// </summary>
        /// <param name="initalValue">
        /// Der initiale Wert für <see cref="ValueRouterBase{TValue}.MyValue" />.
        /// </param>
        public ComparableValueRouter(TValue initalValue)
            : base(initalValue)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ComparableValueRouter{TValue}" />.
        /// </summary>
        public ComparableValueRouter()
            : base()
        {

        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gibt die Strategie zurück, die verwendet wird, um zwei Werte miteinander
        /// zu vergleichen´, oder legt diesen Wert fest.
        /// </summary>
        public ComparableValueRouterStradegy Stradegy
        {
            get { return this._stradegy; }

            set
            {
                if (!EqualityComparer<ComparableValueRouterStradegy>.Default.Equals(this._stradegy, value))
                {
                    this.OnPropertyChanging(() => this.Stradegy);
                    this._stradegy = value;
                    this.OnPropertyChanged(() => this.Stradegy);

                    this.OnPropertyChanged(() => this.RoutedValue);
                }
            }
        }

        #endregion Properties

        #region Methods (5)

        // Public Methods (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ValueRouterBase{TValue}.CalculateRoutedDescription()" />
        public override string CalculateRoutedDescription()
        {
            var stradegyFunc = this.GetStradegyFunc();

            var result = this.MyDescription;

            var currentValue = this.MyValue;
            foreach (var mediatorData in this.GetMediators()
                                             .Select(m => new
                                                     {
                                                         Description = m.RoutedDescription,
                                                         Value = m.RoutedValue,
                                                     }))
            {
                var computedValue = stradegyFunc(currentValue, mediatorData.Value);
                if (!EqualityComparer<TValue>.Default.Equals(currentValue, computedValue))
                {
                    currentValue = computedValue;
                    result = mediatorData.Description;
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ValueRouterBase{TValue}.CalculateRoutedValue()" />
        public override TValue CalculateRoutedValue()
        {
            var stradegyFunc = this.GetStradegyFunc();

            TValue result = this.MyValue;
            foreach (var mediatorValue in this.GetMediators()
                                              .Select(m => m.RoutedValue))
            {
                result = stradegyFunc(result, mediatorValue);
            }

            return result;
        }
        // Protected Methods (3) 

        /// <summary>
        /// Gibt die Funktion zurück, die einen Wert auf Basis von zwei Eingabeparametern
        /// liefert und über die <see cref="ComparableValueRouter{TValue}.Stradegy" />
        /// Eigenschaft definiert ist.
        /// </summary>
        /// <returns>Die Funktion.</returns>
        protected virtual Func<TValue, TValue, TValue> GetStradegyFunc()
        {
            switch (this.Stradegy)
            {
                case ComparableValueRouterStradegy.Minimum:
                    return Min;

                default:
                    return Max;
            }
        }

        /// <summary>
        /// Gibt den kleineren von zwei Werten zurück.
        /// </summary>
        /// <param name="x">Der linke Wert.</param>
        /// <param name="y">Der rechte Wert.</param>
        /// <returns>Der kleinere Wert.</returns>
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

        #endregion Methods
    }
}
