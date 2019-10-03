using System.Activities;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace QRCodeLib
{
    public class QRReadFromImage : CodeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [Description("An image object containing a QR code")]
        public InArgument<Image> QRImage { get; set; }

        [Category("Output")]
        [Description("The information decoded from the provided QR code")]
        public OutArgument<string> Message { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            Image image = QRImage.Get(context);
            string message = MessageExtractor.ExtractFromBitmap(image as Bitmap);

            Message.Set(context, message);
            Trace.TraceInformation("Finished reading QR code from image.");
        }
    }
}