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
    partial class Form1 : Form
    {
        private Game game;
    
        public Form1(Game game)
        {
            InitializeComponent();
            this.game = game;
        }

        // Get a rectangle bounds from a PointF array.
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
                              GetObject(imageName, Taki.Properties.Resources.Culture);

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

        private void PaintHand(Graphics gfx, string resourceName, int startX, int startY, int angleOffset, int radius, int cardAmount)
        {
            int x, y;
            int devisor = 180 / (cardAmount + 1);
            for (int i = angleOffset + devisor; i <= 180 - devisor + angleOffset; i += devisor)
            {
                int angle = i;
                double angleRadians = (Math.PI / 180) * angle;
                if (angle <= 90)
                {
                    x = (int)(Math.Cos(angleRadians) * radius * -1 + startX + (40 * -Math.Cos(angleRadians)));
                }
                else
                {
                    x = (int)(Math.Cos(angleRadians) * radius * -1 + startX - (80 * -Math.Cos(angleRadians)));

                }
                y = (int)(Math.Sin(angleRadians) * radius * -1 + startY);

                RotateBitmap(gfx, angle - 90, x, y, cardHeight, cardWidth, resourceName);
            }
            gfx.Transform = new Matrix();
        }

        public void RenderStashedCard(Graphics gfx, string imageName, int x, int y)
        {
            Random rnd = new Random();
            Bitmap bmp = (Bitmap)Taki.Properties.Resources.ResourceManager.
                              GetObject(imageName, Taki.Properties.Resources.Culture);

            int rndX = rnd.Next(-10, 10);
            int rndY = rnd.Next(-10, 10);
            Matrix matrix = new Matrix();
            matrix.RotateAt(rnd.Next(-7, 7), new PointF(x + rndX, y + rndY));
            gfx.Transform = matrix;
            gfx.DrawImage(bmp, x + rndX, y + rndY, cardWidth, cardHeight);
        }


        const int cardWidth = 100;
        const int cardHeight = 200;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            gfx.Clear(this.BackColor);
            PaintHand(gfx, "_1b", ((this.Width / 2) - 50), (this.Height * 8) / 9, 0, 200, 8);
            PaintHand(gfx, "back", -100, this.Height / 3, 90, 150, 8);
            PaintHand(gfx, "back", ((this.Width / 2) - 100), -150, -180, 150, 8);
            PaintHand(gfx, "back", this.Width, this.Height / 3, -90, 150, 8); 
            

            // Draw the deck
            int deckX = (this.Width - cardWidth) / 2 - cardWidth;
            int deckY = (this.Height - cardHeight) / 2;
            for (int i = 0; i < 20; i++)
            {
                RenderStashedCard(gfx, "back", deckX, deckY);
            }
            
            // Draw the used cards
            int usedCardsX = (this.Width - cardWidth) / 2 + cardWidth;
            int usedCardsY = (this.Height - cardHeight) / 2;
            for (int i = 0; i < this.game.usedCards.Count; i++)
            {
                RenderStashedCard(gfx, this.game.usedCards[i].GetResourceName(), usedCardsX, usedCardsY);
            }
        }
    }
}
