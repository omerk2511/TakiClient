using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Numerics;
using Taki.Cards;

namespace Taki
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Card[] hand = new Card[5];
            hand[0] = new PlusCard(Color.GREEN);
            hand[1] = new TwoPlusCard(Color.YELLOW);
            hand[2] = new PlusCard(Color.YELLOW);
            hand[3] = new TakiCard(Color.YELLOW);
            hand[4] = new TwoPlusCard(Color.YELLOW);

            hand[1].Serialize();

            //List<Card> hand = new List<Card>(5);
            //hand.Add(new NumberCard(1, Color.GREEN));
            //hand.Add(new NumberCard(3, Color.GREEN));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
