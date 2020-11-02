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
using System.Text.Json;
using Newtonsoft.Json;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new WelcomeForm());
            string[] players = new string[4];
            for (int i = 0; i < 4; i++)
            {
                players[i] = "lolxd" + i.ToString();
            }

            List<JSONCard> activePlayerCards = new List<JSONCard>();
            for (int i = 0; i < 8; i++)
            {
                activePlayerCards.Add(new JSONCard("number_card", "red", "3"));
            }
            

            //Game game = new Game(players, activePlayerCards);
            Application.Run(new WelcomeForm());
        }
    }
}
