using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace Miku.Client.Helpers
{
    public class DestopScreenHelper
    {
        public static Bitmap GetScreenImage( bool full,Point point01 , Point point02)
        {
            IntPtr dc1 = Win32API.CreateDC("DISPLAY", null, null, (IntPtr)null);

            Graphics g1 = Graphics.FromHdc(dc1);
            Bitmap MyImage;
            int width, height;
            if (full)
            {
                width = Screen.PrimaryScreen.Bounds.Width;
                height = Screen.PrimaryScreen.Bounds.Height;
            }
            else
            {
                width = point02.X - point01.X;
                height = point02.Y - point02.Y;
            }

            MyImage = new Bitmap(width, height, g1);

            Graphics g2 = Graphics.FromImage(MyImage);

            IntPtr dc3 = g1.GetHdc();

            IntPtr dc2 = g2.GetHdc();

            Win32API.BitBlt(dc2, 0, 0, width, height, dc3, point01.X, point01.Y, 13369376);

            g1.ReleaseHdc(dc3);
            g2.ReleaseHdc(dc2);

            return MyImage;
        }

        public static byte[] ChangeSize(byte[] picSource,int width = 100,int height = 100)
        {
            MemoryStream ms = new MemoryStream(picSource, 0, picSource.Length);
            Image oldPic = Image.FromStream(ms);
            ms = new MemoryStream();
            using (Bitmap newPic = new Bitmap(width,height))
            {
                using (Graphics canvas = Graphics.FromImage(newPic))
                {
                    canvas.DrawImage(oldPic, new Rectangle(new Point(0, 0), new Size(100, 100)));
                    newPic.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
        }
    }
}
