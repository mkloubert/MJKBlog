// s. http://blog.marcel-kloubert.de


using System.Drawing;
using System.Drawing.Imaging;

/// <summary>
/// Extension Methoden für Bild(-bearbeitungs)operationen (.NET 3.5).
/// </summary>
public static partial class __DrawingExtensionMethods35
{
    #region Methods (1)

    // Public Methods (1) 

    /// <summary>
    /// Wandelt die Farben eines <see cref="Bitmap" />s in Graustufen um.
    /// </summary>
    /// <param name="input">Das Eingabebild.</param>
    /// <returns>
    /// Das Graustufenbild oder <see langword="null" />, wenn <paramref name="input" />
    /// ebenfalls eine <see langword="null" /> Referenz ist.
    /// </returns>
    public static Bitmap Grayscale(this Bitmap input)
    {
        if (input == null)
        {
            return null;
        }

        var result = new Bitmap(input.Width, input.Height,
                                PixelFormat.Format32bppArgb);

        var bmpData1 = input.LockBits(new Rectangle(0, 0,
                                                    input.Width, input.Height),
                                      ImageLockMode.ReadOnly,
                                      PixelFormat.Format32bppArgb);

        var bmpData2 = result.LockBits(new Rectangle(0, 0,
                                                     result.Width, result.Height),
                                       ImageLockMode.ReadOnly,
                                       PixelFormat.Format32bppArgb);

        var a = 0;

        unsafe
        {
            var imgPointer1 = (byte*)bmpData1.Scan0;
            var imgPointer2 = (byte*)bmpData2.Scan0;

            for (var y = 0; y < bmpData1.Height; y++)
            {
                for (var x = 0; x < bmpData1.Width; x++)
                {
                    a = (imgPointer1[0] + imgPointer1[1] +
                         imgPointer1[2]) / 3;

                    imgPointer2[0] = (byte)a;
                    imgPointer2[1] = (byte)a;
                    imgPointer2[2] = (byte)a;
                    imgPointer2[3] = imgPointer1[3];

                    imgPointer1 += 4;
                    imgPointer2 += 4;
                }

                imgPointer1 += bmpData1.Stride -
                               (bmpData1.Width * 4);

                imgPointer2 += bmpData1.Stride -
                               (bmpData1.Width * 4);
            }

        }

        result.UnlockBits(bmpData2);
        input.UnlockBits(bmpData1);

        return result;
    }

    #endregion Methods
}
