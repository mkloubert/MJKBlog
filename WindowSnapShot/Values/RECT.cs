// s. http://blog.marcel-kloubert.de

// s. http://www.codeproject.com/Articles/19192/How-to-capture-a-Window-as-an-Image-and-save-it


using System;
using System.Runtime.InteropServices;

namespace MarcelJoachimKloubert.Blog.WindowSnapShot.Values
{
    /// <summary>
    /// Speichert die Position und Grösse eines Rechtecks.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        #region Data Members (6)

        /// <summary>
        /// Die äussere Y-Position (unten).
        /// </summary>
        public int Bottom;
        /// <summary>
        /// Die äussere X-Position (links).
        /// </summary>
        public int Left;
        /// <summary>
        /// Die äussere X-Position (rechts).
        /// </summary>
        public int Right;
        /// <summary>
        /// Die äussere Y-Position (oben).
        /// </summary>
        public int Top;
        /// <summary>
        /// Gibt die Höhe zurück.
        /// </summary>
        public int Height
        {
            get { return Math.Abs(this.Bottom - this.Top); }
        }

        /// <summary>
        /// Gibt die Breite zurück.
        /// </summary>
        public int Width
        {
            get { return Math.Abs(this.Right - this.Left); }
        }

        #endregion Data Members

        #region Methods (1)

        /// <summary>
        /// 
        /// </summary>
        /// <see cref="object.ToString()" />
        public override string ToString()
        {
            return string.Format("Left = {0}, Top = {1}, Right = {2}, Bottom ={3}",
                                 Left,
                                 Top,
                                 Right,
                                 Bottom);
        }

        #endregion Methods
    }
}
