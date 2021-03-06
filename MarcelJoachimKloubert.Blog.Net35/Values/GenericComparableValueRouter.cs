﻿// s. http://blog.marcel-kloubert.de


using System;
using System.Collections.Generic;
using System.Linq;

namespace MarcelJoachimKloubert.Blog.Values
{
    /// <summary>
    /// Ein <see cref="IValueRouter{TValue}" /> für
    /// <see cref="IComparable" /> basierende Werte.
    /// </summary>
    /// <typeparam name="TValue">Typ des zugrundeliegenden Wertes.</typeparam>
    public class GenericComparableValueRouter<TValue> : ValueRouterBase<TValue>
        where TValue : global::System.IComparable<TValue>
    {
        #region Fields (1)

        private ComparableValueRouterStradegy _stradegy;

        #endregion Fields

        #region Constructors (2)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="GenericComparableValueRouter{TValue}" />.
        /// </summary>
        /// <param name="initalValue">
        /// Der initiale Wert für <see cref="ValueRouterBase{TValue}.MyValue" />.
        /// </param>
        public GenericComparableValueRouter(TValue initalValue)
            : base(initalValue)
        {

        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="GenericComparableValueRouter{TValue}" />.
        /// </summary>
        public GenericComparableValueRouter()
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

        #region Methods (4)

        // Protected Methods (4) 

        /// <summary>
        /// Gibt die Funktion zurück, die einen Wert auf Basis von zwei Eingabeparametern
        /// liefert und über die <see cref="GenericComparableValueRouter{TValue}.Stradegy" />
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

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="ValueRouterBase{TValue}.OnCalculateRoutedSource()"/>
        protected override IValueRouter<TValue> OnCalculateRoutedSource()
        {
            var stradegyFunc = this.GetStradegyFunc();

            IValueRouter<TValue> result = this;

            var currentValue = this.MyValue;
            foreach (var mediatorData in this.GetMediators()
                                             .Select(m => new
                                             {
                                                 RouterObject = m,
                                                 Value = m.RoutedValue,
                                             }))
            {
                var computedValue = stradegyFunc(currentValue, mediatorData.Value);
                if (!EqualityComparer<TValue>.Default.Equals(currentValue, computedValue))
                {
                    currentValue = computedValue;
                    result = mediatorData.RouterObject;
                }
            }

            return result;
        }

        #endregion Methods
    }
}
