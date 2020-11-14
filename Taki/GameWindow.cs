using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

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

        private int currentPlayerTurn = -1;

        // List of cards to animate
        private List<AnimationData> animationData = new List<AnimationData>();
        private bool animationStarted;

        private Game game;
        private Client client;
        private System.Timers.Timer timer;

        private Tuple<Point, int, int, Rectangle>[] playerRenderInfo;

        private Rectangle colorRect;

        public GameWindow(Game game, Client client)
        {
            InitializeComponent();
            this.game = game;
            this.client = client;

            this.timer = new System.Timers.Timer(80);
            this.timer.Enabled = false;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);

            this.playerRenderInfo = new Tuple<Point, int, int, Rectangle>[4];
            this.playerRenderInfo[0] = new Tuple<Point, int, int, Rectangle>(new Point(this.Width / 2 - 50, this.Height * 8 / 9), 0, 200, new Rectangle(0, this.Height - 200, this.Width, 200)); // Down
            this.playerRenderInfo[1] = new Tuple<Point, int, int, Rectangle>(new Point(-100, this.Height / 3), 90, 150, new Rectangle(0, 0, 200, this.Height)); // Left
            this.playerRenderInfo[2] = new Tuple<Point, int, int, Rectangle>(new Point(this.Width / 2 - 100, -150), -180, 150, new Rectangle(0, 0, this.Width + 500, 200)); // Up
            this.playerRenderInfo[3] = new Tuple<Point, int, int, Rectangle>(new Point(this.Width, this.Height / 3), -90, 150, new Rectangle(this.Width - 200, 0, 200, this.Height)); // Right

            this.deckX = (this.Width - cardWidth) / 2 - cardWidth;
            this.deckY = this.Height / 2 - cardHeight;

            this.colorRect = new Rectangle(deckX + 10, deckY + cardHeight + 20, 50, 50);
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
            Matrix rotateAtOrigin = new Matrix();
            rotateAtOrigin.Rotate(angle);

            // Rotate the image's corners to see how big
            // it will be after rotation.
            PointF[] points =
            {
                new PointF(x, y),
                new PointF(x + w, y),
                new PointF(x + w, y + h),
                new PointF(x, y + h),
            };
            rotateAtOrigin.TransformPoints(points);

            float xmin, xmax, ymin, ymax;
            GetPointBounds(points, out xmin, out xmax,
                out ymin, out ymax);

            // Make a bitmap to hold the rotated result.
            int wid = (int)Math.Round(xmax - xmin);
            int hgt = (int)Math.Round(ymax - ymin);
            Bitmap result = new Bitmap(wid, hgt);

            // Create the real rotation transformation.
            Matrix rotateAtCenter = new Matrix();
            rotateAtCenter.RotateAt(angle,
                new PointF(x + wid / 2f, y + hgt / 2f));

            // Draw the image onto the new bitmap rotated.
            // Use smooth image interpolation.
            gr.InterpolationMode = InterpolationMode.High;

            gr.Transform = rotateAtCenter;

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
            if (player is ActivePlayer)
            {
                CardAnimation(card.GetResourceName());
            }
            else
            {
                // Update player's hand
                int playerIndex = this.game.GetPlayerIndex(player);
                InvalidatePlayer(playerIndex);
            }
        }

        // Animate a card used by a player. If the card is known, card should contain
        // the card the player drew, if the card is unknown (e.g. non active player drew it)
        // card should contain null;
        public void AnimateUseCard(Player player, Card card)
        {
            // Update player's hand
            int playerIndex = this.game.GetPlayerIndex(player);
            InvalidatePlayer(playerIndex);
            // Add an card to the used cards stash
            RenderStashedCard(this.CreateGraphics(), card.GetResourceName(), usedCardsX, usedCardsY);
        }

        private void InvalidatePlayer(int playerIndex)
        {
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

                // Point - render point,
                // int - angleOffset,
                // int - radius,
                // Point - backgound render point 

                // Draw the hands
                List<string> resourcesNames;
                for (int i = 0; i < 4; i++)
                {
                    if (true) // TODO: chagne to false if a user left the game
                    {
                        resourcesNames = this.game.GetPlayer(i).GetCardResources();
                        if (currentPlayerTurn == i)
                        {
                            Rectangle currntPlayerHighlightRect = playerRenderInfo[i].Item4;
                            gfx.FillRectangle(new SolidBrush(System.Drawing.Color.Green), currntPlayerHighlightRect);
                        }

                        PaintHand(gfx, resourcesNames, playerRenderInfo[i].Item1.X, playerRenderInfo[i].Item1.Y, playerRenderInfo[i].Item2, playerRenderInfo[i].Item3);
                    }
                }

                // Draw the deck
                for (int i = 0; i < 20; i++)
                {
                    RenderStashedCard(gfx, "back", deckX, deckY);
                }

                // Draw the used cards
                usedCardsX = (this.Width - cardWidth) / 2 + cardWidth;
                usedCardsY = this.Height / 2 - cardHeight;
                int off = 5;
                if (this.game.usedCards.Count < 5)
                {
                    off = this.game.usedCards.Count;
                }
                for (int i = this.game.usedCards.Count - off; i < this.game.usedCards.Count; i++)
                {
                    RenderStashedCard(gfx, this.game.usedCards[i].GetResourceName(), usedCardsX, usedCardsY);
                }
                // Render current color circle
                if (this.game.CurrentColor != Color.UNDEFINED)
                {
                    gfx.Transform = new Matrix();
                    gfx.FillEllipse(new SolidBrush(System.Drawing.Color.FromName(this.game.CurrentColor.ToString().ToLower())), this.colorRect);
                }
            }
            else
            {
                for (int i = 0; i < animationData.Count; i++)
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

        private void HandleConnection()
        {
            Bot bot = new Bot(this.game, client, this);
            while (true)
            {
                dynamic json = this.client.RecvJSON(true);
                Dictionary<string, object> values = ((JObject)json).ToObject<Dictionary<string, object>>();
                if (values.ContainsKey("status"))
                {
                    if (json.status == "success")
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
                    currentPlayerTurn = this.game.GetPlayerIndex(currentPlayer);
                    InvalidatePlayer(currentPlayerTurn);

                    if (currentPlayer is ActivePlayer)
                    {
                        Console.WriteLine("Its your turn.");
                        // Make a turn: Draw or use card with ActivePlayer.PlayCard() or ActivePlayer.DrawCard(), then animate 
                        // the turn using AnimateDrawCard() or AnimateUseCard()

                        Thread.Sleep(7000);
                        List<Card> cardsToPlay = new List<Card>();
                        bot.DoTurn(cardsToPlay);
                    }
                    else
                    {
                        Console.WriteLine("Its " + currentPlayer.name + "'s turn.");
                    }
                }
                else if (json.code == "move_done")
                {
                    Player currentPlayer = this.game.GetPlayerByName(json.args.player_name.ToString());
                    this.currentPlayerTurn = -1;
                    InvalidatePlayer(currentPlayerTurn);

                    if (json.args.type == "cards_taken")
                    {
                        if (currentPlayer is NonActivePlayer player)
                        {
                            player.AddCards(json.args.amount.ToObject<int>());
                        }
                        for (int i = 0; i < json.args.amount.ToObject<int>(); i++)
                        {
                            AnimateDrawCard(currentPlayer, null);
                        }
                        if (game.IsTwoPlusActive)
                        {
                            game.IsTwoPlusActive = false;
                        }
                    }
                    else if (json.args.type == "cards_placed")
                    {
                        List<JSONCard> used = ((JArray)json.args.cards).ToObject<List<JSONCard>>();
                        if (currentPlayer is NonActivePlayer player)
                        {
                            player.RemoveCards(used.Count);
                        }
                        foreach (JSONCard jsonCard in used)
                        {
                            // TODO: Dont let the user use a non ColorCard
                            Card card = game.GetActivePlayer().ConvertJsonCardToCard(jsonCard);
                            if (card is ColorCard colorCard)
                            {
                                game.usedCards.Add(colorCard);

                            }
                            AnimateUseCard(currentPlayer, card);
                        }
                        if (used.Last().type == "plus_2")
                        {
                            game.IsTwoPlusActive = true;
                        }

                        Color lastColor = game.CurrentColor;
                        game.CurrentColor = (Color)Enum.Parse(typeof(Color), used.Last().color.ToUpper());
                        if (lastColor != game.CurrentColor)
                        {
                            this.Invalidate(this.colorRect);
                        }
                    }
                }
                if (json.code == "game_ended")
                {
                    List<string> scoreboard = new List<string>();
                    scoreboard = ((JArray)json.args.scoreboard).ToObject<List<string>>();
                    MessageBox.Show(scoreboard[0] + "\n" + scoreboard[1] + "\n" + scoreboard[2] + "\n" + scoreboard[3] + "\n","the result");
                }
            }
        }

        private void GameWindow_Load_1(object sender, EventArgs e)
        {

        }

        private void GameWindow_Load_2(object sender, EventArgs e)
        {

        }

        // Draw an card animation. The card image should be specified with cardImageName.
        private void CardAnimation(string cardImageName)
        {
            // delay to add before next card is drawn
            int delay = animationData.Count * 5;

            animationData.Add(new AnimationData(delay, cardImageName, new Point(deckX, deckY + cardHeight + 10)));

            this.animationStarted = true;
            if (!this.timer.Enabled)
                this.timer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.animationStarted)
            {
                for (int i = 0; i < animationData.Count; i++)
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
