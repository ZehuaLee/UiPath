using System.Activities;
using System.ComponentModel;
using System.Diagnostics;

namespace QRCodeLib
{
    public class QRReadFromPath : CodeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [Description("A path to an image file containing a QR code")]
        public InArgument<string> QRpath { get; set; }

        [Category("Output")]
        [Description("The information decoded from the provided QR code")]
        public OutArgument<string> Message { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            string path = QRpath.Get(context);
            string message = MessageExtractor.ExtractFromPath(path);

            Message.Set(context, message);
            Trace.TraceInformation($"Finished reading QR code from path: '{path}'.");
        }
    }
}