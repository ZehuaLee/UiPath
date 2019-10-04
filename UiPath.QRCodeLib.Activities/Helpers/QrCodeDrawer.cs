using System;
using System.Drawing;
using ZXing;
using ZXing.QrCode;

namespace QRCodeLib
{
    internal static class QrCodeDrawer
    {
        public static Image Draw(string message, int width, int height)
        {
            if(string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (width <= 0)
            {
                width = 250;
            }

            if (height <= 0)
            {
                height = 250;
            }

            var options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = width,
                Height = height
            };
            var barcodeWriter = new BarcodeWriter
            { 
                Format = BarcodeFormat.QR_CODE,
                Options = options 
            };

            return barcodeWriter.Write(message);
        }
    }
}