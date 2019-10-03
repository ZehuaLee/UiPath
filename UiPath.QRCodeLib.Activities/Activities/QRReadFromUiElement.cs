using System.Activities;
using System.ComponentModel;
using System.Diagnostics;
using UiPath.Core;

namespace QRCodeLib
{
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
}