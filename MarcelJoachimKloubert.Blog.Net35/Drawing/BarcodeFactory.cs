// s. http://blog.marcel-kloubert.de


using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ZXing;
using ZXing.QrCode;

namespace MarcelJoachimKloubert.Blog.Drawing
{
    /// <summary>
    /// Klasse zum Erstellen von Barcodes.
    /// </summary>
    public static class BarcodeFactory
    {
        #region Methods (5)

        // Public Methods (4) 

        /// <summary>
        /// Erstellt einen QR-Code aus einer Zeichenkette mit einer Größe
        /// von 640x640 Pixel.
        /// </summary>
        /// <param name="chars">Der Text aus dem der QR-Code erstellt werden soll.</param>
        /// <returns>Der QR-Code.</returns>
        public static Bitmap CreateQrCodeImage(IEnumerable<char> chars)
        {
            return CreateQrCodeImage(chars,
                                     640);
        }

        /// <summary>
        /// Erstellt einen QR-Code aus einer Zeichenkette mit einer bestimmten Größe.
        /// </summary>
        /// <param name="chars">Der Text aus dem der QR-Code erstellt werden soll.</param>
        /// <param name="widthAndHeight">Die Höhe und Breite des QR-Codes, in Pixel.</param>
        /// <returns>Der QR-Code.</returns>
        public static Bitmap CreateQrCodeImage(IEnumerable<char> chars,
                                               int widthAndHeight)
        {
            return CreateQrCodeImage(chars,
                                     widthAndHeight,
                                     widthAndHeight);
        }

        /// <summary>
        /// Erstellt einen QR-Code aus einer Zeichenkette mit einer bestimmten Größe.
        /// </summary>
        /// <param name="chars">Der Text aus dem der QR-Code erstellt werden soll.</param>
        /// <param name="size">Die Größe des QR-Codes, in Pixel.</param>
        /// <returns>Der QR-Code.</returns>
        public static Bitmap CreateQrCodeImage(IEnumerable<char> chars,
                                               Size size)
        {
            var matrix = new QRCodeWriter().encode(chars.AsString() ?? string.Empty,
                                                   BarcodeFormat.QR_CODE,
                                                   size.Width,
                                                   size.Height);

            return new BarcodeWriter().Write(matrix);
        }

        /// <summary>
        /// Erstellt einen QR-Code aus einer Zeichenkette mit einer bestimmten Größe.
        /// </summary>
        /// <param name="chars">Der Text aus dem der QR-Code erstellt werden soll.</param>
        /// <param name="width">Die Breite des QR-Codes, in Pixel.</param>
        /// <param name="height">Die Höhe des QR-Codes, in Pixel.</param>
        /// <returns>Der QR-Code.</returns>
        public static Bitmap CreateQrCodeImage(IEnumerable<char> chars,
                                               int width,
                                               int height)
        {
            return CreateQrCodeImage(chars,
                                     new Size(width,
                                              height));
        }
        // Private Methods (1) 

        private static string AsString(IEnumerable<char> chars)
        {
            if (chars == null)
            {
                return null;
            }

            if (chars is string)
            {
                return (string)chars;
            }

            if (chars is char[])
            {
                return new string((char[])chars);
            }

            return new string(chars.ToArray());
        }

        #endregion Methods
    }
}
