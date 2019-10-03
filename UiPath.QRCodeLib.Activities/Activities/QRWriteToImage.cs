using System.Activities;
using System.ComponentModel;
using System.Drawing;

namespace QRCodeLib
{
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
}