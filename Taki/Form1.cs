using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

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

        private void PaintHand(Graphics gfx, List<string> resourcesName, int startX, int startY, int angleOffset, int radius)
        {
            int x, y;
            int devisor = 180 / (resourcesName.Count() + 1);
            //for (int i = angleOffset + devisor; i <= 180 - devisor + angleOffset; i += devisor)
            int angle = angleOffset + devisor;
            for (int i = 0; i < resourcesName.Count(); i += 1)
            {
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

                RotateBitmap(gfx, angle - 90, x, y, cardHeight, cardWidth, resourcesName[i]);
                angle += devisor;
            }
            gfx.Transform = new Matrix();
        }

        public Bitmap RenderStashedCard(Graphics gfx, string imageName, int x, int y)
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
            return bmp;
        }


        // Animate a card drew from the deck, if the card is known, card should contain
        // the card the player drew, if the card is unknown (e.g. non active player drew it)
        // card should contain null;
        public void AnimateDrawCard(Player player, Card card)
        {
            if(player is ActivePlayer)
            {
                CardAnimation(card.GetResourceName(), true);
            }
            else
            {
                // Update player's hand
            }
        }

        // Animate a card used by a player. If the card is known, card should contain
        // the card the player drew, if the card is unknown (e.g. non active player drew it)
        // card should contain null;
        public void AnimateUseCard(Player player, Card card)
        {
            if (player is ActivePlayer)
            {
                CardAnimation(card.GetResourceName(), false);
            }
            else
            {
                // Update player's hand
            }
        }


        const int cardWidth = 100;
        const int cardHeight = 200;
        int deckX, deckY;
        int usedCardsX, usedCardsY;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            if (!animationStarted)
            {
                gfx.Clear(this.BackColor);

                // Draw the hands
                bool[] shouldRender = { false, false, false, false };
                for(int i = 0; i < this.game.GetPlayersAmount(); i++)
                {
                    shouldRender[i] = true;
                }
                List<string> resourcesNames;
                if (shouldRender[0]) 
                {
                    resourcesNames = this.game.GetPlayer(0).GetCardResources();
                    PaintHand(gfx, resourcesNames, ((this.Width / 2) - 50), (this.Height * 8) / 9, 0, 200);
                }
                if (shouldRender[1])
                {
                    resourcesNames = this.game.GetPlayer(1).GetCardResources();
                    PaintHand(gfx, resourcesNames, -100, this.Height / 3, 90, 150);
                }
                if (shouldRender[1])
                {
                    resourcesNames = this.game.GetPlayer(2).GetCardResources();
                    PaintHand(gfx, resourcesNames, ((this.Width / 2) - 100), -150, -180, 150);
                }
                if (shouldRender[1])
                {
                    resourcesNames = this.game.GetPlayer(3).GetCardResources();
                    PaintHand(gfx, resourcesNames, this.Width, this.Height / 3, -90, 150);
                }
                // Draw the deck
                deckX = (this.Width - cardWidth) / 2 - cardWidth;
                deckY = this.Height / 2 - cardHeight;
                for (int i = 0; i < 20; i++)
                {
                    RenderStashedCard(gfx, "back", deckX, deckY);
                }
                
                // Draw the used cards
                usedCardsX = (this.Width - cardWidth) / 2 + cardWidth;
                usedCardsY = this.Height / 2 - cardHeight;
                for (int i = this.game.usedCards.Count - 5; i < this.game.usedCards.Count; i++)
                {
                    RenderStashedCard(gfx, this.game.usedCards[i].GetResourceName(), usedCardsX, usedCardsY);
                }
            }
            else {
                Bitmap bmp = (Bitmap)Taki.Properties.Resources.ResourceManager.
                  GetObject(this.animationImageName, Taki.Properties.Resources.Culture);
                gfx.DrawImage(bmp, currentAnimationPoint.X, currentAnimationPoint.Y, cardWidth, cardHeight);
            }
        }

        // Animation variables
        private Point currentAnimationPoint;
        private bool animationStarted;
        private string animationImageName;
        private bool animationDirection;

        // Draw an card animation. The card image should be specified with cardImageName.
        // animationDirection should be true if the card animation needs to move from the deck
        // to the user hand, false if the card animation needs to move from the hand to used
        // cards pile
        private void CardAnimation(string cardImageName, bool animationDirection)
        {
            this.animationDirection = animationDirection;
            if (animationDirection)
            {
                currentAnimationPoint = new Point(deckX, deckY + cardHeight + 10);
            }
            else
            {
                currentAnimationPoint = new Point(usedCardsX, this.Height - cardWidth - 400);
            }
            
            this.animationStarted = true;
            this.animationImageName = cardImageName;
            this.timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.animationStarted)
            {
                Rectangle reRenderRect = new Rectangle(currentAnimationPoint.X, currentAnimationPoint.Y, cardWidth, cardHeight);

                if (this.animationDirection)
                {
                    currentAnimationPoint.Y += 10;
                    if (currentAnimationPoint.Y + cardHeight + 100 > this.Height)
                    {
                        this.timer1.Enabled = false;
                        this.animationStarted = false;
                        this.Invalidate(new Rectangle(0, (this.Height * 2) / 3, this.Width, this.Height / 3));
                    }
                    else
                    {
                        this.Invalidate(reRenderRect);
                    }
                }
                else
                {
                    currentAnimationPoint.Y -= 10;
                    if (currentAnimationPoint.Y < deckY)
                    {
                        this.timer1.Enabled = false;
                        this.animationStarted = false;
                        this.Invalidate(new Rectangle(usedCardsX - 30, usedCardsY - 30, cardWidth + 60, cardHeight + 60));
                        this.Invalidate(new Rectangle(0, (this.Height * 2) / 3, this.Width, this.Height / 3));
                    }
                    else
                    {
                        this.Invalidate(reRenderRect);
                    }
                }
            }
        }

    }
}
