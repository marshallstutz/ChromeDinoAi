using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Dino
{
    public class scnShot
    {
        public Bitmap getScreenShot()
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                           Screen.PrimaryScreen.Bounds.Height,
                           PixelFormat.Format32bppArgb);
            //var bmpScreenshot = new Bitmap(1380,
            //               150,
            //               PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                0,
                                0,
                                Screen.PrimaryScreen.Bounds.Size,
                                CopyPixelOperation.SourceCopy);
            //gfxScreenshot.CopyFromScreen(1380,
            //                150,
            //        0,
            //        0,
            //        Screen.PrimaryScreen.Bounds.Size,
            //        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            //bmpScreenshot.Save("dinoShot.png", ImageFormat.Png);
            return bmpScreenshot;
        }
    }
}
