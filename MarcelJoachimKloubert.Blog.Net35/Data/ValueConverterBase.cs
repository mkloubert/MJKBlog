// s. http://blog.marcel-kloubert.de


using System;
using System.Globalization;
using System.Windows.Data;

namespace MarcelJoachimKloubert.Blog.Data
{
    #region ValueConverterBase<TSrc, TDest>

    /// <summary>
    /// Ein stark typisierter <see cref="IValueConverter" />.
    /// </summary>
    /// <typeparam name="TSrc">Der Quelltyp.</typeparam>
    /// <typeparam name="TDest">Der Zieltyp.</typeparam>
    public abstract class ValueConverterBase<TSrc, TDest> : ValueConverterBase<TSrc, TDest, object>
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ValueConverterBase{TSrc, TDest}" />.
        /// </summary>
        protected ValueConverterBase()
        {

        }

        #endregion Constructors
    }

    #endregion

    #region ValueConverterBase<TSrc, TDest, TParam>

    /// <summary>
    /// Ein stark typisierter <see cref="IValueConverter" />.
    /// </summary>
    /// <typeparam name="TSrc">Der Quelltyp.</typeparam>
    /// <typeparam name="TDest">Der Zieltyp.</typeparam>
    /// <typeparam name="TParam">Der Typ der Parameter.</typeparam>
    public abstract class ValueConverterBase<TSrc, TDest, TParam> : IValueConverter
    {
        #region Constructors (1)

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse <see cref="ValueConverterBase{TSrc, TDest, TParam}" />.
        /// </summary>
        protected ValueConverterBase()
        {

        }

        #endregion Constructors

        #region Methods (5)

        // Public Methods (2) 

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueConverter.Convert(object, Type, object, CultureInfo)" />
        public virtual TDest Convert(TSrc value, TParam parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="IValueConverter.ConvertBack(object, Type, object, CultureInfo)" />
        public virtual TSrc ConvertBack(TDest value, TParam P, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        // Private Methods (3) 

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Convert(ToTypedValue<TSrc>(value),
                                ToTypedValue<TParam>(parameter),
                                culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.ConvertBack(ToTypedValue<TDest>(value),
                                    ToTypedValue<TParam>(parameter),
                                    culture);
        }

        private static T ToTypedValue<T>(object value)
        {
            return value != null ? (T)value : default(T);
        }

        #endregion Methods
    }

    #endregion
}
