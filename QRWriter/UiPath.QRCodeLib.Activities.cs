using System;
using System.Activities;
using System.ComponentModel;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using System.Drawing;
using System.IO;
using UiPath.Core;

namespace QRCodeLib
{

    public class QRReadFromPath : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("The path of the qr code picture")]
        public InArgument<string> QRpath { get; set; }

        [Category("Output")]
        [Description("Message string hidden inside the qr code")]
        public OutArgument<string> Message { get; set; }

        /* This function is to read the information from a picture file
         * argument: string 
         * return: string
         */
        protected string QrReader(string imagePath)
        {
            string result = "";
            
            // extract the picture file into image.
            MultiFormatReader multiReader = new MultiFormatReader();
            Bitmap img = (Bitmap)Bitmap.FromFile(imagePath);
            if (img == null)
            {
                throw (new Exception("No image exists"));
            }

            // transfer the image into binarybitmap
            LuminanceSource ls = new BitmapLuminanceSource(img);
            BinaryBitmap bbmp = new BinaryBitmap(new HybridBinarizer(ls));

            // get the information hidden in Qr code.
            Result res = multiReader.decode(bbmp);
            result = res.ToString();
            return result;
        }

        protected override void Execute(CodeActivityContext context)
        {
            var ImagePath = QRpath.Get(context);
            var result = QrReader(ImagePath);
            Message.Set(context, result);
            Console.WriteLine($"QR code read from {ImagePath} finished.");
        }
    }



    public class QRReadFromImage : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("The image object of a qr code picture")]
        public InArgument<System.Drawing.Image> QRImage { get; set; }

        [Category("Output")]
        [Description("Message string hidden inside the qr code")]
        public OutArgument<string> Message { get; set; }

        /* This function is to read the information from a qr code image
         * argument: image 
         * return: string
         */
        protected string QrReader(System.Drawing.Image imageqr)
        {
            string result = "";

            MultiFormatReader multiReader = new MultiFormatReader();
            Bitmap img = (Bitmap)imageqr;
            if (img == null)
            {
                throw (new Exception("No image exists"));
            }
            LuminanceSource ls = new BitmapLuminanceSource(img);
            BinaryBitmap bbmp = new BinaryBitmap(new HybridBinarizer(ls));
            Result res = multiReader.decode(bbmp);
            result = res.ToString();
            return result;
        }

        protected override void Execute(CodeActivityContext context)
        {
            var Imageqr = QRImage.Get(context);
            var result = QrReader(Imageqr);
            Message.Set(context, result);
        }
    }

    
    public class QRReadFromUiElement : CodeActivity
    {
        [Category("Input")]
        [Description("The target UiElement containing qr code")]
        public InArgument<UiElement> UiElement { get; set; }
        
        [Category("Output")]
        [Description("Message string hidden inside the qr code")]
        public OutArgument<string> Message { get; set; }

        /* This function is to read the information from a qr code image
         * argument: image 
         * return: string
         */
        protected string QrReader(System.Drawing.Image imageqr)
        {
            string result = "";

            MultiFormatReader multiReader = new MultiFormatReader();
            Bitmap img = (Bitmap)imageqr;
            if (img == null)
            {
                throw (new Exception("No image exists"));
            }
            LuminanceSource ls = new BitmapLuminanceSource(img);
            BinaryBitmap bbmp = new BinaryBitmap(new HybridBinarizer(ls));
            Result res = multiReader.decode(bbmp);
            result = res.ToString();
            return result;
        }
        
        /*This function is to catch the wanted area according to UiElement
         * argument: UiElement
         * return: image 
         */
        private System.Drawing.Image screenshoot(UiElement ui) {

            // Catch the whole screen and save in memory
            Size screen = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            Bitmap memoryImage = new Bitmap(screen.Width, screen.Height);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            
            // position the rectangle area where QRcode exists 
            Point startPoint = new Point(0,0);
            Point destPoint = new Point(0,0);
            destPoint = ui.GetPosition().Rectangle.Value.Location;
            destPoint.X += ui.GetPosition().Rectangle.Value.Width;
            destPoint.Y += ui.GetPosition().Rectangle.Value.Height;
            startPoint = ui.GetPosition().Rectangle.Value.Location;

            // get only the specific rectangle that contains Qrcode
            memoryGraphics.CopyFromScreen( destPoint, startPoint, ui.GetPosition().Rectangle.Value.Size);
            return (System.Drawing.Image)memoryImage;
        }

        protected override void Execute(CodeActivityContext context)
        {
            UiElement uielem = UiElement.Get(context);
            string base64 = uielem.ImageBase64;
            byte[] bytes = Convert.FromBase64String(base64);
            System.Drawing.Image img = System.Drawing.Image.FromStream(new MemoryStream(bytes));
            var result = QrReader(img);
            Message.Set(context, result);
        }
    }

    public class QRWriteToImage : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("Message string to be hidden in qr code")]
        public InArgument<string> Message { get; set; }

        [Category("Input")]
        [Description("The width of output image")]
        public InArgument<int> Width { get; set; }

        [Category("Input")]
        [Description("The height of output image")]
        public InArgument<int> Height { get; set; }

        [Category("Output")]
        [Description("The output image of qr code")]
        public OutArgument<System.Drawing.Image> Result { get; set; }

        /*This function is to hide information to a size-specified qr code image.
         * argument: string, int, int
         * return: image 
         */
        protected System.Drawing.Image QrWriter(string info, int width = 0, int height = 0)
        {
            if (width <= 0 || height <= 0)
            {
                width = 250;
                height = 250;
            }
            BarcodeWriter writer = new BarcodeWriter();
            var options = new QrCodeEncodingOptions();
            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = width,
                Height = height,
            };
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            var bitmap = new Bitmap(writer.Write(info));
            return bitmap;
        }

        protected override void Execute(CodeActivityContext context)
        {
            var message = Message.Get(context);
            var width = Width.Get(context);
            var height = Height.Get(context);
            var Succeed = QrWriter(message, width, height);
            Result.Set(context, Succeed);
        }
    }

    public class QRWriteToPath : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [Description("The path to store the output qr code picture")]
        public InArgument<string> Path { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [Description("The message inside the qr code")]
        public InArgument<string> Message { get; set; }

        [Category("Input")]
        [Description("The width of output image")]
        public InArgument<int> Width { get; set; }

        [Category("Input")]
        [Description("The height of output image")]
        public InArgument<int> Height { get; set; }

        /*This function is to hide information to a size-specified qr code image.
         * argument: string, int, int
         * return: image 
         */
        protected System.Drawing.Image QrWriter(string info, int width = 0, int height = 0)
        {
            if (width <= 0 || height <= 0)
            {
                width = 250;
                height = 250;
            }
            BarcodeWriter writer = new BarcodeWriter();
            var options = new QrCodeEncodingOptions();
            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = width,
                Height = height,
            };
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            var bitmap = new Bitmap(writer.Write(info));
            return bitmap;
        }

        protected override void Execute(CodeActivityContext context)
        {
            var Imagepath = Path.Get(context);
            var message = Message.Get(context);
            var width = Width.Get(context);
            var height = Height.Get(context);
            var Succeed = QrWriter(message, width, height);
            Succeed.Save(Imagepath);
        }
    }

}
