// s. http://blog.marcel-kloubert.de


using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

/// <summary>
/// Extension Methoden für WPF.
/// </summary>
public static partial class __WPFExtensionMethods
{
    #region Methods (1)

    // Public Methods (1) 

    /// <summary>
    /// Gibt den aktuellen Inhalt eines WPF-Framework-Elements als Bitmap zurück.
    /// </summary>
    /// <param name="element">Das zugrundeliegende Element.</param>
    /// <returns>
    /// Der Inhalt des Elements als Bitmap oder <see langword="null" />, wenn
    /// <paramref name="element" /> keine Breite und/oder keine Höhe hat.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="element" /> ist eine <see langword="null" /> Referenz.
    /// </exception>
    public static RenderTargetBitmap TakeScreenshot(this FrameworkElement element)
    {
        if (element == null)
        {
            throw new ArgumentNullException("element");
        }

        var size = new Size(element.ActualWidth, element.ActualHeight);
        if (size.IsEmpty ||
            (int)size.Width == 0 || (int)size.Height == 0)
        {
            return null;
        }

        var result = new RenderTargetBitmap((int)size.Width, (int)size.Height,
                                            96, 96,
                                            PixelFormats.Pbgra32);

        var drawingvisual = new DrawingVisual();
        using (var context = drawingvisual.RenderOpen())
        {
            context.DrawRectangle(new VisualBrush(element), null, new Rect(new Point(), size));
            context.Close();
        }

        result.Render(drawingvisual);
        return result;
    }

    #endregion Methods
}
