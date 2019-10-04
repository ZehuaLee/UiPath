using System.Activities;
using System.ComponentModel;
using Image = System.Drawing.Image;

namespace QRCodeLib
{
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
