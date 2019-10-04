using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void ScreenShoot() {
            System.Drawing.Size screen = new System.Drawing.Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            System.Drawing.Bitmap memoryImage = new System.Drawing.Bitmap(200,200);
            System.Drawing.Graphics memoryGraphics = System.Drawing.Graphics.FromImage(memoryImage);
            Point destPoint = new Point(0,0);
            Point startPoint = new Point(0,0);
            destPoint.X += destPoint.X + 200;
            destPoint.Y += destPoint.Y + 200;
            memoryGraphics.CopyFromScreen(destPoint, startPoint, screen);
            memoryImage.Save("C:\\Users\\ritakuka\\Desktop\\testimg.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScreenShoot();
        }
    }
}
