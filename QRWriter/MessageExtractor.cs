using System;
using System.Drawing;
using System.IO;
using UiPath.Core;
using ZXing;
using ZXing.Common;
using Image = System.Drawing.Image;

namespace QRCodeLib
{
    internal static class MessageExtractor
    {
        public static string ExtractFromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap));
            }

            var binaryBitmap = new BinaryBitmap(new HybridBinarizer(new BitmapLuminanceSource(bitmap)));
            var multiFormatReader = new MultiFormatReader();
            Result decodedMessage = multiFormatReader.decode(binaryBitmap);

            return decodedMessage.ToString();
        }

        public static string ExtractFromPath(string pathToQrCodeImage)
        {
            using (var bitmap = (Bitmap)Image.FromFile(pathToQrCodeImage))
            {
                return ExtractFromBitmap(bitmap);
            }
        }

        public static string ExtractFromUiElement(UiElement uiElement)
        {
            byte[] bytes = Convert.FromBase64String(uiElement.ImageBase64);
            using (var memoryStream = new MemoryStream(bytes))
            using (var bitmap = (Bitmap)Image.FromStream(memoryStream))
            {
                return ExtractFromBitmap(bitmap);
            }
        }
    }
}
