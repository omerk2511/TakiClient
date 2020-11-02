using Newtonsoft.Json.Linq;
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
using Taki.Cards;

namespace Taki
{
    partial class GameWindow : Form
    {
        // A class that indicates a card to animate. The struct contains 3 values:
        // int delay - When this variable reaches 0, the animation should start.
        // string imageName - the name of the image.
        // Point animationPoint - the current point that indicates the top left corner of the animated image.
        class AnimationData
        {
            public int delay { get; set; }
            public string imageName { get; set; }
            public Point animationPoint { get; set; }
            public AnimationData(int delay, string imageName, Point animationPoint)
            {
                this.delay = delay;
                this.imageName = imageName;
                this.animationPoint = animationPoint;
            }
        }

        // List of cards to animate
        private List<AnimationData> animationData = new List<AnimationData>();
        private bool animationStarted;

        private Game game;
        private Client client;
        private System.Timers.Timer timer;

        public GameWindow(Game game, Client client)
        {
            InitializeComponent();
            this.game = game;
            this.client = client;

            this.timer = new System.Timers.Timer(80);
            this.timer.Enabled = false;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);

        }
        
        public GameWindow()
        {
            InitializeComponent();

            this.timer = new System.Timers.Timer(80);
            this.timer.Enabled = false;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
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
                CardAnimation(card.GetResourceName());
            }
            else
            {
                // Update player's hand
                int playerIndex = this.game.GetPlayerIndex(player);
                switch(playerIndex)
                {
                    case 1:
                        this.Invalidate(new Rectangle(0, 0, this.Width / 3, this.Height));
                        break;
                    case 2:
                        this.Invalidate(new Rectangle(0, 0, this.Width, this.Height / 4));
                        break;
                    case 3:
                        this.Invalidate(new Rectangle((this.Width * 2) / 3, 0, this.Width / 3, this.Height));
                        break;
                }
            }
        }

        // Animate a card used by a player. If the card is known, card should contain
        // the card the player drew, if the card is unknown (e.g. non active player drew it)
        // card should contain null;
        public void AnimateUseCard(Player player, Card card)
        {
            // Update player's hand
            int playerIndex = this.game.GetPlayerIndex(player);
            switch (playerIndex)
            {
                case 0:
                    this.Invalidate(new Rectangle(0, (this.Height * 2) / 3, this.Width, this.Height / 3));
                    break;
                case 1:
                    this.Invalidate(new Rectangle(0, 0, this.Width / 3, this.Height));
                    break;
                case 2:
                    this.Invalidate(new Rectangle(0, 0, this.Width, this.Height / 4));
                    break;
                case 3:
                    this.Invalidate(new Rectangle((this.Width * 2) / 3, 0, this.Width / 3, this.Height));
                    break;
            }
            // Add an card to the used cards stash
            RenderStashedCard(this.CreateGraphics(), card.GetResourceName(), usedCardsX, usedCardsY);
        }

        const int cardWidth = 100;
        const int cardHeight = 200;
        int deckX, deckY;
        int usedCardsX, usedCardsY;

        bool firstPaint = true;
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
                if (shouldRender[2])
                {
                    resourcesNames = this.game.GetPlayer(2).GetCardResources();
                    PaintHand(gfx, resourcesNames, ((this.Width / 2) - 100), -150, -180, 150);
                }
                if (shouldRender[3])
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
                int off = 5;
                if(this.game.usedCards.Count < 5)
                {
                    off = this.game.usedCards.Count;
                }
                for (int i = this.game.usedCards.Count - off; i < this.game.usedCards.Count; i++)
                {
                    RenderStashedCard(gfx, this.game.usedCards[i].GetResourceName(), usedCardsX, usedCardsY);
                }
            }
            else {
                for(int i = 0; i < animationData.Count; i++)
                {
                    if (animationData[i].delay == 0)
                    {
                        Bitmap bmp = (Bitmap)Taki.Properties.Resources.ResourceManager.
                            GetObject(animationData[i].imageName, Taki.Properties.Resources.Culture);
                        gfx.DrawImage(bmp, animationData[i].animationPoint.X, animationData[i].animationPoint.Y, cardWidth, cardHeight);

                    }
                }

            }
            if (firstPaint)
            {
                Thread connectionThread = new Thread(HandleConnection);
                connectionThread.Start();

                firstPaint = false;

            }
        }

        private void GameWindow_Load(object sender, EventArgs e)
        {
            
        }

        private void HandleConnection()
        {
            while (true)
            {
                dynamic json = this.client.RecvJSON(true);
                Dictionary<string, object> values = ((JObject)json).ToObject<Dictionary<string, object>>();
                if (values.ContainsKey("status"))
                {
                    if(json.status == "success")
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Error" + json.ToString());
                    }
                }
                if (json.code == "update_turn")
                {
                    string currentPlayerName = json.args.current_player.ToString();
                    Player currentPlayer = this.game.GetPlayerByName(currentPlayerName);
                    if (currentPlayer is ActivePlayer)
                    {
                        Console.WriteLine("Its your turn.");
                        //Card cardPlayed = this.game.GetActivePlayer().PlayCard(0, this.client);
                        Thread.Sleep(7000);
                        List<Card> cardDrawn = this.game.GetActivePlayer().DrawCard(client);
                        foreach (Card card in cardDrawn)
                        {
                            AnimateDrawCard(currentPlayer, card);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Its " + currentPlayer.name + "'s turn.");
                    }
                }
                else if(json.code == "move_done")
                {
                    //dynamic moveDoneJson = this.client.RecvJSON(true);
                    if (json.args.type == "cards_taken")
                    {
                        Player currentPlayer = this.game.GetPlayerByName(json.args.player_name.ToString());
                        if(currentPlayer is NonActivePlayer)
                        {
                            ((NonActivePlayer)currentPlayer).AddCards(json.args.amount.ToObject<int>());
                        }
                        for (int i = 0; i < json.args.amount.ToObject<int>(); i++)
                        {
                            AnimateDrawCard(currentPlayer, null);
                        }
                    }
                }

            }
        }

        // Draw an card animation. The card image should be specified with cardImageName.
        private void CardAnimation(string cardImageName)
        {
            // delay to add before next card is drawn
            int delay = animationData.Count * 5; 

            animationData.Add(new AnimationData (delay, cardImageName, new Point(deckX, deckY + cardHeight + 10)));
               
            this.animationStarted = true;
            if(!this.timer.Enabled)
                this.timer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.animationStarted)
            {
                for(int i = 0; i < animationData.Count; i++ )
                {
                    if (animationData[i].delay == 0)
                    {
                        Point newPoint = animationData[i].animationPoint;
                        Rectangle reRenderRect = new Rectangle(newPoint.X, newPoint.Y, cardWidth, cardHeight);
                        newPoint.Y += 10;
                        animationData[i].animationPoint = newPoint;

                        if (newPoint.Y + cardHeight + 100 > this.Height)
                        {
                            animationData.RemoveAt(i);
                        }
                        else
                        {
                            this.Invalidate(reRenderRect);
                        }
                        if (animationData.Count == 0)
                        {
                            this.animationStarted = false;
                            this.timer.Stop();
                            this.Invalidate(new Rectangle(0, (this.Height * 2) / 3, this.Width, this.Height / 3));
                        }
                    }
                    else
                    {
                        animationData[i].delay -= 1;
                    }
                }
            }
        }

    }
}
