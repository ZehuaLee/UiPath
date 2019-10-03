using System.Activities;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using UiPath.Core;
using Image = System.Drawing.Image;

namespace QRCodeLib
{
    public class QRReadFromPath : CodeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [Description("A path to an image file containing a QR code")]
        public InArgument<string> QRImage { get; set; }

        [Category("Output")]
        [Description("The information decoded from the provided QR code")]
        public OutArgument<string> Message { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string path = QRImage.Get(context);
            string message = MessageExtractor.ExtractFromPath(path);

            Message.Set(context, message);
            Trace.TraceInformation($"Finished reading QR code from path: '{path}'.");
        }
    }

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
    
    public class QRReadFromUiElement : CodeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [Description("The target UiElement containing a QR code")]
        public InArgument<UiElement> UiElement { get; set; }
        
        [Category("Output")]
        [Description("Message string hidden inside the qr code")]
        public OutArgument<string> Message { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            UiElement uiElement = UiElement.Get(context);
            string result = MessageExtractor.ExtractFromUiElement(uiElement);

            Message.Set(context, result);
            Trace.TraceInformation($"Finished reading QR code from UiElement: {uiElement.Selector}.");
        }
    }

    public class QRWriteToImage : CodeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [Description("The message to be encoded in the QR code")]
        public InArgument<string> Message { get; set; }

        [Category("Input")]
        [Description("The width of the output image")]
        [DefaultValue(250)]
        public InArgument<int> Width { get; set; }

        [Category("Input")]
        [Description("The height of the output image")]
        [DefaultValue(250)]
        public InArgument<int> Height { get; set; }

        [Category("Output")]
        [Description("The output image")]
        public OutArgument<Image> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string message = Message.Get(context);
            int width = Width.Get(context);
            int height = Height.Get(context);

            Image image = QrCodeDrawer.Draw(message, width, height);
            Result.Set(context, image);
        }
    }

    public class QRWriteToPath : CodeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [Description("The message to be encoded in the QR code")]
        public InArgument<string> Message { get; set; }

        [RequiredArgument]
        [Category("Input")]
        [Description("The path to save the output image")]
        public InArgument<string> Path { get; set; }

        [Category("Input")]
        [Description("The width of the output image")]
        public InArgument<int> Width { get; set; }

        [Category("Input")]
        [Description("The height of the output image")]
        public InArgument<int> Height { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string path = Path.Get(context);
            string message = Message.Get(context);
            int width = Width.Get(context);
            int height = Height.Get(context);

            using (Image image = QrCodeDrawer.Draw(message, width, height))
            {
                image.Save(path);
            }
        }
    }
}
