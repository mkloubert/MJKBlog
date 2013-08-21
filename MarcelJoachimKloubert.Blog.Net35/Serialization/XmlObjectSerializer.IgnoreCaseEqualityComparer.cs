// s. http://blog.marcel-kloubert.de


using System.Collections.Generic;

static partial class XmlObjectSerializer
{
    #region Nested Classes (1)

    private sealed class __IgnoreCaseEqualityComparer : EqualityComparer<string>
    {
        #region Methods (3)

        // Public Methods (2) 

        public override bool Equals(string x, string y)
        {
            return Normalize(x) == Normalize(y);
        }

        public override int GetHashCode(string obj)
        {
            return Normalize(obj).GetHashCode();
        }
        // Private Methods (1) 

        private static string Normalize(string str)
        {
            return (str ?? string.Empty).ToLower().Trim();
        }

        #endregion Methods
    }
    #endregion Nested Classes
}
