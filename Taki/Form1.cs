using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taki
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GetPointBounds(PointF[] points,
        out float xmin, out float xmax,
        out float ymin, out float ymax)
            {
                xmin = points[0].X;
                xmax = xmin;
                ymin = points[0].Y;
                ymax = ymin;
                foreach (PointF point in points)
                {
                    if (xmin > point.X) xmin = point.X;
                    if (xmax < point.X) xmax = point.X;
                    if (ymin > point.Y) ymin = point.Y;
                    if (ymax < point.Y) ymax = point.Y;
                }
            }
        private Bitmap RotateBitmap(Graphics gr, float angle, int x, int y, int h, int w, string imageName)
        {
            Bitmap bm = (Bitmap)Taki.Properties.Resources.ResourceManager.
                              GetObject("_" + imageName, Taki.Properties.Resources.Culture);

            // Make a Matrix to represent rotation
            // by this angle.
            Matrix rotate_at_origin = new Matrix();
            rotate_at_origin.Rotate(angle);

            // Rotate the image's corners to see how big
            // it will be after rotation.
            PointF[] points =
            {
                new PointF(x, y),
                new PointF(x + w, y),
                new PointF(x + w, y + h),
                new PointF(x, y + h),
            };
            rotate_at_origin.TransformPoints(points);
            float xmin, xmax, ymin, ymax;
            GetPointBounds(points, out xmin, out xmax,
                out ymin, out ymax);

            // Make a bitmap to hold the rotated result.
            int wid = (int)Math.Round(xmax - xmin);
            int hgt = (int)Math.Round(ymax - ymin);
            Bitmap result = new Bitmap(wid, hgt);

            // Create the real rotation transformation.
            Matrix rotate_at_center = new Matrix();
            rotate_at_center.RotateAt(angle,
                new PointF(x + wid / 2f, y + hgt / 2f));

            // Draw the image onto the new bitmap rotated.
            // Use smooth image interpolation.
            gr.InterpolationMode = InterpolationMode.High;

            gr.Transform = rotate_at_center;

            // Draw the image centered on the bitmap.
            x += (wid - w) / 2;
            y += (hgt - h) / 2;
            gr.DrawImage(bm, x, y, w, h);

            // Return the result bitmap.
            return result;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);
            int radius = 200;
            int circleCenterX = ((this.Width / 2) - 50);
            int circleCenterY = (this.Height * 5) / 6;
            int x, y;

            int cardAmount = 10;
            int devisor = 180 / (cardAmount + 1);

            for (int i = devisor; i <= 180 - devisor; i += devisor)
            {
                int angle = i;
                double angleRadians = (Math.PI / 180) * angle;
                if(angle <= 90)
                {
                    x = (int)(Math.Cos(angleRadians) * radius * - 1 + circleCenterX + (40 * -Math.Cos(angleRadians)));
                }
                else
                {
                    x = (int)(Math.Cos(angleRadians) * radius * -1 + circleCenterX - (80 * -Math.Cos(angleRadians)));

                }
                y = (int)(Math.Sin(angleRadians) * radius * - 1 + circleCenterY);

                RotateBitmap(e.Graphics, angle - 90, x, y, 200, 100, "1b");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
        private void timer1_Tick(object sender, EventArgs e)
        {
        }
    }
}
