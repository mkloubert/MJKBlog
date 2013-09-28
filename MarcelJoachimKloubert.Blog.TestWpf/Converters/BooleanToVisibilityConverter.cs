// s. http://blog.marcel-kloubert.de


using System.Globalization;
using System.Windows;
using MarcelJoachimKloubert.Blog.Data;

namespace MarcelJoachimKloubert.Blog.TestWpf.Converters
{
    public sealed class BooleanToVisibilityConverter : ValueConverterBase<bool?, Visibility, string>
    {
        #region Methods (1)

        // Public Methods (1) 

        public override Visibility Convert(bool? value, string parameter, CultureInfo culture)
        {
            Visibility invisibleValue;
            switch ((parameter ?? string.Empty).ToLower().Trim())
            {
                case "collapsed":
                    invisibleValue = Visibility.Collapsed;
                    break;

                default:
                    invisibleValue = Visibility.Hidden;
                    break;
            }

            if (value.HasValue)
            {
                return value.Value ? Visibility.Visible : invisibleValue;
            }

            return Visibility.Collapsed;
        }

        #endregion Methods
    }
}
