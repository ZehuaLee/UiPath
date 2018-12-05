using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRWriter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void screenshoot()
        {

            System.Drawing.Size screen = new System.Drawing.Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            System.Drawing.Bitmap memoryImage = new System.Drawing.Bitmap(screen.Width, screen.Height);
            System.Drawing.Graphics memoryGraphics = System.Drawing.Graphics.FromImage(memoryImage);
            Point destPoint = new Point();
            Point startPoint = destPoint = new Point(100, 100);
            destPoint.X += 100;
            destPoint.Y += 100;
            screen.Height -= 50;
            screen.Width -= 50;
            memoryGraphics.CopyFromScreen(startPoint, destPoint, );
            memoryImage.Save("C:\\Users\\ritakuka\\Desktop\\testimg.png");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            screenshoot();
        }
    }
}
